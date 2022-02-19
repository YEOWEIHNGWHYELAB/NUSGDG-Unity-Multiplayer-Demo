using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerMovement[] players;
    PlayerMovement nearestPlayer;

    public float speed;

    private void Start()
    {
        players = FindObjectsOfType<PlayerMovement>();
    }

    private void Update()
    {
        float distance1 = Vector2.Distance(transform.position, players[0].transform.position);
        float distance2 = Vector2.Distance(transform.position, players[1].transform.position);

        // Debug.Log(distance1);
        // Debug.Log(distance2);

        if (distance1 < distance2)
        {
            nearestPlayer = players[0];
        }
        else
        {
            nearestPlayer = players[1];
        }

        if (nearestPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, speed * Time.deltaTime);
        }
    }

}
