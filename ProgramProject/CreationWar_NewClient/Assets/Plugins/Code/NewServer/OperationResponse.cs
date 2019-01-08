using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Zealm
{
	public class OperationResponse :ICloneable
	{
		public OperationResponse()
		{
		}

		public OperationResponse(short mOperationCode,short mReturnCode,Dictionary<short,object> mParameters,string mDebugMessage)
		{
			this.OperationCode=mOperationCode;
			this.ReturnCode=mReturnCode;
			this.Parameters=mParameters;
			this.DebugMessage=mDebugMessage;
		}

		public string DebugMessage;
		
		public Dictionary<short, object> Parameters;
		
		public short OperationCode;
		
		public short ReturnCode;

		#region ICloneable implementation
		public object Clone ()
		{
			return this.MemberwiseClone ();
		}
		#endregion
	}
}
