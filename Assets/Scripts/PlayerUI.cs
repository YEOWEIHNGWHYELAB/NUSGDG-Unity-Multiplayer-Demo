using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;

    Animator anim;

    public HealthBar healthbar;

    PhotonView view;

    private void Start()
    {
        anim = GetComponent<Animator>();
        view = GetComponentInChildren<PhotonView>();
        healthbar = FindObjectOfType<HealthBar>();
        healthbar.SetMaxHealth();
        // Debug.Log("Take Health");
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                healthbar.TakeDamage();
                Debug.Log("Take Damage");
            }
        } 
    }
}
