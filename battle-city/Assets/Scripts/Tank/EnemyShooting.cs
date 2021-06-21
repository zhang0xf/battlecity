using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private EnemyKind m_EnemyKind;



    public void SetEnemyKind(int id)
    {
        m_EnemyKind = EnemyConfig.Instance.GetEnemyKind(id);
    }
}
