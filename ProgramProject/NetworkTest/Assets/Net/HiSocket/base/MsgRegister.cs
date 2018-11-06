using System;
using System.Collections.Generic;

namespace HiSocket {
    public class MsgRegister : IMsgRegister {
        private Dictionary<int, Action<IByteArray, int>> _msgDic = new Dictionary<int, Action<IByteArray, int>> ();

        private readonly object _locker = new object ();

        public void Regist (int id, Action<IByteArray, int> action) {
            lock (_locker) {
                if (_msgDic.ContainsKey (id)) {
                    throw new Exception ("do not need to regist again:" + id);
                }
                _msgDic.Add (id, action);
            }
        }

        public void Unregist (int id) {
            lock (_locker) {
                if (!_msgDic.ContainsKey (id)) {
                    throw new Exception ("should regist first:" + id);
                }
                _msgDic.Remove (id);
            }
        }

        public void Dispatch (int id, IByteArray iByteArray, int length) {
            lock (_locker) {
                if (!_msgDic.ContainsKey (id)) {
                    iByteArray.Read (length);
                    return;
                    //throw new Exception ("should regist first:" + id);
                }
                _msgDic[id] (iByteArray, length);
            }
        }
    }
}
