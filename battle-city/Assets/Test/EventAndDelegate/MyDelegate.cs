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

        // Ϊ�¼���Ӷ���
        YourEvent.StateChange += new SomethingHappenHandler(StateChangeHandler);
    }

    // Update is called once per frame
    void Update()
    {
        YourEvent.EventState = State.NewState;
        YourEvent.IsChanged();
    }

    // ί�ж���
    public void StateChangeHandler(string str) 
    {
        Debug.Log(str);
    }

}
