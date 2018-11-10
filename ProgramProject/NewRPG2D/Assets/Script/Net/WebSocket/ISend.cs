
using ProtoBuf;

namespace Assets.Script.Net
{
    interface ISend
    {
        IExtensible Send(params object[] param);
    }
}
