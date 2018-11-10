
using ProtoBuf;

namespace Assets.Script.Net
{
    interface IReceive
    {
        void Receive(IExtensible data, int id);
    }


}
