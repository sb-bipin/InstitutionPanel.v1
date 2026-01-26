using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionPanel.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ActionResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View("AccessDenied");
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
