using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KugelRespawn : MonoBehaviour
{
    //wenn LegoMensch aus Tor rausfällt, Haus zu schnell bewegt wird etc-> respwan an Spawnpoint
    public Transform respawnPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y <= -100)
        {
            transform.position = respawnPoint.transform.position;
        }
    }

}