using UnityEngine;

public class InputHandler
{
    private static InputHandler mInstance = null;

    public static InputHandler Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new InputHandler();
            return mInstance;
        }
    }

    public Command UIInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            return new UISelectUp();
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            return new UISelectDown();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            return new UISelectLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            return new UISelectRight();
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            return new UIComfirm();
        return null;
    }
}
