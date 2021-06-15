using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDelegate : MonoBehaviour
{
    public MyEvent YourEvent;   

    // Start is called before the first frame update
    void Start()
    {
        YourEvent = new MyEvent();

        // 为事件添加订阅
        YourEvent.StateChange += new SomethingHappenHandler(StateChangeHandler);
    }

    // Update is called once per frame
    void Update()
    {
        YourEvent.EventState = State.NewState;
        YourEvent.IsChanged();
    }

    // 委托定义
    public void StateChangeHandler(string str) 
    {
        Debug.Log(str);
    }

}
