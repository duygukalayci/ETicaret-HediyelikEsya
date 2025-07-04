using Microsoft.AspNetCore.Mvc;

namespace GiftShop.WebUI.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return Content("Test controller Index action çalışıyor!");
        }

        public IActionResult Edit()
        {
            return Content("Test controller Edit action çalışıyor!");
        }
    }
}

