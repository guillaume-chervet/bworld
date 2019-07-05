using Demo.Business;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Controllers
{
    public class ManifestController : ControllerBase
    {
        public ManifestController(BusinessFactory business)
            : base(business)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}