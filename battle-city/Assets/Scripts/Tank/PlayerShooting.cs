using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private PlayerStatus m_PlayerStatus;

    public void SetPlayerStatus(int id)
    {
        m_PlayerStatus = PlayerConfig.Instance.GetPlayerStatus(id);
    }
}
