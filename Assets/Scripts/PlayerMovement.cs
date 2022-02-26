using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    PhotonView view;
    public HealthBar healthbar;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        healthbar = FindObjectOfType<HealthBar>();
        healthbar.SetMaxHealth();
        Debug.Log("Take Health");
    }

    private void Update()
    {
        if (view.IsMine)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * 2, Input.GetAxis("Vertical") * 2);

            if (Input.GetKeyDown(KeyCode.Space)) 
            { 
                healthbar.TakeDamage();
                Debug.Log("Take Damage");
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                healthbar.HealHealth();
                Debug.Log("Healing");
            }
        }
    }
}
