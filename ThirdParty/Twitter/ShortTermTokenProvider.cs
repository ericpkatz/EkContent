using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth.ChannelElements;
using EKContent.web.Infrastructure;
using EKContent.web.Models.Entities;
using EKContent.web.Models.Services;

namespace EKContent.web.ThirdParty.Twitter
{
    public class ShortTermTokenProvider : TwitterBaseClass, IConsumerTokenManager
    {

        public const string key = "STORE_KEY";

        public  string ConsumerKey
        {
            get { return base.ConsumerKey; }
        }

        public  string ConsumerSecret
        {
            get { return base.ConsumerSecret; }
        }

        public Dictionary<string, string> Store
        {
            get
            {
                var ctx = System.Web.HttpContext.Current;
                var store = ctx.Session[key] as Dictionary<string, string>;

                if (store == null)
                    ctx.Session[key] = new Dictionary<string, string>();
                return ctx.Session[key] as Dictionary<string, string>;
            }
        }



        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            Store.Remove(requestToken);
            var keys = Keys();
            keys.ApplicationAuthorizationKey = accessToken;
            keys.ApplicationAuthorizationSecret = accessTokenSecret;
            keys.Configured = true;
            Service().SaveTwitterKeys(keys);
            //Store[accessToken] = accessTokenSecret;
        }

        public string GetTokenSecret(string token)
        {
            return Store[token];
        }

        public TokenType GetTokenType(string token)
        {
            throw new NotImplementedException();
        }

        public void StoreNewRequestToken(DotNetOpenAuth.OAuth.Messages.UnauthorizedTokenRequest request, DotNetOpenAuth.OAuth.Messages.ITokenSecretContainingMessage response)
        {
            Store[response.Token] = response.TokenSecret;
        }

    }
}