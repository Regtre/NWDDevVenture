using NWDFoundation.Configuration.Environments;

namespace NWDWebRuntime.Tools
{
    public static class NWDEnvironmentIcon
    {
        public static string IconCssNameFor(NWDEnvironmentKind sEnvironmentKind)
        {
            string rReturn = string.Empty;
            switch (sEnvironmentKind)
            {
                case NWDEnvironmentKind.Dev:
                {
                    rReturn = "far fa-keyboard";
                }
                    break;
                case NWDEnvironmentKind.PlayTest:
                {
                    rReturn = "bi bi-speedometer2";
                }
                    break;
                case NWDEnvironmentKind.Qualification:
                {
                    rReturn = "bi bi-funnel";
                }
                    break;
                case NWDEnvironmentKind.PreProduction:
                {
                    rReturn = "bi bi-eyeglasses";
                }
                    break;
                case NWDEnvironmentKind.Production:
                {
                    // rReturn = "bi bi-joystick";
                    rReturn = "bi bi-controller";
                }
                    break;
                case NWDEnvironmentKind.PostProduction:
                {
                    rReturn = "bi bi-gear";
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sEnvironmentKind), sEnvironmentKind, null);
            }

            return rReturn;
        }
    }
}