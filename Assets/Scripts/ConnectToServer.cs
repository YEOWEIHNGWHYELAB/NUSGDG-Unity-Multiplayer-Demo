using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Connect to Photon server in the start function. 
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // After connection, join the lobby.
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    // Then once joined, load up lobby scene.
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("LobbyScreen");
    }
}
