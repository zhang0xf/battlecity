using UnityEngine;

// 判断当前输入设备：键盘、控制器
// 废弃原因：使用"new input system"去实现不同设备的切换更简单！

public class InputControll : MonoBehaviour
{
    public enum InputState
    { 
        MouseAndKeyBoard,
        Controller
    }

    private InputState mState = InputState.MouseAndKeyBoard;


    public InputState GetInputState()
    {
        return mState;
    }

    private void OnGUI()
    {
        if (isMouseAndKeyBoardState())
        {
            mState = InputState.MouseAndKeyBoard;
            Debug.Log("Current input state is MouseAndKeyBoard");
        }
        else if (isControllerState())
        {
            mState = InputState.Controller;
            Debug.Log("Current input state is Controller");
        }
    }

    private bool isMouseAndKeyBoardState()
    {
        // mouse & keyboard buttons
        if (Event.current.isKey || Event.current.isMouse)
        {
            return true;
        }

        // mouse movement
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return true;
        }

        return false;
    }

    private bool isControllerState()
    {
        // joystick buttons
        if(Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19))
        {
            return true;
        }

        // joystick axis
        if (Input.GetAxis("XC Left Stick X") != 0.0f ||
           Input.GetAxis("XC Left Stick Y") != 0.0f ||
           Input.GetAxis("XC Triggers") != 0.0f ||
           Input.GetAxis("XC Right Stick X") != 0.0f ||
           Input.GetAxis("XC Right Stick Y") != 0.0f)
        {
            return true;
        }

        return false;
    }



}
