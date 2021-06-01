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
    private Command mouse_left = null;

    private static InputHandler mInstance = null;

    private InputHandler()
    {
        initKey();
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

    public Command HandleInput()
    {
        if (null == button_up ||
            null == button_down ||
            null == button_left ||
            null == button_right ||
            null == button_fire ||
            null == button_back)
        {
            return null;
        }

        if (Input.GetKeyDown(button_up.GetBindKey())) { return button_up; }
        else if (Input.GetKeyDown(button_down.GetBindKey())) { return button_down; }
        else if (Input.GetKeyDown(button_left.GetBindKey())) { return button_left; }
        else if (Input.GetKeyDown(button_right.GetBindKey())) { return button_right; }
        else if (Input.GetKeyDown(button_fire.GetBindKey())) { return button_fire; }
        else if (Input.GetKeyDown(mouse_left.GetBindKey())) { return mouse_left; }
        else if (Input.GetKeyDown(button_back.GetBindKey())) { return button_back; }
        else if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0) { return new MouseMoveCommand(KeyCode.None); }
        
        return null;
    }

    public KeyCode GetKeyCodeDown()
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                Debug.Log("KeyCode down: " + keycode);
                return keycode;
            }
        }
        return KeyCode.None;
    }

    public void initKey()
    {
        BindUpCommandToKey(KeyCode.UpArrow);
        BindDownCommandToKey(KeyCode.DownArrow);
        BindLeftCommandToKey(KeyCode.LeftArrow);
        BindRightCommandToKey(KeyCode.RightArrow);
        BindFireCommandToKey(KeyCode.Space);
        BindBackCommandToKey(KeyCode.Escape);
        BindMouseLeftCommandToKey(KeyCode.Mouse0);
    }

    public void BindUpCommandToKey(KeyCode keyCode)
    {
        button_up = new UpCommand(keyCode);
    }

    public void BindDownCommandToKey(KeyCode keyCode)
    {
        button_down = new DownCommand(keyCode);
    }

    public void BindLeftCommandToKey(KeyCode keyCode)
    {
        button_left = new LeftCommand(keyCode);
    }

    public void BindRightCommandToKey(KeyCode keyCode)
    {
        button_right = new RightCommand(keyCode);
    }

    public void BindFireCommandToKey(KeyCode keyCode)
    {
        button_fire = new FireCommand(keyCode);
    }

    public void BindBackCommandToKey(KeyCode keyCode)
    {
        button_back = new BackCommand(keyCode);
    }

    public void BindMouseLeftCommandToKey(KeyCode keyCode)
    {
        mouse_left = new MouseLeftCommand(keyCode);
    }
}
