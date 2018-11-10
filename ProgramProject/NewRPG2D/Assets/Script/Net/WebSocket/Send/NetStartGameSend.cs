using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo;
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
            mStartGame.serverId = 1;
            mStartGame.token = "2";
            return mStartGame;
        }
    }
}
