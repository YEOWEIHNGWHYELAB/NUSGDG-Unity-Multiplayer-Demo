using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text);
        Debug.Log("Create Room: " + createInput.text);
    }

    // Note that when you create room, you automatically join that room.
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
        Debug.Log("Joined to Room: " + joinInput.text);
    }

    // When you to load a multiplayer scene, you must use PhotonNetwork.LoadLevel and not the SceneManager.LoadScene
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScreen");
    }
}
