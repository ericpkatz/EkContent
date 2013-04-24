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

        public HomeController(INavigationProvider navigationProvider, IEkDataProvider dataProvider, IEkSiteDataProvider siteProvider)
        {
            _service = new PageService(navigationProvider, dataProvider, siteProvider);
        }

        public ActionResult Index(int? id)
        {
            var model = HomeIndexViewModelLoader.Create(id, _service);

            model.Page.Modules = _service.GetPage(model.Page.Id).Modules;
            //model.Page.Modules = _service.GetModules(model.Page.Id);

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
            var page = _service.GetPage(model.NavigationModel.Page.Id);
            var content = model.Inserting() ? new Content { } : page.Modules[model.Mdx].Content[model.Idx];
            if (model.Inserting())
                page.Modules[model.Mdx].Content.Add(content);

            content.Title = model.Content.Title;
            content.Body = model.Content.Body;
            _service.SavePage(page);
            Message("Content Saved");
            return RedirectToAction("Index", new { id = page.Id });
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
            if (!pages.Any(p => p.ParentId == page.Id))
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

    }
}
