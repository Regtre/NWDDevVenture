using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NWDFoundation.Configuration;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;

namespace NWDWebStandard.Controllers
{
    // https://accedia.com/blog/dotnetifying-sign-in-with-apple/
    
    // https://blog.martincostello.com/sign-in-with-apple-prototype-for-aspnet-core/

    // https://medium.com/identity-beyond-borders/how-to-configure-sign-in-with-apple-77c61e336003

    // https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers

    // https://sarunw.com/posts/sign-in-with-apple-4/   
    // https://www.scottbrady91.com/openid-connect/implementing-sign-in-with-apple-in-aspnet-core
    // https://arfasoftech.com/blog/How-to-Integrate-Sign-in-with-Apple-in-PHP-(5-minute-code)

    public partial class NWDAccountController
    {
        public static string AppleOAuth_Redirection()
        {
            string rReturn = "#";
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.AppleServiceId) == false
                && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.AppleTeamId) == false
                && string.IsNullOrEmpty(AppleKeyValueToString()) == false
                && NWDWebStandardConfiguration.KConfig.AppleKeyValue.Length > 0
                && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.AppleKeyId) == false
               )
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" + nameof(AppleRedirect) + "/";
            }
            return rReturn;
        }

        public static string AppleOAuth_URL()
        {
            string rReturn =
                "https://appleid.apple.com/auth/authorize?"
                + "client_id=" + NWDWebStandardConfiguration.KConfig.AppleServiceId
                //+ "&redirect_uri=" + HttpUtility.UrlEncode(AppleOAuth_Redirection())
                + "&redirect_uri=" + AppleOAuth_Redirection()
                + "&response_type=code%20id_token"
                + "&scope=name%20email"
                + "&response_mode=form_post"
                + "&state=123"
                + "";
            return rReturn;
        }

        public class Apple
        {
            public class User
            {
                public string id { set; get; } = string.Empty;
                public Name name { set; get; } = new Name();
                public string email { set; get; } = string.Empty;
            }

            public class Name
            {
                public string firstName { set; get; } = string.Empty;
                public string lastName { set; get; } = string.Empty;
            }
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> AppleRedirect(string code) // don't change name!
        {
            try
            {
                Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                {
                    { "code", code },
                    { "client_id", NWDWebStandardConfiguration.KConfig.AppleServiceId },
                    { "client_secret", CreateSecretKey() },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", AppleOAuth_Redirection() },
                };
                FormUrlEncodedContent tFormContent = new FormUrlEncodedContent(tRequestValues);
                HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.PostAsync("https://appleid.apple.com/auth/token", tFormContent);
                string tResponseString = await tResponse.Content.ReadAsStringAsync();
                try
                {
                    var tTokenHandler = new JwtSecurityTokenHandler();
                    string tTokenId = JObject.Parse(tResponseString)["id_token"]?.ToString();
                    var tJwtSecurityToken = tTokenHandler.ReadJwtToken(tTokenId);
                    var tClaims = tJwtSecurityToken.Claims;
                    //Get the expiration of the token and convert its value from unix time seconds to DateTime object
                    string? tTrustedId = tClaims.FirstOrDefault(x => x.Type == "sub").Value;
                    string? tAccessToken = JObject.Parse(tResponseString)["access_token"]?.ToString();
                    if (tAccessToken != null)
                    {
                        try
                        {
                            var tCclaims = tJwtSecurityToken.Claims;
                            SecurityKey tPublicKey;
                            //Get the expiration of the token and convert its value from unix time seconds to DateTime object
                            var tExpirationClaim = tCclaims.FirstOrDefault(x => x.Type == "exp").Value;
                            var tExpirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tExpirationClaim)).DateTime;

                            if (tExpirationTime < DateTime.UtcNow)
                            {
                                throw new SecurityTokenExpiredException("Expired token");
                            }

                            //Request Apple's JWKS used for verifying the tokens.
                            var tApplePublicKeys = NWDWebRuntimeConfiguration.HttpClientShared.GetAsync("https://appleid.apple.com/auth/keys");
                            var tKeyset = new JsonWebKeySet(tApplePublicKeys.Result.Content.ReadAsStringAsync().Result);
                            //Since there is more than one JSON Web Key we select the one that has been used for our token.
                            //This is achieved by filtering on the "Kid" value from the header of the token
                            tPublicKey = tKeyset.Keys.FirstOrDefault(x => x.Kid == tJwtSecurityToken.Header.Kid);
                            //Create new TokenValidationParameters object which we pass to ValidateToken method of JwtSecurityTokenHandler.
                            //The handler uses this object to validate the token and will throw an exception if any of the specified parameters is invalid.
                            var validationParameters = new TokenValidationParameters
                            {
                                ValidIssuer = "https://appleid.apple.com",
                                IssuerSigningKey = tPublicKey,
                                ValidAudience = NWDWebStandardConfiguration.KConfig.AppleServiceId
                            };

                            SecurityToken tValidatedToken;
                            tTokenHandler.ValidateToken(tTokenId, validationParameters, out tValidatedToken);

                            if (string.IsNullOrEmpty(tTrustedId) == false)
                            {
                                    NWDAccountSign tSign = NWDAccountSign.CreateApple(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                                    UseSocialSign(tSign);
                                return View(nameof(System.Index));
                            }
                            else
                            {
                                AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Token is null."));
                            }
                        }
                        catch
                        {
                            AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token invalid", "Token is invalid."));
                        }
                    }
                }
                catch
                {
                    AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Token is false."));
                }
            }
            catch
            {
                AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Code error", "Code is false."));
            }
            //AddTempDataObject(PageInformation);
            return RedirectToAction(nameof(Error));
        }

        public static string AppleKeyValueToString()
        {
            // contents of your .p8 file
            string rReturn = string.Join("\n", NWDWebStandardConfiguration.KConfig.AppleKeyValue).Replace("-----BEGIN PRIVATE KEY-----\n", "").Replace("\n-----END PRIVATE KEY-----", "");
            return rReturn;
        }

        public static string CreateSecretKey()
        {
            string iss = NWDWebStandardConfiguration.KConfig.AppleTeamId; // your account's team ID found in the dev portal
            string aud = "https://appleid.apple.com";
            string sub = NWDWebStandardConfiguration.KConfig.AppleServiceId; // same as client_id
            string privateKey = AppleKeyValueToString();
            var now = DateTime.UtcNow;
            var ecdsa = ECDsa.Create();
            ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            var handler = new JsonWebTokenHandler();
            string rReturn = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = iss,
                Audience = aud,
                Claims = new Dictionary<string, object> { { "sub", sub } },
                Expires = now.AddMinutes(5), // expiry can be a maximum of 6 months - generate one per request or re-use until expiration
                IssuedAt = now,
                NotBefore = now,
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256)
            });
            return rReturn;
        }


        public string GenerateAppleClientSecret()
        {
            string privateKey = AppleKeyValueToString(); //Put here the content of the .p8 file (without -----BEGIN PRIVATE KEY----- and -----END PRIVATE KEY-----).
            string keyId = NWDWebStandardConfiguration.KConfig.AppleKeyId; //The 10-character key identifier from the portal.
            string clientId = NWDWebStandardConfiguration.KConfig.AppleServiceId;
            string teamId = NWDWebStandardConfiguration.KConfig.AppleTeamId;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Import the key using a Pkcs8PrivateBlob.
            var cngKey = CngKey.Import(Convert.FromBase64String(privateKey), CngKeyBlobFormat.Pkcs8PrivateBlob);

            //Create new ECDsaCng object with the imported key.
            var ecDsaCng = new ECDsaCng(cngKey);
            ecDsaCng.HashAlgorithm = CngAlgorithm.ECDsaP256;

            //Create new SigningCredentials instance which will be used for signing the token.
            var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecDsaCng), SecurityAlgorithms.EcdsaSha256);

            var now = DateTime.UtcNow;

            //Create new list with the required claims.
            var claims = new List<Claim>
            {
                new Claim("iss", teamId),
                new Claim("iat", EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64),
                new Claim("exp", EpochTime.GetIntDate(now.AddMinutes(5)).ToString(), ClaimValueTypes.Integer64),
                new Claim("aud", "https://appleid.apple.com"),
                new Claim("sub", clientId)
            };
            var token = new JwtSecurityToken(
                issuer: teamId,
                claims: claims,
                expires: now.AddMinutes(5),
                signingCredentials: signingCredentials);

            token.Header.Add("kid", keyId);
            return tokenHandler.WriteToken(token);
        }
    }
}