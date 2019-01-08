using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ZMNetDataLikePhoton : ZMNetData {

	public short ReturnCode;
	public string DebugMessage;

	public ZMNetDataLikePhoton(byte[] data) : base(data)
	{

	}

	public ZMNetDataLikePhoton(short opCode,Dictionary<short,object> mParms) : base(opCode,(byte)1)
	{
		this.writeInt(-255);
		 this.putMapBO(mParms);
	}


}
