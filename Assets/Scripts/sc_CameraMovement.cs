using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_CameraMovement : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Vector3 distance;
    void Start()
    {
        distance=transform.position-player.transform.position;
    }

    
    void Update()
    {
        transform.position=new Vector3(transform.position.x,transform.position.y,player.transform.position.z+distance.z);
    }
}
