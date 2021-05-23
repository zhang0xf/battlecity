using UnityEngine;

public class LoadState : StateBase
{
    private static LoadState mInstance = null;

    private LoadState() { }

    public static LoadState Instance
    {
        get 
        {
            if (null == mInstance)
            {
                mInstance = new LoadState();
                mInstance.AddState(GameState.LOAD);
            }
            return mInstance;
        }
    }

    public override void AddState(GameState state)
    {
        base.AddState(state);
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"Load State\"."));
        UIManager.Instance.OpenUI(UIType.GAME_START_UI);
        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
