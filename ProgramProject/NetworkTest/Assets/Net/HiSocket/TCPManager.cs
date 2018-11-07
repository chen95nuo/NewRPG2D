using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HiSocket;
using System;

public class TCPManager : MonoBehaviour
{

    private string accName, authKey, connectIp;
    private long playerId;
    private int merId, fromId, loginId, exTcpPort, exUdpPort;
    private byte antiAddiction, isGm;

    private readonly IMsgRegister _register = new MsgRegister();
    private ISocket _socket, _gameSocket;
    private IByteArray _byteArray = new ByteArray();
    private MsgBytes _msgSend, _gameMsgSend;
    public uint clientSeqNum = 0;
    public byte[] strPrivateKey;
    private const string strPublicKey = "yjT[s8@$7AjH^%o$@5@$g^d4";
    private static TCPManager instance;
    public static TCPManager Ins
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        strPrivateKey = null;
    }

    void Start()
    {
        _msgSend = new MsgBytes(_byteArray);
        IPackage iPackage = new PackageTCP(_register);
        _socket = new TcpClient(iPackage);
        _socket.StateEvent += OnStateChange;
        _register.Regist((int)NetMsg.RAC_EX_GET_SERIALNUM_RES, OnGetSerialnumRes);
        SendConnect("47.97.218.131", 5101);

    }

    public void SendConnect(string IPStr, int port)
    {
        _socket.Connect(IPStr, port);
    }

    public void SendGameConnect(string IPStr, int port)
    {
        _gameSocket.Connect(IPStr, port);
    }

    public void OnApplicationQuit()
    {
        _socket.DisConnect();
    }

    public delegate void DelegateSendFunction(MsgBytes msg);

    public void sendMsg(DelegateSendFunction func)
    {
        _msgSend._iByteArray.Clear();
        func(_msgSend);
        _socket.Send(_msgSend._iByteArray.ToArray());
    }
    private readonly object _locker =new object();
    public void sendGameMsg(DelegateSendFunction func)
    {
        lock(_locker)
        {
            _gameMsgSend._iByteArray.Clear();
            func(_gameMsgSend);
            _gameSocket.Send(_gameMsgSend._iByteArray.ToArray());
        }
    }

    #region send
    public void OnGetSerialnumReq()
    {
        sendMsg(msg =>
        {
            msg.Write<uint>((uint)NetMsg.RAC_EX_GET_SERIALNUM_REQ);
            msg.Write<uint>(clientSeqNum);
        });
    }

    public void OnGetGameSerialnumReq()
    {
        sendGameMsg(msg =>
        {
            msg.Write<uint>((uint)NetMsg.RAC_EX_GET_SERIALNUM_REQ);
            msg.Write<uint>(clientSeqNum);
        });
    }


    public void SendCreatePlayer(string accountName)
    {
        sendMsg(msg =>
        {
            msg.Write<uint>((uint)NetMsg.RAC_EX_CREATEROLE_REQ);
            msg.Write<uint>(clientSeqNum);
            msg.Write<long>(playerId);
            msg.Write<byte>((byte)accountName.Length);
            msg.Write<string>(accountName);
            msg.Write<UInt32>(1);
        });
    }

    public void SendAuthorize(int loginId, string accName, string accPwd)
    {
        sendMsg(msg =>
        {
            msg.Write<uint>((uint)NetMsg.RAC_EX_AUTHORIZE_REQ);
            msg.Write<uint>(clientSeqNum);
            msg.Write<int>(loginId);
            msg.Write<byte>((byte)accName.Length);
            msg.Write((string)accName);
            msg.Write<byte>((byte)accPwd.Length);
            msg.Write((string)accPwd);
        });
    }


    public void SendLogin()
    {
        sendMsg(msg =>
        {
            msg.Write<uint>((uint)NetMsg.RAC_EX_LOGIN_REQ);
            msg.Write<uint>(clientSeqNum);
            msg.Write<byte>((byte)accName.Length);
            msg.Write<string>(accName);
            msg.Write<long>(playerId);
            msg.Write<int>(merId);
            msg.Write<int>(fromId);
            msg.Write<int>(loginId);
            msg.Write<byte>((byte)authKey.Length);
            msg.Write<string>(authKey);
            msg.Write<byte>(antiAddiction);
            msg.Write<byte>(isGm);
        });
    }


    #endregion

    #region receive
    #region Hall
    private void OnStateChange(SocketState state)
    {
        Debug.LogError(" OnStateChange result  " + state);
        if (state == SocketState.Connected) OnGetSerialnumReq();
    }

    private void OnGameStateChange(SocketState state)
    {
        if (state == SocketState.Connected) OnGetGameSerialnumReq();
    }

    private void OnGetSerialnumRes(IByteArray bytes, int length)
    {
        if (length > 0)
        {
            bytes.Read(length);
            Debug.LogError("OnGetSerialnumRes parse Error!");
        }
    }

    private void OnRegistacc(IByteArray bytes, int length)
    {
        Int32 result = bytes.ReadByte<Int32>(); length -= 4;
        clientSeqNum++;
        ResultMsg(" OnRegistacc", result, length);
        if (result == 0)
        {
        }
    }

    private void OnAuthorize(IByteArray bytes, int length)
    {
        Int32 result = bytes.ReadByte<Int32>(); length -= 4;
        clientSeqNum++;

        ResultMsg(" OnAuthorize", result, length);
        if (result == 0)
        {
           
        }
    }

    private void OnLogin(IByteArray bytes, int length)
    {
        Int32 result = bytes.ReadByte<Int32>(); length -= 4;
        long playerId = bytes.ReadByte<long>(); length -= 8;
        clientSeqNum++;
        ResultMsg(" OnLogin", result, length);
        if (result == 0)
        {
        }
        else
        {
          
        }
    }



    private void OnCreaterole(IByteArray bytes, int length)
    {
        Int32 result = bytes.ReadByte<Int32>(); length -= 4;
        long playerId = bytes.ReadByte<Int64>(); length -= 8;
        clientSeqNum++;

        ResultMsg(" OnCreaterole", result, length);
        if (result == 0)
        {
           
        }
    }

 
    #endregion

    #endregion

    private void ResultMsg(string methodName, int resultId, int length)
    {
        if (resultId == 0)
        {
            Debug.LogFormat(" {0} successful ", methodName);
        }
        else
        {
            Debug.LogErrorFormat(" {0} result  {1}", methodName, resultId);
        }
        if (length > 0)
        {
            Debug.LogErrorFormat("{0} parse Error!", methodName);
        }
    }
}
