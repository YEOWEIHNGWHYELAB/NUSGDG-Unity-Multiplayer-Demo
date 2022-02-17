using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayerUI : MonoBehaviour
{
    public Canvas playerUI;

    private void Start()
    {
        Vector2 healthBarPos = new Vector2(-7, 4);
        PhotonNetwork.Instantiate(playerUI.name, healthBarPos, Quaternion.identity);
    }
}
