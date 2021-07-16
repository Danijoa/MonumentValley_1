using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickButton : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPos;

    private RotateTriangle rotateTriangle;
    private bool isOnce;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rotateTriangle = GameObject.FindObjectOfType<RotateTriangle>();
        isOnce = true;
    }

    void Update()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y - 0.8f, player.transform.position.z);
        Vector3 temp = transform.position - playerPos;
        if (temp.magnitude < 0.1f && isOnce)
        {
            rotateTriangle.DoRotation();
            isOnce = false;
        }
    }
}
