using UnityEngine;

public class StateMachine
{
    private StateBase mState = null;
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

    public StateBase State
    {
        set 
        {
            mState = value;
            mState.OnEnter();
        }
        get { return mState; }
    }

    public void ChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.MIAN_MENU:
                Debug.Log(string.Format("Change To \"mian_menu\" State."));
                State = MainMenuState.Instance;
                break;
            case GameState.START:

                break;
            case GameState.IN_GAME:

                break;
            case GameState.GAME_OVER:

                break;
            case GameState.QUIT:

                break;
            default:
                break;
        }
    }
}
