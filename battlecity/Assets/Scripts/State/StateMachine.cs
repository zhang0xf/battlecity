using UnityEngine;

public class StateMachine
{
    private StateBase mLastState = null;
    private StateBase mCurrState = null;
    private static StateMachine mInstance = null;

    private StateMachine()
    {
        // mState = new StateBase();
    }

    public static StateMachine Instance
    {
        get 
        {
            if (null == mInstance)
                mInstance = new StateMachine();
            return mInstance;
        }
    }

    public StateBase CurrState
    {
        set 
        {
            if (mCurrState != value)
            {
                mLastState = mCurrState;
                mCurrState = value;
                mCurrState.OnEnter();
            }
        }
        get { return mCurrState; }
    }

    public StateBase LastState
    {
        get { return mLastState; }
    }

    public void ChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.MIAN_MENU:
                Debug.Log(string.Format("Change To \"main_menu\" State."));
                CurrState = MainMenuState.Instance;
                break;
            case GameState.SETTING_UI:
                Debug.Log(string.Format("Change To \"setting_ui\" State."));
                CurrState = SettingUIState.Instance;
                break;
            case GameState.START:

                break;
            case GameState.IN_GAME:

                break;
            case GameState.GAME_OVER:

                break;
            case GameState.EXIT:

                break;
            default:
                break;
        }
    }
}
