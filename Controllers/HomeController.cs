using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;

namespace EKContent.web.Controllers
{
    public class HomeController : Controller
    {
        private PageService _service;

        public HomeController(IEKProvider dal)
        {
            _service = new PageService(dal);
        }

        public ActionResult Index(int? id, HomeIndexViewModel model, bool userMode = false)
        {
            ViewBag.Service = _service;
            return View(model);
        }

        public ActionResult SendMessage(int? id, HomeIndexViewModel model, bool userMode = false)
        {
            ViewBag.Service = _service;
            TempData["message"] = "Your message has been sent";
            return RedirectToAction("Index", new {id = id});
        }

        public ActionResult Help(int? id, HomeIndexViewModel model, bool userMode = false)
        {
            ViewBag.Service = _service;
            return View(model);
        }

        private void Message(string msg)
        {
            TempData["message"] = msg;
        }

        [Authorize(Roles="Admin")]
        public ActionResult EditContent(int Idx, int pageId, int Mdx)
        {
            var page = _service.GetPage(pageId);
            var model = new EditContentItemViewModel
                            {
                                Mdx = Mdx,
                                Idx = Idx,
                                NavigationModel =  HomeIndexViewModelLoader.Create(pageId, _service)
                            };
            model.Content = model.Inserting() ? new Content { } : page.Modules[Mdx].Content[Idx];
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditContent(EditContentItemViewModel model)
        {
            if (String.IsNullOrEmpty(model.Content.Body))
                ModelState.AddModelError("Content.Body", "Required");
            if (ModelState.IsValid)
            {
                var content = model.Inserting() ? new Content { } : model.NavigationModel.Page.Modules[model.Mdx].Content[model.Idx];
                if (model.Inserting())
                    model.NavigationModel.Page.Modules[model.Mdx].Content.Add(content);

                content.Title = model.Content.Title;
                content.Body = model.Content.Body;
                content.ShowTitle = model.Content.ShowTitle;
                content.Priority = model.Content.Priority;
                _service.SavePage(model.NavigationModel.Page);
                Message("Content Saved");
                return RedirectToAction("Index", new { id = model.NavigationModel.Page.Id });
            }
            model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.Id, _service);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditPage(int? pageId, int? parentId)
        {
            var id = pageId.HasValue ? pageId.Value : parentId.Value;
            var page = _service.GetNavigation().Single(p => p.Id == id);
            var model = new EditPageViewModel
            {
                ParentId = parentId,
                PageId = pageId,
                NavigationModel = HomeIndexViewModelLoader.Create(id, _service)
            };
            model.Page = model.Inserting() ? new Page { ParentId = parentId } : page;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPage(EditPageViewModel model)
        {
            var pages = _service.GetNavigation();
            var page = model.Inserting() ? new Page { ParentId = model.ParentId } : pages.Single(p => p.Id == model.NavigationModel.Page.Id);
            page.Title = model.Page.Title;
            page.PageType = model.Page.PageType;
            page.Active = model.Page.Active;
            page.Description = model.Page.Description;
            page.Priority = model.Page.Priority;
            if (model.Inserting())
            {
                page.Id = pages.Max(p => p.Id) + 1;
                pages.Add(page);
            }
            _service.SaveNavigation(pages);
            Message("Page Saved");
            return RedirectToAction("Index", new { id = page.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePage(int id)
        {
            var pages = _service.GetNavigation();
            var page =  pages.Single(p => p.Id == id);
            if (pages.Any(p => p.ParentId == page.Id))
            {
                Message("Could not Delete Page");
                return RedirectToAction("Index", new { id = id });
            }

            pages.Remove(page);
            _service.SaveNavigation(pages);
            _service.DeletePage(id);
            Message("Page Deleted");
            return RedirectToAction("Index", new { id = page.ParentId});
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteContent(EditContentItemViewModel model)
        {
            var page = _service.GetPage(model.PageId);
            page.Modules[model.Mdx].Content.RemoveAt(model.Idx);
            _service.SavePage(page);
            Message("Item Deleted");
            return RedirectToAction("Index", new { id = model.PageId });
        }

        public ActionResult Slider(int? id, HomeIndexViewModel model)
        {
            ViewBag.Service = _service;
            return View(model);
        }

    }
}
