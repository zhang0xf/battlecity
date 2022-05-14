using System;
using UnityEngine;

public class InputHandler
{
    private Command button_up = null;
    private Command button_down = null;
    private Command button_left = null;
    private Command button_right = null;
    private Command button_fire = null;
    private Command button_back = null;

    private static InputHandler mInstance = null;

    private InputHandler()
    {
        button_up = new UpCommand();
        button_down = new DownCommand();
        button_left = new LeftCommand();
        button_right = new RightCommand();
        button_fire = new FireCommand();
        button_back = new BackCommand();

        BindKey(button_up, KeyCode.UpArrow);
        BindKey(button_down, KeyCode.DownArrow);
        BindKey(button_left, KeyCode.LeftArrow);
        BindKey(button_right, KeyCode.RightArrow);
        BindKey(button_fire, KeyCode.Space);
        BindKey(button_fire, KeyCode.Mouse0);
        BindKey(button_back, KeyCode.Escape);
    }

    public static InputHandler Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new InputHandler();
            return mInstance;
        }
    }

    public void BindKey(Command command, KeyCode keyCode)
    {
        if (null == command) { return; }
        command.BindKey(keyCode);
    }

    public Command HandleInput()
    {
        if (null == button_up ||
            null == button_down ||
            null == button_left ||
            null == button_right ||
            null == button_fire ||
            null == button_back)
        {
            Debug.LogWarning("button not bind to command yet!");
            return null;
        }

        KeyCode keyCode = GetKeyCodeDown();
        if (keyCode == KeyCode.None) 
        {
            // Debug.LogWarning("KeyCode is null!");
            return null;
        }

        if (button_up.IsMatch(keyCode)) { return button_up; }
        else if (button_down.IsMatch(keyCode)) { return button_down; }
        else if (button_left.IsMatch(keyCode)) { return button_left; }
        else if (button_right.IsMatch(keyCode)) { return button_right; }
        else if (button_fire.IsMatch(keyCode)) { return button_fire; }
        else if (button_back.IsMatch(keyCode)) { return button_back; }
        
        return null;
    }

    public KeyCode GetKeyCodeDown()
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                Debug.Log("KeyCode : " + keycode);
                return keycode;
            }
        }
        return KeyCode.None;
    }
}
