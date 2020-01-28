using System.Web.Http;
using CaptchaGenerator.Lib;

namespace CaptchaGenerator.Web.Controllers
{
    public class CaptchaGeneratorController : ApiController
    {
        public CaptchaResult GetCaptcha()
        {
            var b = new CaptchaBuilder();
            return b.BuildCaptcha("TEST CAPTCHA");
        }

        public CaptchaResult GetCaptcha(string message)
        {
            var b = new CaptchaBuilder();
            return b.BuildCaptcha(message);
        }

        public CaptchaResult GetCaptcha(int numberOfCharacters)
        {
            var b = new CaptchaBuilder();
            return b.BuildCaptcha(numberOfCharacters);
        }
    }
}
