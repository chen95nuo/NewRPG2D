using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using UnityEngine.SceneManagement;
using System.Text;
using System.Security.Cryptography;
using Assets.Script.Net;
using BestHTTP;
using System;

public class UILogin : TTUIPage
{
    private const string registerUrl = "http://39.104.79.77:8882/user/register?u={0}&p={1}";
    private const string loginUrl = "http://39.104.79.77:8882/user/login?u={0}&p={1}";
    private const string onkeyregisterUrl = "http://39.104.79.77:8882/user/onkeyregister?sid={0}";

    public InputField UserName;
    public InputField PassWord;
    public Button btn_Login;
    public Button btn_RegisterPage;
    public Button btn_RegisterBack;
    public Button btn_Register;
    public Button btn_OnKeyRegister;
    public GameObject StartPage;

    private int index;
    private bool isRun = false;

    public GameObject ErrorPopup;
    public Text txt_Error;
    public Button btn_Error;

    public GameObject RegisterPopup;
    public InputField registerUserName;
    public InputField registerPassWord;
    private LoginType type;

    private bool isRegister = false; //是注册？

    private string userName;
    private string passWord;


    public InputField Ip;
    public InputField Port;
    public Button Link;

    private void Awake()
    {
        btn_Login.onClick.AddListener(ChickLogin);
        btn_RegisterPage.onClick.AddListener(ChickRegisterPage);
        btn_Register.onClick.AddListener(ChickRegister);
        btn_Error.onClick.AddListener(ChickError);
        btn_OnKeyRegister.onClick.AddListener(ChickOnkeyRegister);
        btn_RegisterBack.onClick.AddListener(ChickRegisterBack);
        StartPage.gameObject.SetActive(false);

        Link.onClick.AddListener(TestLink);

        //if (PlayerPrefs.HasKey("UserName"))
        //{
        //    string userName = PlayerPrefs.GetString("UserName");
        //    string passWord = PlayerPrefs.GetString("PassWord");
        //    Request(loginUrl, userName, passWord);
        //}
        //else
        //{
        //    StartPage.gameObject.SetActive(false);
        //}
        //PlayerPrefs.SetString("UserName", "Name");
        //PlayerPrefs.SetString("PassWord", "Name");
    }

    private void TestLink()
    {
        //string ip = Ip.text;
        //if (Ip.text.Equals(""))
        //{
        //    ip = "127.0.0.1";
        //}
        //int port = int.Parse(Port.text);
        //if (Port.text.Equals(""))
        //{
        //    port = 9010;
        //}

        Assets.Script.GameLogic.Instance.InitData("192.168.1.102", 9010, "1");
    }

    //private void Start()
    //{
    //    Assets.Script.GameLogic.Instance.InitData("192.168.1.102", 9010, "123");
    //}

    private void ChickRegisterBack()
    {
        RegisterPopup.SetActive(false);
    }

    private void ChickOnkeyRegister()
    {
        OnkeyRegister();
    }

    private void ChickError()
    {
        ErrorPopup.SetActive(false);
    }


    private void ChickLogin()
    {
        string userName = UserName.text;
        string passWord = PassWord.text;
        //SceneManager.LoadScene("EasonMainScene");
        Request(loginUrl, userName, passWord);
    }

    private void ErrorTip(int ErrorID)
    {
        string st = "";
        switch (ErrorID)
        {
            case -1:
                st = "通用错误";
                break;
            case 101:
                st = "账号已存在";
                break;
            case 102:
                st = "账户不存在";
                break;
            case 103:
                st = "密码错误";
                break;
            default:
                break;
        }
        ErrorPopup.SetActive(true);
        txt_Error.text = st;
    }

    private void ChickRegisterPage()
    {
        RegisterPopup.SetActive(!isRegister);
    }

    private void ChickRegister()
    {
        isRegister = true;
        string userName = registerUserName.text;
        string passWord = registerPassWord.text;
        Debug.Log(string.Format("账号:{0},密码:{1}", userName, passWord));
        Request(registerUrl, userName, passWord);
    }

    private void RegisterComplete(int ret)
    {
        if (ret != 0)
        {
            ErrorTip(ret);
            return;
        }
        isRegister = !isRegister;
        RegisterPopup.SetActive(isRegister);

        UserName.text = registerUserName.text;
        PassWord.text = registerPassWord.text;
        PlayerPrefs.SetString("UserName", registerUserName.text);
        PlayerPrefs.SetString("PassWord", registerPassWord.text);
        PlayerPrefs.Save();
        Debug.Log("注册成功,保存账号密码 :" + registerUserName + registerPassWord);
    }

    private void OnRequestFinished(HTTPRequest originalRequest, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        LoginRequest RS = JsonUtility.FromJson<LoginRequest>(response.DataAsText);
        if (RS.ret != 0)
        {
            ErrorTip(RS.ret);
            return;
        }
        if (isRegister)
        {
            RegisterComplete(RS.ret);
            return;
        }
        if (RS.u != null)
        {
            Debug.Log("快速注册成功");
            PlayerPrefs.SetString("UserName", RS.u);
            PlayerPrefs.SetString("PassWord", RS.p);
            Debug.Log(string.Format("账号:{0}密码{1}", RS.u, RS.p));
            Request(loginUrl, RS.u, RS.p);
            return;
        }
        PlayerPrefs.SetString("UserName", userName);
        PlayerPrefs.SetString("PassWord", passWord);
        PlayerPrefs.Save();
        Debug.Log("登陆成功=====" + RS.server_ip + " port:" + RS.port + " token:" + RS.token);
        Assets.Script.GameLogic.Instance.InitData(RS.server_ip, RS.port, RS.token);
    }



    public void Request(string Url, string userName, string password)
    {
        this.userName = userName;
        this.passWord = password;
        password = UserMd5(password);
        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(Url, userName, password)), OnRequestFinished);
        request.Send();
    }

    public void OnkeyRegister()
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(onkeyregisterUrl, SystemInfo.deviceUniqueIdentifier)), OnRequestFinished);
        request.Send();
    }

    static string UserMd5(string str)
    {
        string cl = str;
        StringBuilder pwd = new StringBuilder();
        MD5 md5 = MD5.Create();//实例化一个md5对像
                               // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            pwd.Append(s[i].ToString("x2"));
            //pwd = pwd + s[i].ToString("X");
        }
        return pwd.ToString();
    }
}

[System.Serializable]
public class LoginRequest
{
    [SerializeField]
    public int port;
    [SerializeField]
    public int ret;
    [SerializeField]
    public string token;
    [SerializeField]
    public string server_ip;
    [SerializeField]
    public string u;
    [SerializeField]
    public string p;
}

public enum LoginType
{

}