

using BestHTTP;
using System;
using UnityEngine;

namespace Assets.Script.Net
{
    public class HTTPManager : TSingleton<HTTPManager>
    {
        private const string defaultUrl = "http://39.104.79.77:8882/user/regist?u={0}&p={1}";

        public override void Init()
        {
            base.Init();
        }

        public void Regist(string userName, string password)
        {
            HTTPRequest request = new HTTPRequest(new Uri(string.Format(defaultUrl, userName, password)), onRequestFinished);
            request.Send();
        }

        private void onRequestFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.LogError(" HTTPResponse ==  " + response.DataAsText);
        }
    }
}
