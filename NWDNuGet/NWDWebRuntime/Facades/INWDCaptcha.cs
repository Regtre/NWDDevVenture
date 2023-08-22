using System.ComponentModel.DataAnnotations;

namespace NWDWebRuntime.Facades
{
    public interface INWDCaptcha
    {
        [Required()]
        [Display(Name = "Captcha")]
        public string CaptchaValue { set; get; }
    }
}