public enum ObjectState
{ 
    NONE,
    INITIAL,
    LOADING,
    READY,
    INVALID,
    RELEASING,
    CLOSED
}

public enum RegisterState
{ 
    NONE,   // 不需要注册
    NEED,   // 需要注册
    DONE    // 已注册
}

public enum GameState
{ 
    NONE,
    MENU,
    NEW,
    CONTINUE,
    SETTING,
    CUSTOMIZE,
    ONLINE,
    START,
    LOGIN,
    GAMEOVER,
    EXIT
}

public enum UIType : int
{ 
    NONE = 0,
    MAIN_MENU_UI,
    SETTING_UI,
}

public class SceneName
{
    public const string START_GAME = "StartGame";
    public const string MENU = "MainMenu";
}

// 委托
public delegate void MessageControlHandler(Notification notify);

public delegate void StateChangedHandler(object sender, ObjectState newState, ObjectState oldState);