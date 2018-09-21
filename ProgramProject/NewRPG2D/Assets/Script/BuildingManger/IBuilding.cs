using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生产
/// </summary>
public interface IProduction
{
    float Yield { get; }//产量
    float Stock { get; set; }//库存
}

/// <summary>
/// 储存
/// </summary>
public interface IStorage
{
    float Stock { get; set; }//库存
}
