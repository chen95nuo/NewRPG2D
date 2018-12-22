using UnityEngine;
using System.Collections;

public class CornucopiaResultJson : ErrorJson
{
    public int id { get; set; } //已经聚宝到了第几个聚宝盆，若之前未聚宝，则为0 //

    public int time { get; set; }     //距离活动结束的时间，若活动结束则剩余时间为0，若活动没有时间限制则为-1//

    public int crestalPay { get; set; } //活动期间充值的水晶

}
