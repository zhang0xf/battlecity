public enum ObjectState
{ 
    NONE,
    INITIAL,
    LOADING,
    READY,
    RELEASING,
    CLOSE
}

public enum ObjectRegisterState
{ 
    NONE,
    NEED,
    DONE
}

public enum PlayerState : int
{
    NONE = -1,
    BASIC = 1,
    LONG_CANON = 2,
    BIG_CANNON = 3,
    ULTIMATE = 4
}

public enum EnemyType : int
{ 
    NONE = -1,
    BASIC = 1,
    QUICK = 2,
    ARMOR = 3,
    BASIC_STRENGTHEN = 4,
    QUICK_STRENGTHEN = 5,
    ARMOR_STRENGTHEN = 6
}

public class PlayerConfigData
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float SheildTime { set; get; }
    public float Cooling { set; get; }
    public PlayerState State { set; get; }
}

public class EnemyConfigData
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float Cooling { set; get; }
    public EnemyType Type { set; get; }
}

// 委托
public delegate void MessageControlHandler(Notification notify);

public delegate void StateChangedHandler(object sender, ObjectState newState, ObjectState oldState);