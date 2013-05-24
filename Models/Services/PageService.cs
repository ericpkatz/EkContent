using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;
using EKContent.web.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace EKContent.web.Models.Services
{
    public class PageService
    {
        private INavigationProvider _navigationProvider = null;
        private IEkDataProvider _dataProvider = null;
        private IEkSiteDataProvider _siteProvider = null;
        private IImageDataProvider _imageProvider = null;
        private IEKRoleProvider _roleProvider = null;
        private IEKProvider _dal = null;

        public Site GetSite()
        {
            return _siteProvider.Get();
        }

        public void SetSite(Site site)
        {
            _siteProvider.Save(site);
        }

        //public PageService(INavigationProvider navigationProvider, IEkDataProvider dataProvider, IEkSiteDataProvider siteProvider, IImageDataProvider imageProvider)
        //{
        //    _navigationProvider = navigationProvider;
        //    _dataProvider = dataProvider;
        //    _siteProvider = siteProvider;
        //    _imageProvider = imageProvider;
        //}

        public IEKProvider Dal
        {
            get
            {
                return _dal;
            }
        }

        public PageService(IEKProvider provider)
        {
            _dal = provider;
            _navigationProvider = provider.NavigationProvider;
            _dataProvider = provider.DataProvider;
            _siteProvider = provider.SiteProvider;
            _imageProvider = provider.ImageProvider;
            _roleProvider = provider.RoleProvider;

        }

        public Page GetHomePage()
        {
            var page = _navigationProvider.GetNavigation().Where(p => p.IsHomePage()).Single();
            return GetPage(page.Id);
        }

        public List<PageNavigation> GetNavigation()
        {
            return _navigationProvider.GetNavigation().OrderBy(p => p.Priority).ToList();
        }

        public void SaveNavigation(List<PageNavigation> pages)
        {
            _navigationProvider.Save(pages);
        }

        public Page GetPage(int id)
        {
            var page = new Page {PageNavigation = _navigationProvider.GetNavigation().Where(p => p.Id == id).Single()};
            
            page.Modules = _dataProvider.Get(page.PageNavigation.Id);
            foreach(var module in page.Modules)
                foreach(var item in module.Content)
                    if(item.ImageId != 0)
                    {
                        var image = _imageProvider.Get().SingleOrDefault(i => i.Id == item.ImageId);
                        item.Image = image;
                    }
            return page;
        }

        public void DeletePage(int id)
        {
            _dataProvider.Delete(id);
        }

        public void SavePage(Page page)
        {
            foreach (Module m in page.Modules)
                m.Content = m.Content.OrderBy(c => c.Priority).ThenByDescending(c => c.DateCreated).ToList();
            _dataProvider.Save(page);
        }

        public Page MovePageToChildPage(int pageId)
        {
            var pages = GetNavigation();
            var page = new Page { PageNavigation = pages.Single(p => p.Id == pageId) };
            var modules = GetPage(pageId).Modules;
            foreach (Module m in modules)
                m.Content = m.Content.OrderBy(c => c.Priority).ThenByDescending(c => c.DateCreated).ToList();
            var newPage = page.Clone();
            newPage.Modules = modules;
            page.Modules = new List<Module>();
            //page.Title = "Change Title";

            newPage.PageNavigation.Id = pages.Max(p => p.Id) + 1;
            newPage.PageNavigation.Title = modules.SelectMany(m=>m.Content).First().Title;
            pages.Add(newPage.PageNavigation);
            _dataProvider.Save(newPage);
            _dataProvider.Save(page);
            SaveNavigation(pages);
            return newPage;
        }

        //images
        public List<Image> GetImages()
        {
            return _imageProvider.Get().OrderByDescending(i=>i.DateCreated).ToList();
        }

        public string GetImage(int id)
        {
            var img = _imageProvider.Get().SingleOrDefault(i => i.Id == id);
            if (img == null)
                return String.Format("[IMG]{0}", id);
            else
                return img.FileName;
        }

        public void SaveImage(Image image)
        {
            _imageProvider.Save(image);
        }

        public string RenderImages(string content)
        {
            if (content == null)
                return String.Empty;
            var regex = new Regex(@"\[IMG(?<image_number>[0-9]+)\]");
            return regex.Replace(content, m => String.Format("<img src=\"user_images/{0}\"/>", GetImage(int.Parse(m.Groups["image_number"].Value))));

        }
        public void DeleteImage(int id)
        {
            _imageProvider.Delete(id);
        }

        public List<EKRole> GetRoles()
        {
            return _roleProvider.Get().ToList();
        }


        public void SendMessage(Message message)
        {
            var site = this.GetSite();
            MailMessage msgeme = new MailMessage(message.Email, site.SmtpSiteEmail, String.Format("Message From {0}", message.Name), message.Body);
            SmtpClient smtpclient = new SmtpClient(site.SmtpServer);
            smtpclient.EnableSsl = false;
            smtpclient.Credentials = new NetworkCredential(site.SmtpSiteEmail, site.SmtpSiteEmailPassword);
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpclient.Send(msgeme);

        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            var user = Membership.GetUser();
            user.ChangePassword(oldPassword, newPassword);
        }

        public void SaveTwitterKeys(TwitterKeys keys)
        {
            _dal.TwitterKeysProvider.Save(keys);
        }

        public TwitterKeys GetTwitterKeys()
        {
            return _dal.TwitterKeysProvider.Get();
        }

        public JArray GetTweets()
        {
            return _dal.TwitterKeysProvider.Tweets();
        }

        public bool ShowTwitterFeed()
        {
            return _dal.TwitterKeysProvider.Get().Configured && _dal.TwitterKeysProvider.Get().Enabled;
        }

        public string Encrypt(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(input);
            data = x.ComputeHash(data);
            String md5Hash = System.Text.Encoding.ASCII.GetString(data);
            return md5Hash;
        }

    }
}