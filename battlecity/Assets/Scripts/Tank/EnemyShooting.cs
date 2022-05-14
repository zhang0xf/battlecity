using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private TankInfo m_EnemyInfo;

    public void SetEnemyInfo(int kind)
    {
        m_EnemyInfo = TankConfig.Instance.GetEnemyInfo(kind);
    }
}
