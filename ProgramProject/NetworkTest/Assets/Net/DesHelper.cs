using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class DESHelper {
    /// <summary>
    /// 将密码转成直接数组
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] HexToBytes (String str) {
        if (str == null) {
            return null;
        }
        else if (str.Length < 2) {
            return null;
        }
        else {
            int len = str.Length / 2;
            byte[] buffer = new byte[len];
            for (int i = 0; i < len; i++) {
                var temp = str.Substring (i * 2, 2);
                buffer[i] = (byte)Convert.ToInt32 (temp, 16);
            }
            return buffer;
        }
    }
    /// <summary>
    /// 3DES加密
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] GetDes3EncryptedText (byte[] key, byte[] data, bool needCover = true) {

        return data;
        //byte[] k = new byte[24];
        //int len = data.Length;
        //if (needCover && data.Length % 8 != 0) {
        //    len = data.Length - data.Length % 8 + 8;
        //}
        //byte[] needData = null;
        //if (len != 0)
        //    needData = new byte[len];

        //for (int i = 0; i < len; i++) {
        //    needData[i] = 0x00;
        //}

        //Buffer.BlockCopy (data, 0, needData, 0, data.Length);

        //if (key.Length == 16) {
        //    Buffer.BlockCopy (key, 0, k, 0, key.Length);
        //    Buffer.BlockCopy (key, 0, k, 16, 8);
        //}
        //else {
        //    Buffer.BlockCopy (key, 0, k, 0, 24);
        //}
        //var des3 = new TripleDESCryptoServiceProvider ();
        //des3.Key = k;
        //des3.Mode = CipherMode.ECB;
        //if (needCover == false) des3.Padding = PaddingMode.None;

        //using (MemoryStream ms = new MemoryStream ())
        //using (CryptoStream cs = new CryptoStream (ms, des3.CreateEncryptor (), CryptoStreamMode.Write)) {
        //    cs.Write (data, 0, data.Length);
        //    cs.FlushFinalBlock ();
        //    return ms.ToArray ();
        //}
    }

    // 3DES解密
    public static byte[] GetDes3DecryptedText (byte[] key, byte[] data) {
        return data;
        //byte[] k = new byte[24];

        //int len = data.Length;
        //if (data.Length % 8 != 0)
        //{
        //    len = data.Length - data.Length % 8 + 8;
        //}
        //byte[] needData = null;
        //if (len != 0)
        //    needData = new byte[len];

        //for (int i = 0; i < len; i++)
        //{
        //    needData[i] = 0x00;
        //}

        //Buffer.BlockCopy(data, 0, needData, 0, data.Length);

        //if (key.Length == 16)
        //{
        //    Buffer.BlockCopy(key, 0, k, 0, key.Length);
        //    Buffer.BlockCopy(key, 0, k, 16, 8);
        //}
        //else
        //{
        //    Buffer.BlockCopy(key, 0, k, 0, 24);
        //}

        //var des3 = new TripleDESCryptoServiceProvider();
        //des3.Key = k;
        //des3.Mode = CipherMode.ECB;
        //des3.Padding = PaddingMode.Zeros;
        //DebugHelper.Log(" data.Length  === " + data.Length);
        //using (MemoryStream ms = new MemoryStream())
        //using (CryptoStream cs = new CryptoStream(ms, des3.CreateDecryptor(), CryptoStreamMode.Write))
        //{
        //    cs.Write(data, 0, data.Length);
        //    cs.FlushFinalBlock();
        //    return ms.ToArray();
        //}
    }

    /// <summary>
    /// 将加密结果转成字符串
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static String GetByte2Hex (byte[] data) {
        StringBuilder sb = new StringBuilder ();
        for (int i = 0; i < data.Length; i++) {
            String temp = string.Format ("{0:X}", ((int)data[i]) & 0xFF);
            for (int t = temp.Length; t < 2; t++) {
                sb.Append ("0");
            }
            sb.Append (temp);
        }
        return sb.ToString ();
    }
}