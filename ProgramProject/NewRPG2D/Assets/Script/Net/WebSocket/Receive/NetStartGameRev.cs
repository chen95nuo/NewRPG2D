using Assets.Script.UIManger;
using demo;
using ProtoBuf;
using UnityEngine.SceneManagement;


namespace Assets.Script.Net
{
    public class NetStartGameRev : IReceive
    {
        public void Receive(IExtensible data, int Id)
        {
            RS_StartGame mStartGame = Extensible.GetValue<RS_StartGame>(data, Id);
            int ret = mStartGame.ret;
            int state = mStartGame.state;

            if (ret == 0) //角色获取成功
            {
                DebugHelper.Log("角色获取成功");
                switch (state)
                {
                    case 0:
                        DebugHelper.Log("未创建角色名");
                        break;
                    case 1:
                        DebugHelper.Log("已创建角色名");
                        break;
                    case 2:
                        DebugHelper.Log("禁止登陆");
                        return;
                    default:
                        break;
                }
                WebSocketManger.instance.Send(NetSendMsg.RQ_RoleLogin);
                UIPanelManager.instance.allPages.Clear();
                UIPanelManager.instance.currentPageNodes.Clear();
                SceneManager.LoadSceneAsync("EasonMainScene");
            }
            DebugHelper.Log("角色获取失败");
            DebugHelper.Log("NetStartGameRev ==  " + ret + "  " + state);
        }
    }
}