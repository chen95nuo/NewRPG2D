

using BestHTTP;
using System;
using UnityEngine;

namespace Assets.Script.Net
{
    public enum RequestTypeEnum
    {
        Get,
        Post,
    }

    public class HTTPManager : TSingleton<HTTPManager>
    {
        private const string defaultUrl = "http://39.104.79.77:8882/user/regist";
        private const string getUrl = "?u={0}&p={1}";

        public override void Init()
        {
            base.Init();
        }

        public void Register(string userName, string password, RequestTypeEnum requestType = RequestTypeEnum.Get)
        {
            if (requestType == RequestTypeEnum.Get)
            {
                HTTPRequest request = new HTTPRequest(new Uri(string.Format(defaultUrl + getUrl, userName, password)), onRequestFinished);
                request.Send();
            }
            else if (requestType == RequestTypeEnum.Post)
            {
                HTTPRequest request = new HTTPRequest(new Uri(defaultUrl + "/posturi"), HTTPMethods.Post, onRequestFinished);
                request.AddField("", userName);
                request.AddField("", password);
                request.Send();
            }
        }

        private void onRequestFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.LogError(" HTTPResponse ==  " + response.DataAsText);
        }
    }
}
