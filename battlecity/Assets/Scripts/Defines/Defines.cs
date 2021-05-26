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

// UI使用栈管理
// 不使用状态机的原因：状态的切换与UI的选择耦合
// 那么UI如何使用状态机？
public enum GameState
{ 
    NONE,
    MIAN_MENU,
    START,
    IN_GAME,
    GAME_OVER,
    QUIT
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