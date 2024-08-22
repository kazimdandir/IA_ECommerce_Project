using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    public class ECommerceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
