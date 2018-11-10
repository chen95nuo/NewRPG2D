using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using UnityEngine.SceneManagement;
using BestHTTP;
using System;

public class UILogin : MonoBehaviour
{
    private const string registerUrl = "http://39.104.79.77:8882/user/register?u={0}&p={1}";
    private const string loginUrl = "http://39.104.79.77:8882/user/login?u={0}&p={1}";

    public InputField UserName;
    public InputField PassWord;
    public Button btn_Login;
    public Button btn_RegisterPage;
    public Button btn_Register;
    public GameObject Load;
    public Slider slider;
    public Text txt_LoadNum;

    private int index;
    private bool isRun = false;

    public GameObject ErrorPopup;
    public Text txt_Error;
    public Button btn_Error;

    public GameObject RegisterPopup;
    public InputField registerUserName;
    public InputField registerPassWodk;
    private LoginType type;

    private void Awake()
    {
        slider.value = 0;
        btn_Login.onClick.AddListener(ChickLogin);
        btn_RegisterPage.onClick.AddListener(ChickRegisterPage);
        btn_Register.onClick.AddListener(ChickRegister);
        btn_Error.onClick.AddListener(ChickError);
    }


    private void ChickError()
    {
        ErrorPopup.SetActive(false);
    }


    private void ChickLogin()
    {
        string userName = UserName.text;
        string passWord = PassWord.text;
        if (userName == "" || passWord == "")
        {
            ErrorTip("错误：请输入正确的账号密码");
        }
        Debug.Log(string.Format("账号:{0},密码:{1}", userName, passWord));
        //SceneManager.LoadScene("EasonMainScene");
        Login(userName, passWord);
    }

    private void ErrorTip(string st)
    {
        ErrorPopup.SetActive(true);
        txt_Error.text = st;
    }

    private void ChickRegisterPage()
    {
        RegisterPopup.SetActive(true);
    }

    private void ChickRegister()
    {
        RegisterPopup.SetActive(false);
        string userName = registerUserName.text;
        string passWord = registerPassWodk.text;
        Debug.Log(string.Format("账号:{0},密码:{1}", userName, passWord));
        Register(userName, passWord);
    }

    private void onRequestFinished(HTTPRequest originalRequest, HTTPResponse response)
    {
        LoginRequest request = JsonUtility.FromJson<LoginRequest>(response.DataAsText);
        Debug.LogError(response.DataAsText);

        Debug.LogError("返回 编号:" + request.ret);
        Debug.LogError("返回 编码:" + request.token);

        switch (type)
        {
            default:
                break;
        }
    }
    public void Register(string userName, string password)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(registerUrl, userName, password)), onRequestFinished);
        request.Send();
    }

    public void Login(string userName, string password)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(loginUrl, userName, password)), onRequestFinished);
        request.Send();
    }
}

[System.Serializable]
public class LoginRequest
{
    [SerializeField]
    public int ret;
    [SerializeField]
    public string token;
}

public enum LoginType
{

}