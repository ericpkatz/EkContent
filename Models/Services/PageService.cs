﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Services
{
    public class PageService
    {
        private INavigationProvider _navigationProvider = null;
        private IEkDataProvider _dataProvider = null;


        public PageService(INavigationProvider navigationProvider, IEkDataProvider dataProvider)
        {
            _navigationProvider = navigationProvider;
            _dataProvider = dataProvider;
        }

        public Page GetHomePage()
        {
            var page = _navigationProvider.GetNavigation().Where(p => p.IsHomePage()).Single();
            return GetPage(page.Id);
        }

        public List<Page> GetNavigation()
        {
            return _navigationProvider.GetNavigation();
        }

        public void SaveNavigation(List<Page> pages)
        {
            _navigationProvider.Save(pages);
        }

        public Page GetPage(int id)
        {
            var page = _navigationProvider.GetNavigation().Where(p => p.Id == id).Single();
            page.Modules = _dataProvider.Get(page.Id);
            return page;
        }

        public void DeletePage(int id)
        {
            _dataProvider.Delete(id);
        }

        public void SavePage(Page page)
        {
            _dataProvider.Save(page);
        }

    }
}