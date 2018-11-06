using HiSocket;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace HiSocket {
    public class TcpClient : ISocket {
        public int TimeOut {
            private get { return _timeOut; }
            set { _timeOut = value; }
        }

        public int ReceiveBufferSize {
            private get { return _receiveBufferSize; }
            set {
                _receiveBufferSize = value;
                _receiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get { return _client != null && _client.Client != null && _client.Connected; } }

        private string _ip;
        private int _port;
        private IPackage _iPackage;
        private System.Net.Sockets.TcpClient _client;
        private int _receiveBufferSize = 1024;//128k
        private byte[] _receiveBuffer;
        private int _timeOut = 5000;//5s:收发超时时间
        private readonly IByteArray _iByteArraySend = new ByteArray ();
        private readonly IByteArray _iByteArrayReceive = new ByteArray ();

        public TcpClient (IPackage iPackage) {
            _receiveBuffer = new byte[ReceiveBufferSize];
            _iPackage = iPackage;
            _client = new System.Net.Sockets.TcpClient ();
            _client.NoDelay = true;
            _client.SendTimeout = _client.ReceiveTimeout = TimeOut;
            //_client.Client.Blocking = false;
        }

        public void Connect (string ip, int port) {
            ChangeState (SocketState.Connecting);
            if (IsConnected) {
                ChangeState (SocketState.Connected);
                return;
            }
            try {
                _client.BeginConnect (ip, port, (delegate (IAsyncResult ar) {
                    try {
                        _client.EndConnect (ar);
                        if (_client.Connected) {
                            ChangeState (SocketState.Connected);
                            _client.Client.BeginReceive (_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, _client);
                        }
                        else ChangeState (SocketState.DisConnected);
                    }
                    catch (Exception e) {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception(e.ToString());
                    }
                }), _client);
            }
            catch (Exception e) {
                Debug.Log (e.ToString ());
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public void DisConnect () {
            if (IsConnected) {
                _client.Client.Shutdown (SocketShutdown.Both);
                _client.Close ();
                _client = null;
            }
            ChangeState (SocketState.DisConnected);
            StateEvent = null;
        }

        public void ReConnect()
        {
            _iPackage.ResetPrivateKey();
        }

        public void SetConnect(SocketState state)
        {
            ChangeState(state);
        }

        public void Send (byte[] bytes) {
            if (!IsConnected) {
                ChangeState (SocketState.DisConnected);
                throw new Exception ("receive failed");
            }
            try {
                _iByteArraySend.Clear ();
                _iByteArraySend.Write (bytes, bytes.Length);
                _iPackage.Pack (_iByteArraySend);
                var toSend = _iByteArraySend.Read (_iByteArraySend.Length);
                _client.Client.BeginSend (toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar) {
                    try {
                        _client.Client.EndSend (ar);
                    }
                    catch (Exception e) {
                        Debug.Log (e.ToString ());
                        ChangeState (SocketState.DisConnected);
                        throw new Exception (e.ToString ());
                    }
                }, _client);
            }
            catch (Exception e) {
                Debug.Log (e.ToString ());
                ChangeState (SocketState.DisConnected);
                throw new Exception (e.ToString ());
            }
        }

        void Receive (IAsyncResult ar) {
            if (!IsConnected) {
                ChangeState (SocketState.DisConnected);
                throw new Exception ("receive failed");
            }
            try {
                int length = _client.Client.EndReceive (ar);
                //Debug.Log("rev:" + length);
                if (length > 0) {
                    _iByteArrayReceive.Write (_receiveBuffer, length);
                    _iPackage.Unpack (_iByteArrayReceive);
                }
                _client.Client.BeginReceive (_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, _client);
            }
            catch (Exception e) {
                Debug.Log (e.ToString ());
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public long Ping () {
            IPAddress ipAddress = IPAddress.Parse (_ip);
            System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping ();
            System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send (ipAddress);
            return temPingReply.RoundtripTime;
        }

        private void ChangeState (SocketState state) {
            if (StateEvent != null) {
                StateEvent (state);
            }
        }
    }
}
