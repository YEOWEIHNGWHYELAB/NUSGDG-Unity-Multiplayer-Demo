using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    [PunRPC]
    void TakeDamageRPC(int damage)
    {
        slider.value -= damage;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void TakeDamage(int damage)
    {
        view.RPC("TakeDamageRPC", RpcTarget.All);
    }
}
