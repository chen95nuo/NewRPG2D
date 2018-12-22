using UnityEngine;
using System.Collections;

public class MailJson : BasicJson {
	//==邮件索引==//
	public int i;
	//==操作:1打开,2领取附件==//
	public int t;
	
	public MailJson(int index,int action)
	{
		i=index;
		t=action;
	}
}
