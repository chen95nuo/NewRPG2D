using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInfoResultJson : ErrorJson
{
	// int value : 0 - no tip, 1 - N tip, 2 - UP tip
	public int ps1; // passiveSkill
	public int ps2;
	public int ps3;
	public int equip1;
	public int equip2;
	public int equip3;

    public int diamond;//ͻ����Ҫ�Ľ��������//
    public int pd; //��ҵ�ǰӵ�еĽ��������//
    public int cardN;//ͻ����Ҫ�Ŀ�������//
    public int pCardN;//���ӵ�д˿��Ƶ�����//
    public int multCard;//ͬ�Ǽ�������ͻ�ƿ�//

    public List<PackElement> pes; //ͻ�ƿ�������/
	
}

