using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public abstract class NWDSessionDefinition
    {
        public NWDSessionDefinitionKind Kind = NWDSessionDefinitionKind.StringKind;
        public string Name = string.Empty;
        public string Title = string.Empty;
        public string Explication = string.Empty;
        public NWDSessionDefinitionGroup Group = NWDSessionDefinitionGroup.Navigation;
        public string DefaultValue = string.Empty;
        public bool Deletable = true;
        public bool ManualEditable = false;

        protected string? _GetValue(HttpContext? sHttpContext)
        {
            string? rReturn = null;
            if (sHttpContext != null)
            {
                if (sHttpContext.Session.GetString(Name) != null)
                {
                    rReturn = sHttpContext.Session.GetString(Name);
                }
                else
                {
                    rReturn = DefaultValue;
                }
            }

            return rReturn;
        }

        protected void _SetValue(HttpContext? sHttpContext, string sValue)
        {
            if (sHttpContext != null)
            {
                sHttpContext.Session.SetString(Name, sValue);
            }
        }

        public bool Exists(HttpContext? sHttpContext)
        {
            bool rReturn = false;
            if (sHttpContext != null)
            {
                if (sHttpContext.Session.GetString(Name) != null)
                {
                    rReturn = true;
                }
            }

            return rReturn;
        }

        public string? GetValueAsString(HttpContext? sHttpContext, bool sForHtml = false)
        {
            if (sHttpContext != null)
            {
                if (sForHtml)
                {
                    string? tR = _GetValue(sHttpContext);
                    if (string.IsNullOrEmpty(tR) == false)
                    {
                        return "<span>"+Regex.Replace(tR, ".{8}", "$0</span>&hairsp;<span>") + "</span>";
                        //return Regex.Replace(tR, ".{8}", "$0 ");
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return _GetValue(sHttpContext);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public void DeleteFrom(HttpContext? sHttpContext)
        {
            if (sHttpContext != null)
            {
                sHttpContext.Session.Remove(Name);
            }
        }
    }
}