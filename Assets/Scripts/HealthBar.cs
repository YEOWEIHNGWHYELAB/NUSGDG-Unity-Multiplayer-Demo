using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    PhotonView view;
    public int maxhealth = 100;
    public int damage = 20;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void SetMaxHealth()
    {
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void TakeDamage()
    {
        view.RPC("TakeDamageRPC", RpcTarget.All);
    }

    [PunRPC]
    void TakeDamageRPC()
    {
        slider.value -= damage;
    }
}
