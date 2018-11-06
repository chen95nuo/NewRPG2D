using Assets.Main.Script.DataClass;
using HiSocket;
using System;
using System.Text;
using UnityEngine;

public class PackageTCP : IPackage {
    private IMsgRegister iMsgRegister;
    private IByteArray iByteArrayParse = new ByteArray ();
    public const string strPublicKey = "yjT[s8@$7AjH^%o$@5@$g^d4";
    public byte[] strPrivateKey = null;
    public PackageTCP (IMsgRegister mr) {
        iMsgRegister = mr;
    }


    public void Unpack (IByteArray bytes) {
        while (bytes.Length >= 4) {
            var lengthBytes = bytes.Read (4);
            int length = BitConverter.ToInt32 (lengthBytes, 0) - 4;
            if (bytes.Length >= length) {
                byte[] tempBytes = (DESHelper.GetDes3DecryptedText (GetKey (), bytes.Read (length)));
                iByteArrayParse.Write (tempBytes, tempBytes.Length);
                uint id = BitConverter.ToUInt32 ((iByteArrayParse.Read (4)), 0);
                //Debug.Log ("TCP msg:" + id);
                uint clientSeq = BitConverter.ToUInt32 ((iByteArrayParse.Read (4)), 0);
                //Debug.Log ("TCP clientSeq:" + clientSeq);
                //RFManager.Ins.UpdatePage (NPage.Log, "AddLog", "netid:" + id);
                iMsgRegister.Dispatch ((int)id, iByteArrayParse, iByteArrayParse.Length);
                iByteArrayParse.Clear ();
            }
            else {
                bytes.Insert (0, lengthBytes);//重新添加,等待下次拆包
                break;
            }
        }
    }

    public void Pack (IByteArray bytes) {
        byte[] tempBytes = (DESHelper.GetDes3EncryptedText (GetKey (), bytes.ToArray ()));
        bytes.Clear ();
        bytes.Write (tempBytes, tempBytes.Length);
        uint length = (uint)(bytes.Length + 4);
        byte[] lengthBytes = BitConverter.GetBytes (length);
        bytes.Insert (0, lengthBytes);//消息头写入长度
    }

    public void ResetPrivateKey () {
        strPrivateKey = null;
    }

    private byte[] GetKey () {
        byte[] key = null;
        if (strPrivateKey == null) {
            if (TCPManager.Ins.strPrivateKey == null) {
                key = Encoding.UTF8.GetBytes (strPublicKey);
            }
            else {
                strPrivateKey = TCPManager.Ins.strPrivateKey;
            }
        }

        if (strPrivateKey != null) {
            key = strPrivateKey;
        }

        return key;
    }
}