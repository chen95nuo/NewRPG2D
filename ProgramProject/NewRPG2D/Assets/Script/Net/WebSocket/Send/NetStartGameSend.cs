using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto.SLGV1;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetStartGameSend : ISend
    {
        RQ_StartGame mStartGame;

        public NetStartGameSend()
        {
            mStartGame = new RQ_StartGame();
        }

        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("登陆成功发送拉取数据消息 RQ_StartGame  token = " + param[0]);
            mStartGame.token = (string)param[0];
            DebugHelper.Log(mStartGame.token);
            return mStartGame;
        }
    }
}
