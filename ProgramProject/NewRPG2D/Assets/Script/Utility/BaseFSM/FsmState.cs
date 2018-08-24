using UnityEngine;

/// <summary>
/// 状态基类,定义了状态的基础方法
/// 为什么需要定义为3个呢
/// 是因为有些逻辑都是进入状态或者退出状态的时候执行的,
/// 所以这里将逻辑分开:
/// 分为:
/// 1>进入状态,
/// 2>状态执行,
/// 3>状态退出
/// </summary>
/// <typeparam name="TState">使用泛型,哪个状态使用,就传入哪个状态</typeparam>
public class FsmState<TState>
{

    /// <summary>
    /// 使用状态的对象
    /// </summary>
    public TState Target;

    /// <summary>
    /// 进入状态的逻辑
    /// 只执行一次的逻辑
    /// </summary>
    /// <param name="TState"></param>
    public virtual void Enter(TState Target)
    {
        //Debug.Log(Target.ToString() +" state  "+ this.ToString() + "    Enter    " + Time.time);
    }

    /// <summary>
    /// 执行状态的逻辑
    /// </summary>
    /// <param name="TState"></param>
    public virtual void Update(TState Target, float deltaTime)
    {
    }

    /// <summary>
    /// 退出状态的逻辑
    /// </summary>
    /// <param name="TState"></param>
    public virtual void Exit(TState Target)
    {
       // Debug.Log(this.ToString() + "    Exit" + Time.time);
    }


}
