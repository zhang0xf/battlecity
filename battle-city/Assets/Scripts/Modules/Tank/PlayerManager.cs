using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerManager : TankManager
{
    [HideInInspector] public int m_PlayerNumber;    // ���ID
    [HideInInspector] public GameObject m_Instance; // ��Ҷ���

}
