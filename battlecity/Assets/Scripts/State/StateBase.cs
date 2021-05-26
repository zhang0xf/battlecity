using System.Collections.Generic;

public class StateBase : IUpdate
{
    protected Command command = null; 
    private GameState mState = GameState.NONE;
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

    public static StateBase GetState(GameState state)
    {
        if (!dict.ContainsKey(state)) { return null; }
        else return dict[state];
    }

    public virtual void AddState(GameState state)
    {
        if (!dict.ContainsKey(state))
            dict.Add(state, this);  // this指向调用AddState()的对象
    }

    public virtual void UpdateUI() { }

    public virtual void UpdateScene() { }

    public virtual void OnEnter() { }

    public virtual void OnExcute() { }

    public virtual void OnLeave() { }

    public void OnUpdate() 
    {
        OnExcute();
    }
}