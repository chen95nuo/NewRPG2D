using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class NetTest : TSingleton<NetTest>
{
    private const string defaultUrl = "http://39.104.79.77:8882/user/register?u={0}&p={1}";
    private const string loginUrl = "http://39.104.79.77:8882/user/login?u={0}&p={1}";

    public void Regist(string userName, string password)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(defaultUrl, userName, password)), onRequestFinished);
        request.Send();
    }

    public void Login(string userName, string password)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(loginUrl, userName, password)), onRequestFinished);
        request.Send();
    }

    private void onRequestFinished(HTTPRequest originalRequest, HTTPResponse response)
    {
        Debug.LogError(" HTTPResponse == " + response.DataAsText);
    }
}
