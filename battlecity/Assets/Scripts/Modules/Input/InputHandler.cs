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

    public Command HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            return new UpCommand();
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            return new DownCommand();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            return new LeftCommand();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            return new RightCommand();
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            return new FireCommand();
        else if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0)
            return new MouseMoveCommand();
        return null;
    }
}
