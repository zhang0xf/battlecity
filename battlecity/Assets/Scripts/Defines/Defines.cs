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
    MIAN_MENU,
    SETTING_UI,
    LOGIN,
    START,
    IN_GAME,
    GAME_OVER,
    EXIT
}

public enum UIType : int
{ 
    NONE = 0,
    MAIN_MENU_UI,
    SETTING_UI,
}

// 委托
public delegate void MessageControlHandler(Notification notify);

public delegate void StateChangedHandler(object sender, ObjectState newState, ObjectState oldState);