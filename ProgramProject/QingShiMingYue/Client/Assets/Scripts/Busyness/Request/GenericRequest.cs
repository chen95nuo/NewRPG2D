using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

class GenericRequest : Request
{
}

// A generic response to process the request which do not depend on the result.
// Some protocol is done at client side and server side with the same logic.
class GenericResponse : Response
{
	public GenericResponse(int pRqstID)
		: base(pRqstID)
	{
	}

	public GenericResponse(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}
}
