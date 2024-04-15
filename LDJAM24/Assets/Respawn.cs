using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform player;

    public Transform respawnPoint;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           player.transform.position = respawnPoint.transform.position;
           player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
