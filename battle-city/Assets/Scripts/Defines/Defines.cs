public enum ObjState
{ 
    NONE,
    INITIAL,
    LOADING,
    READY,
    INVALID,
    RELEASING,
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
    NEWGAME,
    CONTINUE,
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

// 委托
public delegate void MessageControlHandler(Notification notify);

public delegate void StateChangedHandler(object sender, ObjState newState, ObjState oldState);