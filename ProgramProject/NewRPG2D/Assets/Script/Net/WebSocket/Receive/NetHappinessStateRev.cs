using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetHappinessStateRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_HappinessState happinessState = Extensible.GetValue<A_HappinessState>(data, id);
            DebugHelper.Log("当前幸福度为====="+happinessState.happinessNum);
        }
    }
}
