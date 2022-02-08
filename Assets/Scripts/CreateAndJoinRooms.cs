using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    // Note that when you create room, you automatically join that room.
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    // When you to load a multiplayer scene, you must use PhotonNetwork.LoadLevel and not the SceneManager.LoadScene
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScreen");
    }
}
