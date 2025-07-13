using GiftShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class MainController : Controller

    {
        private readonly DatabaseContext _databaseContext;

        public MainController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IActionResult Index()
        {
            ViewBag.Products = _databaseContext.Products;
            return View();
        }
    }
}
