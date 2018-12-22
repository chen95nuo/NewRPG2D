using UnityEngine;
using System.Collections.Generic;

public class PlayerResultJson : ErrorJson
{
	public List<PlayerElement> list;
	public string[] s;//模块解锁  id-是否解锁（0未解锁， 1解锁）//
	public int mark;//==0不需重新登录,1需要重新登录==//
}
