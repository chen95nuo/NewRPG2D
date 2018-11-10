using demo;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetStartGameRev : IReceive
    {
        public void Receive(IExtensible data, int Id)
        {
            RS_StartGame mStartGame = Extensible.GetValue<RS_StartGame>(data, Id);
            int ret = mStartGame.ret;
            int state = mStartGame.state;

            DebugHelper.Log("NetStartGameRev ==  " + ret + "  " + state);
        }

    }
}