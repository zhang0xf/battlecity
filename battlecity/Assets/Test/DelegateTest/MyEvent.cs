using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEvent
{
    public event SomethingHappenHandler StateChange;
    public State EventState;

    public MyEvent() 
    {
        EventState = State.OldState;
    }

    public void IsChanged()
    {
        if (EventState == State.NewState) {
            
            // 事件如果没有被订阅（注册），即为null！
            if (StateChange != null)
            {
                StateChange("事件给相应的委托发送的必要数据"); // 触发事件，参数类型string
            }
            else
            {
                Debug.Log("事件没有订阅");
            }
        }
    }
}
