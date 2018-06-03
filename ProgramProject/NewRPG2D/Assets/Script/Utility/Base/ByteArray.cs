using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket {
    public class ByteArray : IByteArray {
        private readonly List<byte> _bytes = new List<byte> ();
        private readonly object _locker = new object ();
        public int Length {
            get { return _bytes.Count; }
        }

        public byte[] Read (int length) {
            lock (_locker) {
                if (length > this._bytes.Count) {
                    throw new Exception ("length>_bytes's length");
                }
                byte[] bytes = new byte[length];
                for (int i = 0; i < length; i++) {
                    bytes[i] = this._bytes[i];
                }
                this._bytes.RemoveRange (0, length);
                return bytes;
            }
        }

        public void Write (byte[] bytes, int length) {
            lock (_locker) {
                if (length > bytes.Length) {
                    throw new Exception ("length>_bytes's length");
                }
                for (int i = 0; i < length; i++) {
                    this._bytes.Add (bytes[i]);
                }
            }
        }

        public void Insert (int index, byte[] bytes) {
            lock (_locker) {
                _bytes.InsertRange (index, bytes);
            }
        }

        public byte[] ToArray () {
            lock (_locker) {
                return _bytes.ToArray ();
            }
        }

        public void Clear () {
            lock (_locker) {
                _bytes.Clear ();
            }
        }

        public T ReadByte<T> (int length = 0) {
            if (typeof (T) == typeof (byte))
                return (T)Convert.ChangeType (Read (1)[0], typeof (T));
            if (typeof (T) == typeof (byte[])) {
                if (length == 0)
                    throw new Exception ("read byte[] length:" + length);
                return (T)Convert.ChangeType (Read (length), typeof (T));
            }
            if (typeof (T) == typeof (bool))
                return (T)Convert.ChangeType (BitConverter.ToBoolean (Read (sizeof (bool)), 0), typeof (T));
            if (typeof (T) == typeof (char))
                return (T)Convert.ChangeType (BitConverter.ToChar (Read (sizeof (char)), 0), typeof (T));
            if (typeof (T) == typeof (double))
                return (T)Convert.ChangeType (BitConverter.ToDouble (Read (sizeof (double)), 0), typeof (T));
            if (typeof (T) == typeof (short))
                return (T)Convert.ChangeType (BitConverter.ToInt16 (Read (sizeof (short)), 0), typeof (T));
            if (typeof (T) == typeof (int))
                return (T)Convert.ChangeType (BitConverter.ToInt32 (Read (sizeof (int)), 0), typeof (T));
            if (typeof (T) == typeof (long))
                return (T)Convert.ChangeType (BitConverter.ToInt64 (Read (sizeof (long)), 0), typeof (T));
            if (typeof (T) == typeof (float))
                return (T)Convert.ChangeType (BitConverter.ToSingle (Read (sizeof (float)), 0), typeof (T));
            if (typeof (T) == typeof (string)) {
                //index += length;
                if (length == 0)
                    throw new Exception ("read string length:" + length);
                return (T)Convert.ChangeType (Encoding.UTF8.GetString (Read (length)), typeof (T));
            }
            if (typeof (T) == typeof (ushort))
                return (T)Convert.ChangeType (BitConverter.ToUInt16 (Read (sizeof (ushort)), 0), typeof (T));
            if (typeof (T) == typeof (uint))
                return (T)Convert.ChangeType (BitConverter.ToUInt32 (Read (sizeof (uint)), 0), typeof (T));
            if (typeof (T) == typeof (ulong))
                return (T)Convert.ChangeType (BitConverter.ToUInt64 (Read (sizeof (ulong)), 0), typeof (T));
            throw new Exception ("can not get this type" + typeof (T));
        }
    }
}


