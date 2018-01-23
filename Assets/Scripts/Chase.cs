//Written By Camille Dahdah
//Edited By Charbel Daoud 02/01/16

using UnityEngine;

public class Chase : MonoBehaviour
{

    public Transform player;
    public int moveSpeed = 4;
    public int maxDist = 10;
    public int minDist = 5;

    void Update()
    {
        transform.LookAt(player);

        if (Vector3.Distance(transform.position, player.position) >= minDist)
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
