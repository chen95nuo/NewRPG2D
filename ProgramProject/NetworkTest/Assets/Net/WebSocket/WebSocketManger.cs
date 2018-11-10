
using UnityEngine;
using BestHTTP.WebSocket;
using System;
using UnityEngine.UI;
using System.Text;
using ProtoBuf;
using System.IO;
using demo;


public class ProtobufSerilizer
{

    /// <summary>
    /// 将消息序列化为二进制的方法
    /// </summary>
    /// <param name="model">要序列化的对象</param>
    public static byte[] Serialize(IExtensible model)
    {
        try
        {
            //创建流对象
            MemoryStream ms = new MemoryStream();
            //使用ProtoBuf自带的序列化工具序列化IExtensible对象
            Serializer.Serialize(ms, model);
            //创建二级制数组，保存序列化后的流
            byte[] bytes = new byte[ms.Length];
            //将流的位置设为0
            ms.Position = 0;
            //将流中的内容读取到二进制数组中
            ms.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        catch (Exception e)
        {
            Debug.Log("序列化失败: " + e.ToString());
            return null;
        }
    }

    /// <summary>
    /// 将收到的消息反序列化成IExtensible对象
    /// </summary>
    /// <param name="msg">收到的消息的字节流.</param>
    /// <returns></returns>
    public static T DeSerialize<T>(byte[] bytes) where T : IExtensible
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            //将消息写入流中
            ms.Write(bytes, 0, bytes.Length);
            //将流的位置归0
            ms.Position = 0;
            //反序列化对象
            T result = Serializer.Deserialize<T>(ms);
            return result;
        }
        catch (Exception e)
        {
            Debug.Log("反序列化失败: " + e.ToString());
            return default(T);
        }
    }
}

public class WebSocketManger : MonoBehaviour
{
    public string url = "ws://39.104.79.77:9010/websocket";
    public InputField msg;
    public Text console;

    private WebSocket webSocket;

    private void Start()
    {
        init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Connect();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Send("a");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
        if (Input.GetKeyDown(KeyCode.F))
        {

        }
    }

    private void init()
    {
        webSocket = new WebSocket(new Uri(url));
        webSocket.OnOpen += OnOpen;
        webSocket.OnBinary += OnMessageReceived;
        webSocket.OnError += OnError;
        webSocket.OnClosed += OnClosed;
    }

    private void antiInit()
    {
        webSocket.OnOpen = null;
        webSocket.OnMessage = null;
        webSocket.OnError = null;
        webSocket.OnClosed = null;
        webSocket = null;
    }

    private void setConsoleMsg(string msg)
    {
        console.text = "Message: " + msg;
    }

    public void Connect()
    {
        webSocket.Open();
    }

    private byte[] getBytes(string message)
    {

        byte[] buffer = Encoding.Default.GetBytes(message);
        return buffer;
    }

    public void Send()
    {
     //   webSocket.Send(msg.text);
    }

    public void Send(string str)
    {
        RQ_StartGame StartGame = new RQ_StartGame();
        StartGame.serverId = 1;
        StartGame.token = "2";
        BaseData data = new BaseData();
        data.code = 101;
        Extensible.AppendValue(data, 101, StartGame);
      
        byte[] b = ProtobufSerilizer.Serialize(data);
        webSocket.Send(b);
    }

    public void Close()
    {
        webSocket.Close();
    }

    #region WebSocket Event Handlers

    /// <summary>
    /// Called when the web socket is open, and we are ready to send and receive data
    /// </summary>
    void OnOpen(WebSocket ws)
    {
        Debug.Log("connected");
        setConsoleMsg("Connected");
    }

    /// <summary>
    /// Called when we received a text message from the server
    /// </summary>
    void OnMessageReceived(WebSocket ws, byte[] bytes)
    {
        // data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
        BaseData baseData = ProtobufSerilizer.DeSerialize<BaseData>(bytes);
        if (baseData.code == 2001)
        {
            A_ErrorMessage errorMessage = Extensible.GetValue<A_ErrorMessage>(baseData, 2001);
        }
        //else
        //{

        //RS_StartGame startGame =  Extensible.GetValue<RS_StartGame>(baseData, 102);
        //    string data = startGame.ret + " " + startGame.state + " " + baseData.code;
        //    Debug.Log(data);
        //}

        //string data = startGame.ret + "\r\n" + startGame.state + "\r\n" + baseData.code;
        //Debug.Log(data);
        //setConsoleMsg(data);
    }

    /// <summary>
    /// Called when the web socket closed
    /// </summary>
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Debug.Log(message);
        setConsoleMsg(message);
        antiInit();
        init();
    }

    private void OnDestroy()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            webSocket.Close();
            antiInit();
        }
    }

    /// <summary>
    /// Called when an error occured on client side
    /// </summary>
    void OnError(WebSocket ws, Exception ex)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif
        Debug.Log(errorMsg);
        setConsoleMsg(errorMsg);
        //antiInit();
        //init();
    }

    #endregion  

}