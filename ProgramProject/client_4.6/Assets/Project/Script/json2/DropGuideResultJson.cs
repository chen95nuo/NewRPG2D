using UnityEngine;
using System.Collections.Generic;

public class DropGuideResultJson : ErrorJson {
	//==当前itemId==//
	public int id;
	//==关卡id-是否解锁(1解锁)-已完成次数==//
	public List<string> ds;
}
