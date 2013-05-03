﻿using System;
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

        public AuthController(IEKProvider dal)
        {
            _service = new PageService(dal);
        }

        public ActionResult Login(int id, HomeIndexViewModel homeIndexViewModel)
        {
            var model = new LoginViewModel {};
            model.NavigationModel = homeIndexViewModel;
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
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.Id });
            }
        }

        public ActionResult ChangePassword(int id, HomeIndexViewModel navModel)
        {
            var model = new PasswordChangeViewModel { };
            model.NavigationModel = navModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeViewModel model)
        {
            if (!Membership.ValidateUser(Membership.GetUser().UserName, model.OldPassword))
                ModelState.AddModelError("OldPassword", "Incorrect Password");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = Membership.GetUser();
            user.ChangePassword(model.OldPassword, model.NewPassword);
            TempData["message"] = "Password has been changed";
            return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.Id});
        }

    }
}
