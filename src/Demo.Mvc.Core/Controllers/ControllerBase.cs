using Demo.Mvc.Core.Sites.Core;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Controllers
{
    public class ControllerBase : Controller
    {
        public ControllerBase(BusinessFactory business)
        {
            Business = business;
        }

        public BusinessFactory Business { get; }
    }
}