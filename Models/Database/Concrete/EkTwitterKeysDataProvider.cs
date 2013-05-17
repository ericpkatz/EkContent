using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;
using EKContent.web.ThirdParty.Twitter;
using Newtonsoft.Json.Linq;

namespace EKContent.web.Models.Database.Concrete
{
    public class EkTwitterKeysDataProvider : ITwitterKeysProvider
    {
        string file = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/twitterKeys.js"));


        public TwitterKeys Get()
        {
            TwitterKeys keys = null;
            if (!File.Exists(file))
            {
                keys = new TwitterKeys
                               {
 
                               };
                Save(keys);
                return keys;
            }
            keys  = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                keys = serializer.Deserialize<TwitterKeys>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return keys;
        }


        public void Save(TwitterKeys site)
        {

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(site);
                sw.Write(str);
                sw.Flush();
            }
            finally
            {
                sw.Close();
                sw.Dispose();
                sw = null;
            }          
        }


        private ServiceProviderDescription ServiceProviderDescription()
        {

            return new ServiceProviderDescription

            {
                AccessTokenEndpoint = new MessageReceivingEndpoint("     https://api.twitter.com/oauth/access_token", HttpDeliveryMethods.PostRequest),
                RequestTokenEndpoint = new MessageReceivingEndpoint("     https://api.twitter.com/oauth/request_token", HttpDeliveryMethods.PostRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://api.twitter.com/oauth/authorize", HttpDeliveryMethods.PostRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                ProtocolVersion = ProtocolVersion.V10a
            };

        }

        public Newtonsoft.Json.Linq.JArray Tweets()
        {
            var cache = System.Web.HttpContext.Current.Cache;
            JArray tweets = (JArray) cache["Tweets"];

            if (tweets == null)
            {
            
            var token = Get().ApplicationAuthorizationKey;

            var consumer = new DotNetOpenAuth.OAuth.WebConsumer(ServiceProviderDescription(),
                                                                new LongTermTokenProvider());

            var endpoint = new MessageReceivingEndpoint("https://api.twitter.com/1.1/statuses/home_timeline.json",
                                                        HttpDeliveryMethods.GetRequest);

            var request = consumer.PrepareAuthorizedRequest(endpoint, token);

            var response = request.GetResponse();
            var data = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            tweets = JArray.Parse(data);
                cache.Insert("Tweets", tweets, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero);
            }

            return tweets;
        }
    }
}