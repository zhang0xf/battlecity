public class StateMachine : IUpdate
{
    private BaseScene mScene = null;
    private StateBase mLastState = null;
    private StateBase mCurrState = null;
    private static StateMachine mInstance = null;

    private StateMachine(BaseScene scene)
    {
        mScene = scene;
    }

    public static StateMachine Instance(BaseScene scene)
    {
        if (null == mInstance)
            mInstance = new StateMachine(scene);
        return mInstance;
    }

    public StateBase CurrState
    {
        set 
        {
            if (mLastState != null) // 允许：mCurrState = value
                mLastState.OnLeave();
            mLastState = mCurrState;
            mCurrState = value;
            mCurrState.OnEnter();
        }
        get { return mCurrState; }
    }

    public StateBase LastState
    {
        get { return mLastState; }
    }

    public void Update()
    {
        CurrState.OnExcute();
    }
}
