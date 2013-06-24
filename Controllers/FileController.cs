using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;
using System.IO;
using EKContent.web.Models.Entities;

namespace EKContent.web.Controllers
{
    [Authorize(Roles="Admin")]
    public class FileController : BaseController
    {


        public FileController(IEKProvider provider) : base(provider)
        {
        }

        public ActionResult List(int id, HomeIndexViewModel homeIndexViewModel)
        {
            var model = new FileListViewModel{Files = _service.Dal.FileProvider.Get()};
            model.NavigationModel = homeIndexViewModel;
            return View(model);
        }

        public ActionResult Create(int pageId, int? id)
        {
            var file = id.HasValue ? _service.Dal.FileProvider.Get().Single(i=>i.Id == id.Value) : new EKFile{};
            var model = new FileCreateViewModel { File = file};
            model.NavigationModel = HomeIndexViewModelLoader.Create(pageId, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FileCreateViewModel model)
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var fileName = this.Request.Files[0].FileName;
                var extension = Path.GetExtension(fileName);
                model.File.FileName = String.Format("{0}{1}", Guid.NewGuid(), extension);

                //delete the old file
                if(!model.File.IsNew())
                {
                    var oldFile = _service.Dal.FileProvider.Get().Single(i => i.Id == model.File.Id).FileName;
                    var oldFilePath = this.Server.MapPath(String.Format("~/user_files/{0}", oldFile));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);

                }
                var saveTo = this.Server.MapPath(String.Format("~/user_files/{0}", model.File.NameWithExtension()));
                this.Request.Files[0].SaveAs(saveTo);
            }

            _service.Dal.FileProvider.Save(model.File);

            TempData["message"] = "File Saved";
            return RedirectToAction("List", new {id = model.NavigationModel.Page.PageNavigation.Id });
        }

        [HttpPost]
        public ActionResult Delete(int pageId, int id)
        {
            var file = _service.Dal.FileProvider.Get().Single(i => i.Id == id);
            _service.Dal.FileProvider.Delete(file.Id);
            var filePath = this.Server.MapPath(String.Format("~/user_files/{0}", file.FileName));

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            TempData["message"] = "File Removed";
            return RedirectToAction("List", new { id = pageId });
        }



    }
}
