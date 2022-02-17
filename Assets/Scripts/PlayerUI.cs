using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;

    public HealthBar healthbar;

    PhotonView view;

    private void Awake()
    {
        healthbar = GetComponent<HealthBar>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        healthbar.SetMaxHealth(maxHealth);
        Debug.Log("Take Health");
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Take Damage");
                healthbar.TakeDamage(damage);
            }
        }
    }
}
