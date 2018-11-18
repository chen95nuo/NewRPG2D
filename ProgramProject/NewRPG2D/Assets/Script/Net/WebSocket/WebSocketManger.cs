
using UnityEngine;
using BestHTTP.WebSocket;
using System;
using UnityEngine.UI;
using System.Text;
using ProtoBuf;
using System.IO;
using demo;
using System.Collections.Generic;

namespace Assets.Script.Net
{
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

    public class WebSocketManger : TSingleton<WebSocketManger>
    {
        public const string defaultUrl = "ws://39.104.79.77:9010/websocket";

        private string currentUrl;
        private WebSocket webSocket;
        private BaseData baseData;
        private Dictionary<int, ISend> cacheSend;

        private string token; //玩家唯一识别码

        public override void Init()
        {
            base.Init();
            baseData = new BaseData();
            //InitSocket();
        }

        public override void Dispose()
        {
            base.Dispose();
            AntiInit();
        }

        public void GameStartMessaget(string url, string token)
        {
            InitSocket(url);
            this.token = token;
        }

        public void InitSocket(string url)
        {
            currentUrl = url;
            webSocket = new WebSocket(new Uri(url));
            webSocket.OnOpen += OnOpen;
            webSocket.OnBinary += OnMessageReceived;
            webSocket.OnError += OnError;
            webSocket.OnClosed += OnClosed;
            Connect();
        }

        private void AntiInit()
        {
            webSocket.OnOpen = null;
            webSocket.OnMessage = null;
            webSocket.OnError = null;
            webSocket.OnClosed = null;
            webSocket = null;
        }

        private void setConsoleMsg(string msg)
        {
            Debug.Log("Message: " + msg);
        }

        public void Connect()
        {
            webSocket.Open();
        }

        public void Send(NetSendMsg netId, params object[] param)
        {
            int codeId = (int)netId;
            IExtensible sendData = GetSendData(netId, param);
            if (sendData == null)
            {
                Debug.LogError(" NetMsg " + netId + "  data is error"); return;
            }
            baseData.code = codeId;
            Extensible.AppendValue(baseData, codeId, sendData);
            byte[] b = ProtobufSerilizer.Serialize(baseData);
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
            setConsoleMsg("Connected");
            Send(NetSendMsg.RQ_StartGame, 1, token);
        }

        /// <summary>
        /// Called when we received a text message from the server
        /// </summary>
        void OnMessageReceived(WebSocket ws, byte[] bytes)
        {
            string data = "";
            BaseData tempBaseData = ProtobufSerilizer.DeSerialize<BaseData>(bytes);
            SetReceiveData((NetReceiveMsg)tempBaseData.code, tempBaseData);
            setConsoleMsg(data);
        }

        /// <summary>
        /// Called when the web socket closed
        /// </summary>
        void OnClosed(WebSocket ws, UInt16 code, string message)
        {
            Debug.Log(message);
            setConsoleMsg(message);
            AntiInit();
        }

        private void OnDestroy()
        {
            if (webSocket != null && webSocket.IsOpen)
            {
                webSocket.Close();
                AntiInit();
            }
        }

        /// <summary>
        /// Called when an error occured on client side
        /// </summary>
        private void OnError(WebSocket ws, Exception ex)
        {
            string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws.InternalRequest.Response != null)
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif
            Debug.Log(errorMsg);
            setConsoleMsg(errorMsg);
            AntiInit();
            InitSocket(currentUrl);
        }

        #endregion


        private IExtensible GetSendData(NetSendMsg netId, params object[] param)
        {
            IExtensible data = null;
            ISend send = null;
            switch (netId)
            {
                case NetSendMsg.RQ_StartGame:
                    send = new NetStartGameSend();
                    break;
                case NetSendMsg.Q_HG_RefreshTili:
                    send = new NetRefreshTiliSend();
                    break;
                default:
                    break;
            }
            data = send.Send(param);
            return data;
        }


        private void SetReceiveData(NetReceiveMsg netId, IExtensible receiveData)
        {
            IReceive receive = null;
            switch (netId)
            {
                case NetReceiveMsg.RS_StartGame:
                    receive = new NetStartGameRev();
                    break;
                case NetReceiveMsg.A_HG_Tili:
                    break;
                case NetReceiveMsg.A_ErrorMessage:
                    receive = new NetErrorMessageRev();
                    break;
                default:
                    break;

            }
            receive.Receive(receiveData, (int)netId);
        }

    }
}