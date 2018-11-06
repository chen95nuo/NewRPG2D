using System;

namespace HiSocket {
    public interface IMsgRegister {
        void Regist (int id, Action<IByteArray, int> action);

        void Unregist (int id);

        void Dispatch (int id, IByteArray iByteArray, int length);
    }
}
