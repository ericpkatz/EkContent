using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;
using Newtonsoft.Json.Linq;

namespace EKContent.web.Models.Database.Abstract
{
    public interface ITwitterKeysProvider
    {
        TwitterKeys Get();
        void Save(TwitterKeys keys);
        JArray Tweets();
    }
}