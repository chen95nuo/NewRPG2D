using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetRefreshTiliSend : ISend
    {
        Q_HG_RefreshTili mRefreshTili;

        public NetRefreshTiliSend()
        {
            mRefreshTili = new Q_HG_RefreshTili();
        }

        public IExtensible Send(params object[] obj)
        {
            return mRefreshTili;
        }
    }
}
