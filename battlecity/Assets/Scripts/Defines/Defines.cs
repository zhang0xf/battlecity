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
    MAIN_MENU,
    MAIN_MENU_NEW_GAME,
    MAIN_MENU_CONTINUE,
    MAIN_MENU_SETTING,
    MAIN_MENU_CUSTOMIZE,
    MAIN_MENU_ONLINE,
    MAIN_MENU_EXIT,
    SETTING_UI,
    SETTING_UI_AUDIO,
    SETTING_UI_CONTROLLERBIND,
    SETTING_UI_CONTROLLER,
    SETTING_UI_KEYBOARDBIND,
    SETTING_UI_KEYBOARD,
    LOGIN,
    NEW_GAME,
    CONTINUE_GAME,
    CUSTOMIZE_GAME,
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