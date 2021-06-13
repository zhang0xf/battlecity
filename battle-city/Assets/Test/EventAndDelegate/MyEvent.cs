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
            
            // �¼����û�б����ģ�ע�ᣩ����Ϊnull��
            if (StateChange != null)
            {
                StateChange("�¼�����Ӧ��ί�з��͵ı�Ҫ����"); // �����¼�����������string
            }
            else
            {
                Debug.Log("�¼�û�ж���");
            }
        }
    }
}
