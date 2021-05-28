using System.Collections.Generic;
using UnityEngine;

public class StateBase : IUpdate
{
    private BaseUI mBaseUI = null;
    private GameState mState = GameState.NONE;
    private Command mCommand = null;
    private static Dictionary<GameState, StateBase> dict = null;

    // 静态构造函数
    static StateBase()
    {
        dict = new Dictionary<GameState, StateBase>();
    }

    public GameState State
    {
        set { mState = value; }
        get { return mState; }
    }

    public BaseUI baseUI
    {
        set { mBaseUI = value; }
        get { return mBaseUI; }
    }

    public Command command
    {
        set { mCommand = value; }
        get { return mCommand; }
    }

    public static StateBase GetState(GameState state)
    {
        if (!dict.ContainsKey(state)) { return null; }
        else return dict[state];
    }

    public virtual void AddState(GameState state)
    {
        if (!dict.ContainsKey(state))
            dict.Add(state, this);
    }

    protected virtual bool IsUIReady(UIType uIType)
    {
        UIType peekType = UIManager.Instance.GetPeekUIType();
        if (peekType != uIType) { return false; }

        GameObject uiObject = UIManager.Instance.GetPeekUI();
        if (null == uiObject) { return false; }

        ObjectState state = UIManager.Instance.GetUIState(uiObject);
        if (state != ObjectState.READY) { return false; }

        return true;
    }

    public void OnUpdate()
    {
        OnExcute();
    }

    public virtual void OnEnter() { }

    public virtual void OnExcute() { }

    public virtual void OnLeave() { }
}