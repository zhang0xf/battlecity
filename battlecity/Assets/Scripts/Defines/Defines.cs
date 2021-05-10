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
    NONE,
    BASIC,
    LONGCANON,
    BIGCANNON,
    ULTIMATE
}

public class PlayerData
{
    public int ID { set; get; }
    public string Name { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float SheildTime { set; get; }
    public float Cooling { set; get; }
}

public class EnemyData
{
    public int ID { set; get; }
    public string Name { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public bool Strengthened { set; get; }
    public float Cooling { set; get; }
}

// Î¯ÍÐ
public delegate void MessageControlHandler(Notification notify);



