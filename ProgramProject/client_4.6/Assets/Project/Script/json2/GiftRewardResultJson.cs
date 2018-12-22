using UnityEngine;
using System.Collections;

public class GiftRewardResultJson : ErrorJson
{

    public int nextGift { get; set; }//下一个礼包的id，如果为0则为没有下一个礼包//

    public int nextOnline { get; set; } //距离下一个礼包的领取时间//

    public int linetime { get; set; } //剩余时间//
	// Use this for initialization

    public int getNextGift()
    {
        return nextGift;
    }

    public void setNextGift(int nextGift)
    {
        this.nextGift = nextGift;
    }

    public int getNextOnline()
    {
        return nextOnline;
    }

    public void setNextOnline(int nextOnline)
    {
        this.nextOnline = nextOnline;
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
