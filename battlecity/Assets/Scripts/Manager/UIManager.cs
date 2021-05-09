using System.Collections;
using System.Collections.Generic;

public class UIManager
{
    private static UIManager Instance = null;

    private static UIManager GetInstance()
    {
        if (Instance == null) {
            Instance =  new UIManager();
        }
        return Instance;
    }




}
