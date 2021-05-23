using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase
{
    private GameState mState = GameState.NONE;
    private static Dictionary<GameState, StateBase> dict = null;

    // ��̬���캯��
    static StateBase()
    {
        dict = new Dictionary<GameState, StateBase>();
    }

    public GameState State
    {
        set { mState = value; }
        get { return mState; }
    }

    public static StateBase GetState(GameState state)
    {
        if (!dict.ContainsKey(state)) { return null; }
        else return dict[state];
    }

    public virtual void AddState(GameState state)
    {
        if (!dict.ContainsKey(state))
            dict.Add(state, this);  // thisָ�����AddState()�Ķ���
    }

    public virtual void OnEnter() { }

    public virtual void OnExcute() { }

    public virtual void OnLeave() { }
}