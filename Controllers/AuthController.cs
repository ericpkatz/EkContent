using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;

namespace EKContent.web.Controllers
{
    public class AuthController : Controller
    {
        private PageService _service;

        public AuthController(INavigationProvider navigationProvider, IEkDataProvider dataProvider)
        {
            _service = new PageService(navigationProvider, dataProvider);
        }

        public ActionResult Login(int id)
        {
            var model = new LoginViewModel {};
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        public ActionResult Logout(int id)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { id = id });
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!Membership.ValidateUser(model.Username, model.Password))
            {
                ModelState.AddModelError("", "Username and/or Password are incorrect");
                model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.Id, _service);
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.Id });
            }
        }

    }
}
