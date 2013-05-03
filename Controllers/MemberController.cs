using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.ViewModels;

namespace EKContent.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MemberController : BaseAdminController
    {
        public MemberController(IEKProvider provider): base(provider)
        {

        }

        public ActionResult Index(HomeIndexViewModel model, int id)
        {
            var memberIndexViewModel = new MemberIndexViewModel
                                           {
                                               NavigationModel = model,
                                               Roles = _service.GetRoles()
                                           };
            return View(memberIndexViewModel);
        }
    }
}