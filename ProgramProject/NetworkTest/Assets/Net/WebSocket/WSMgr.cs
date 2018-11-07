
using UnityEngine;
using BestHTTP.WebSocket;
using System;
using UnityEngine.UI;
using System.Text;
using ProtoBuf;
using System.IO;

[ProtoContract]
class MessageModel
{
    //添加特性，表示该字段可以被序列化，1可以理解为下标
    [ProtoMember(1)]
    public int ID { get; set; }
    [ProtoMember(2)]
    public string Name { get; set; }
    [ProtoMember(3)]
    public int code { get; set; }
}

[ProtoContract]
class Proto
{
    public static byte[] Serizlize(MessageModel meg)
    {
        try
        {
            //涉及格式转换，需要用到流，将二进制序列化到流中  
            using (MemoryStream ms = new MemoryStream())
            {
                //使用ProtoBuf工具的序列化方法  
                ProtoBuf.Serializer.Serialize<MessageModel>(ms, meg);

                //定义二级制数组，保存序列化后的结果  
                byte[] result = new byte[ms.Length];
                //将流的位置设为0，起始点  
                //ms.Seek(0, SeekOrigin.Begin);  
                ms.Position = 0;
                //将流中的内容读取到二进制数组中  
                ms.Read(result, 0, result.Length);

                return result;
            }
        }
        catch (Exception ex)
        {

            Debug.Log("序列化失败: " + ex.ToString());
            return null;
        }
    }

    public static MessageModel DeSerizlize(byte[] msg)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //将消息写入流中  
                ms.Write(msg, 0, msg.Length);
                //将流的位置归0  
                ms.Position = 0;
                //使用工具反序列化对象  
                MessageModel mm = ProtoBuf.Serializer.Deserialize<MessageModel>(ms);
                return mm;

            }
        }
        catch (Exception ex)
        {

            Debug.Log("反序列化失败: " + ex.ToString());
            return null;
        }
    }
}

public class WSMgr : MonoBehaviour
{

    //public string url = "ws://localhost:8080/web1/websocket";
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
        webSocket.Send(msg.text);
    }

    public void Send(string str)
    {
        MessageModel mm = new MessageModel();
        mm.ID = 1;
        mm.Name = "张三";
        mm.code = 101;
        //序列化数据  
        byte[] bytes = Proto.Serizlize(mm);

        webSocket.Send(bytes);
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
        //反序列化数据
        MessageModel mm = Proto.DeSerizlize(bytes);
        string data = mm.ID + "\r\n" + mm.Name + "\r\n" + mm.code;
        Debug.Log(data);
        setConsoleMsg(data);
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
        antiInit();
        init();
    }

    #endregion  

}