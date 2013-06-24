using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;
using EKContent.web.ThirdParty.Twitter;

namespace EKContent.web.Controllers
{
    public class HomeController : BaseController
    {
        //private PageService _service;

        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    ViewBag.Service = _service;
        //}

        public HomeController(IEKProvider dal) : base(dal)
        {
            //_service = new PageService(dal);
        }

        public ActionResult Index(int? id, HomeIndexViewModel model, bool userMode = false)
        {
            ViewBag.Service = _service;
            return View(model);
        }

        public ActionResult SendMessage(int? id, string pageTitle, HomeIndexViewModel model, Message message, bool userMode = false)
        {
            ViewBag.Service = _service;
            _service.SendMessage(message);
            if(message.IsReview)
                TempData["message"] = "Thanks for your review!!";
            else
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
                                NavigationModel =  HomeIndexViewModelLoader.Create(pageId, _service),
                                Images = _service.GetImages(),
                                Files = _service.Dal.FileProvider.Get()
                            };
            model.Content = model.Inserting() ? new Content { } : page.Modules[Mdx].Content[Idx];
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditContent(EditContentItemViewModel model)
        {
            //if (String.IsNullOrEmpty(model.Content.Body))
            //    ModelState.AddModelError("Content.Body", "Required");
            if (ModelState.IsValid)
            {
                var content = model.Inserting() ? new Content { } : model.NavigationModel.Page.Modules[model.Mdx].Content[model.Idx];
                if (model.Inserting())
                {
                    if (model.NavigationModel.Page.Modules.Count == 0)
                        model.NavigationModel.Page.Modules.Add(new Module {});
                    model.NavigationModel.Page.Modules[model.Mdx].Content.Add(content);
                }

                content.Title = model.Content.Title;
                content.Body = model.Content.Body;
                content.ShowTitle = model.Content.ShowTitle;
                content.Priority = model.Content.Priority;
                content.ImageId = model.Content.ImageId;
                content.FileId = model.Content.FileId;
                content.DatePublished = model.Content.DatePublished;
                content.ShowAddThis = model.Content.ShowAddThis;
                content.BorderImage = model.Content.BorderImage;
                _service.SavePage(model.NavigationModel.Page);
                Message("Content Saved");
                return RedirectToAction("Index", new { id = model.NavigationModel.Page.PageNavigation.Id });
            }
            model.Images = _service.GetImages();
            model.Files = _service.Dal.FileProvider.Get();
            model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.PageNavigation.Id, _service);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditPage(int? pageId, int? parentId)
        {
            var id = pageId.HasValue ? pageId.Value : parentId.Value;
            var page = _service.GetPage(id);
            var model = new EditPageViewModel
            {
                ParentId = parentId,
                PageId = pageId,
                NavigationModel = HomeIndexViewModelLoader.Create(id, _service)
            };
            model.Page = model.Inserting() ? new Page {PageNavigation =  new PageNavigation{ParentId = parentId} } : page;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPage(EditPageViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);
            var pages = _service.GetNavigation();
            var page = model.Inserting() ? new PageNavigation { ParentId = model.ParentId } : pages.Single(p => p.Id == model.NavigationModel.Page.PageNavigation.Id);
            model.Page.PageNavigation.TransferTo(page);
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
        public ActionResult MovePage(EditPageViewModel model)
        {
            //var pageToMove = _service.GetPage(model.NavigationModel.Page.PageNavigation.Id);
            var newPage = _service.MovePageToChildPage(model.NavigationModel.Page.PageNavigation.Id);
            Message("A new page has been added.");
            return RedirectToAction("Index", new { id = newPage.PageNavigation.Id });
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

        public ActionResult FollowUs(int id)
        {
            var consumer = _service.TwitterConsumer();
            var uri = new Uri(this.Request.Url.AbsoluteUri.Replace("FollowUs", "EndFollowUs"));

            consumer.Channel.Send(consumer.PrepareRequestUserAuthorization(uri, null, null));
            return View();

        }

        public FileResult DownloadFile(int id)
        {
            var file = _service.Dal.FileProvider.Get().SingleOrDefault(f => f.Id == id);
            if(file != null)
            {
                return File(file.AbsolutePath(), System.Net.Mime.MediaTypeNames.Application.Octet, file.NameWithExtension());
            }
            return null;
        }

        public ActionResult EndFollowUs(int id)
        {
            var consumer = _service.TwitterConsumer();
            var authorizationTokenResponse = consumer.ProcessUserAuthorization();
            //var keys = _service.Dal.TwitterKeysProvider.Get();
            //keys.ApplicationAuthorizationKey = authorizationTokenResponse.AccessToken;
            //keys.ApplicationAuthorizationSecret = new ShortTermTokenProvider().GetTokenSecret(authorizationTokenResponse.AccessToken);
            //keys.Configured = true;
            //_service.Dal.TwitterKeysProvider.Save(keys);
            TempData["message"] = "Thanks for following us.";
            return RedirectToAction("Index", new { id = id });
            // Response.Write(authorizationTokenResponse.AccessToken);
            // if (authorizationTokenResponse == null)

            //   return RedirectToAction("Index", "Home");


        }


    }
}
