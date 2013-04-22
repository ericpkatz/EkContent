﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Services;

namespace EKContent.web.Models.ViewModels
{
    public class HomeIndexViewModelLoader
    {
        public static HomeIndexViewModel Create(int id, PageService service)
        {
            var page = service.GetPage(id);

            return new HomeIndexViewModel
                       {
                           Page = page,
                           Pages = service.GetNavigation()
                       };
        }
    }
}