public enum ObjectState
{ 
    NONE,
    INITIAL,
    LOADING,
    READY,
    RELEASING,
    CLOSE
}

public enum RegisterState
{ 
    NONE,   // 不需要注册
    NEED,   // 需要注册
    DONE    // 已注册
}

public enum GameState   // 游戏状态
{ 
    NONE,
    LOAD,
    OPTION,
    START,
    IN_GAME,
    GAME_OVER,
    QUIT
}

public enum UIType : int
{ 
    NONE = 0,
    GAME_START_UI,
}

// 委托
public delegate void MessageControlHandler(Notification notify);

public delegate void StateChangedHandler(object sender, ObjectState newState, ObjectState oldState);