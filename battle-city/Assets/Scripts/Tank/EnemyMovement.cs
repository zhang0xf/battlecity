using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyKind m_EnemyKind;



    public void SetEnemyKind(int id)
    {
        m_EnemyKind = EnemyConfig.Instance.GetEnemyKind(id);
    }
}
