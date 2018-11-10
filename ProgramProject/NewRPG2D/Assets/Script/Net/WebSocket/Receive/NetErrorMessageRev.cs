using demo;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetErrorMessageRev : IReceive
    {
        public void Receive(IExtensible data, int Id)
        {
            A_ErrorMessage errorMessage = Extensible.GetValue<A_ErrorMessage>(data, Id);
            DebugHelper.Log("msg == " + errorMessage.msg);
        }

    }
}
