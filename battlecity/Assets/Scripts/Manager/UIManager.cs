using System.Collections;
using System.Collections.Generic;

public class UIManager
{
    private static UIManager mInstance = null;
    private UIManager()
    { 
        
    }

    private static UIManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new UIManager();
            return mInstance;
        }
    }


}
