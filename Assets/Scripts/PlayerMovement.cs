using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    PhotonView view;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * 2, Input.GetAxis("Vertical") * 2);
        }
    }
}
