using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : int
{
    NULL,
    START,
    OPTION,
    ENTERGAME
}

public enum PlayerState : int
{
    NONE = -1,
    BASIC = 1,
    LONG_CANON,
    BIG_CANNON,
    ULTIMATE
}

public enum EnemyType : int
{ 
    NONE = -1,
    BASIC = 1,
    QUICK,
    ARMOR,
    BASIC_STRENGTHEN,
    QUICK_STRENGTHEN,
    ARMOR_STRENGTHEN
}

public class PlayerData
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float SheildTime { set; get; }
    public float Cooling { set; get; }
    public PlayerState State { set; get; }
}

public class EnemyData
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float Cooling { set; get; }
    public EnemyType Type { set; get; }
}

// Î¯ÍÐ
public delegate void MessageControlHandler(Notification notify);