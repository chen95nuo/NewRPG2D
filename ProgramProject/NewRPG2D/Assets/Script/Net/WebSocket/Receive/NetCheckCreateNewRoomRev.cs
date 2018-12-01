using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.SLGV1;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetCheckCreateNewRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_CheckCreateNewRoom confirmKidAdult = Extensible.GetValue<RS_CheckCreateNewRoom>(data, id);
            UIMain.instance.RQCheckCreateNewRoom(confirmKidAdult);//发送资源验证结果
            if (confirmKidAdult.ret != 0)
            {
                DebugHelper.Log("新建房间资源验证失败=======");
                return;
            }
            DebugHelper.Log("新建房间资源验证成功=======");
        }
    }
}