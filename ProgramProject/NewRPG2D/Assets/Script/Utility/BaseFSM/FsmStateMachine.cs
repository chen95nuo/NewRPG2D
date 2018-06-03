using UnityEngine;

/// <summary>
/// 状态机
/// 使用这个来驱动各种状态
/// </summary>
/// <typeparam name="entitytype"></typeparam>
public class FsmStateMachine<TState>
{
    //状态持有对象
    private TState mOwner;

    //
    private FsmState<TState> mCurrentState;//当前状态
    private FsmState<TState> mPreviouState;//上一个状态
    private FsmState<TState> mGlobalState;//全局状态
    private int StateIndex = -1;


    /// <summary>
    /// 状态机的构造函数
    /// </summary>
    /// <param name="owner"></param>
    public FsmStateMachine(TState owner)
    {
        mOwner = owner;
        mPreviouState = null;
        mCurrentState = null;
        mGlobalState = null;
        StateIndex = -1;
    }

    /// <summary>
    /// 设置当前状态
    /// </summary>
    /// <param name="GlobalState"></param>
    public void SetCurrentState(FsmState<TState> CurrentState)
    {
        if (CurrentState != null)
        {
            //保存 当前状态
            mCurrentState = CurrentState;
            //设置 状态中的 Target 为
            mCurrentState.Target = mOwner;
            mCurrentState.Enter(mOwner);
        }
    }


    /// <summary>
    /// 进入全局状态
    /// 进行一些数据的刷新,比如说距离的计算等
    /// </summary>
    /// <param name="GlobalState"></param>
    public void SetGlobalState(FsmState<TState> GlobalState)
    {
        if (GlobalState != null)
        {
            mGlobalState = GlobalState;
            mGlobalState.Target = mOwner;
            mGlobalState.Enter(mOwner);
        }
        else
        {
            Debug.LogError("不能设置空状态");
        }
    }


    /// <summary>
    /// 进入全局状态d
    /// </summary>
    public void GlobalStateEnter()
    {

    }

    /// <summary>
    /// Update方法
    /// </summary>
    public void Update(float deltaTime)
    {
        //只要设置了.就会一直执行
        if (mGlobalState != null)
        {
            mGlobalState.Update(mOwner, deltaTime);
        }
        if (mCurrentState != null)
        {
            mCurrentState.Update(mOwner, deltaTime);
        }
    }


    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="pNewState">希望变成的状态</param>
    public void ChangeState(FsmState<TState> pNewState)
    {

        if (pNewState == null)
        {
            Debug.Log("不能使用空的状态");
        }
        if (pNewState == mCurrentState)
        {
            return;
        }
        if (mCurrentState == null)
        {
            return;
        }
        //触发状态退出方法
        mCurrentState.Exit(mOwner);

        //保存上一状态
        mPreviouState = mCurrentState;

        //设置新状态为当前状态
        mCurrentState = pNewState;
        //传递 状态 使用 对象
        mCurrentState.Target = mOwner;

        //触发当前状态调用Enter方法
        mCurrentState.Enter(mOwner);
    }

    /// <summary>
    /// 切换回上一状态
    /// </summary>
    public void RevertToPreviousState()
    {
        this.ChangeState(mPreviouState);
    }

    /// <summary>
    /// 获取当前状态
    /// </summary>
    /// <returns></returns>
    public FsmState<TState> GetCurrentState()
    {
        return mCurrentState;
    }

    public FsmState<TState> GetGlobalState()
    {
        return mGlobalState;
    }



    /// <summary>
    /// 获取上一状态
    /// </summary>
    /// <returns></returns>
    public FsmState<TState> GetPreviousState()
    {
        return mPreviouState;
    }
}
