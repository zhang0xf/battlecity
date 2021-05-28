// 使用消息降低三个模块的耦合度：
// 1. 输入处理模块：Command类
// 2. GUI界面显示模块：BaseUI类及其子类
// 3. 状态机模块：StateBase类及其子类
// 多态方案：逻辑复杂、耦合度高！

public class StateMachine
{
    private StateBase mLastState = null;
    private StateBase mCurrState = null;
    private static StateMachine mInstance = null;

    private StateMachine() { }

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
            if (mLastState != null)
            {
                mLastState.OnLeave();
            }
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

    public void ChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.MAIN_MENU:
                CurrState = MainMenuState.Instance;
                break;
            case GameState.MAIN_MENU_NEW_GAME:
                CurrState = MainMenuNewGameState.Instance;
                break;
            case GameState.MAIN_MENU_CONTINUE:
                CurrState = MainMenuContinueState.Instance;
                break;
            case GameState.MAIN_MENU_SETTING:
                CurrState = MainMenuSettingState.Instance;
                break;
            case GameState.MAIN_MENU_CUSTOMIZE:
                CurrState = MainMenuCustomizeState.Instance;
                break;
            case GameState.MAIN_MENU_ONLINE:
                CurrState = MainMenuOnlineState.Instance;
                break;
            case GameState.MAIN_MENU_EXIT:
                CurrState = MainMenuExitState.Instance;
                break;

            case GameState.SETTING_UI:
                CurrState = SettingUIState.Instance;
                break;
            case GameState.SETTING_UI_AUDIO:
                CurrState = SettingUIAudioState.Instance;
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
