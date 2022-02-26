using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    PhotonView view;
    public GameObject myBar;
    public GameObject broBar;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();

        myBar = GameObject.Find("HealthBar");
        broBar = GameObject.Find("MyBar");

        myBar.GetComponent<HealthBar>().SetMaxHealth();
        broBar.GetComponent<HealthBar>().SetMaxHealth();

        Debug.Log("Take Health");
    }

    private void Update()
    {
        if (view.IsMine)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * 2, Input.GetAxis("Vertical") * 2);

            if (PhotonNetwork.IsMasterClient)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    myBar.GetComponent<HealthBar>().TakeDamage();
                    Debug.Log("Take Damage On Player 1");
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    myBar.GetComponent<HealthBar>().HealHealth();
                    Debug.Log("Healing On Player 1");
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    broBar.GetComponent<HealthBar>().TakeDamage();
                    Debug.Log("Take Damage On Player 2");
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    broBar.GetComponent<HealthBar>().HealHealth();
                    Debug.Log("Healing On Player 2");
                }
            }
        }
    }
}
