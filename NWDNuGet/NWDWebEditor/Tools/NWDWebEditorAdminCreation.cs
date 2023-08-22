// using NWDEditor.Exchanges;
// using NWDWebEditor.Configuration;
// using NWDWebEditor.Managers;
//
// namespace NWDWebEditor.Tools;
//
// [Obsolete("WILL BE REMOVED AS SOON AS POSSIBLE")]
// public static class NWDWebEditorAdminCreation
// {
//     public static void CreateAdminAccount()
//     {
//         NWDLogger.WriteLine(nameof(CreateAdminAccount) + "() with " + NWDWebEditorConfiguration.KConfig.AdminEmail + " and give it " + NWDWebEditorConfiguration.KConfig.AdminService +
//                           " for long long time ... ");
//         //TODO DO NOT TO DO   ... USE https://editor.net-worked-data.com to edit WebServices
//         // Contact NWDServer to create an account and add services
//
//         NWDResponseEditor? rReturn = NWDWebEditorCallbackServers.PostRequest(NWDRequestEditor.CreateRequestTest()).Result;
//     }
//     
//     public static void CreateAdminDefault()
//     {
//         string tAccountReference = "";
//         if (string.IsNullOrEmpty( NWDWebEditorConfiguration.KConfig.AdminEmail) == false)
//         {
//             /*AccountSign tAccountSign = new AccountSign();
//             if (tAccountSign.NonExistsThenPrepare(NWDAccountSignType.EmailPassword, AdminEmail, string.Empty, string.Empty, string.Empty, string.Empty) == true)
//             {
//                 // Create an account
//                 NWDAccountSignUp tNewAdmin = new NWDAccountSignUp();
//                 tNewAdmin.AccountSignUp_Email = AdminEmail;
//                 tNewAdmin.AccountSignUp_Password = NWDRandom.RandomStringNoMistake(16);
//                 tNewAdmin.AccountSignUp_PasswordConfirm = tNewAdmin.AccountSignUp_Password;
//                 tNewAdmin.AccountSignUp_Checked = true;
//                 tAccountReference = AccountSign.Reccord(tNewAdmin, null, true);
//                 NWDLogger.WriteLine("Create account " + tAccountReference + " for "+ AdminEmail);
//             }
//             else
//             {
//                 tAccountReference = AccountSign.GetAccountByEmail(AdminEmail);
//             }*/
//
//             if (string.IsNullOrEmpty(tAccountReference) == false)
//             {
//                 /*List<AccountService> tAccountServiceList = AccountService.GetServiceByAccount(tAccountReference);
//                 bool tAdminFound = false;
//                 foreach (AccountService tService in tAccountServiceList)
//                 {
//                     if (tService.ServiceName == AccountServiceType.Admin)
//                     {
//                         tAdminFound = true;
//                         tService.End = ToolBox.ToTimestampUnix(DateTime.Now.AddYears(10));
//                         tService.Update();
//                     }
//                 }
//                 if (tAdminFound == false)
//                 {
//                     AccountService tServiceAdmin = new AccountService();
//                     tServiceAdmin.Account = tAccountReference;
//                     tServiceAdmin.ServiceName = AccountServiceType.Admin;
//                     tServiceAdmin.End = ToolBox.ToTimestampUnix(DateTime.Now.AddYears(10));
//                     tServiceAdmin.Reccord();
//                     NWDLogger.WriteLine("Create a service " + tServiceAdmin.Reference + " for " + AdminEmail);
//                 }*/
//             }
//         }
//     }
// }