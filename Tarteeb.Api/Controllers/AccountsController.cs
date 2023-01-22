using Microsoft.AspNetCore.Mvc;

namespace Tarteeb.Api.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
