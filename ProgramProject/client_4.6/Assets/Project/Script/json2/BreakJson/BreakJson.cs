using UnityEngine;
using System.Collections.Generic;

public class BreakJson : BasicJson 
{
	/**target card 索引**/
	public int index;
	// select card list	
    public List<int> list;
	// break
    public BreakJson(int index, List<int> pe)
	{
		this.index=index;
        this.list = pe;
	}

}
