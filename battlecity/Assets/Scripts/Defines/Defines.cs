using UnityEngine;

// 对象状态
public enum ObjState
{
    INVALID,
    INITIAL,
    LOADING,
    READY,
    RELEASING,
}

// 对象是否注册
public enum RegisterState
{ 
    NONE,   // 不需要注册
    NEED,   // 需要注册
    DONE    // 已注册
}

// 游戏状态
public enum GameState
{ 
    NONE,
    MENU,
    NEWGAME,
    CONTINUE,
    CUSTOMIZE,
    ONLINE,
    LOGIN,
    GAMEOVER,
}

// UI
public enum UIType : int
{ 
    NONE = 0,
    MAIN_MENU_UI,
    SETTING_UI,
}

// 方向
public enum Direction
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

// 委托
public delegate void MessageControlHandler(Notification notify);
public delegate void StateChangedHandler(object sender, ObjState newState, ObjState oldState);
public delegate void GameObjectChangeHandler(object sender, GameObject obj);