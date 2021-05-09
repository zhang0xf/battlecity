using System.Collections;
using System.Collections.Generic;

public enum GameState : int
{
    NULL,
    START,
    OPTION,
    ENTERGAME
}

public class NotificationNames 
{
    public static string TEST = "test";
    public static string LOAD_ENEMY = "load_enemy";

}

// Î¯ÍÐ
public delegate void MessageControlHandler(Notification notify);

