using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_CameraMovement : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Vector3 distance;
    Vector3 distanceStart;

    Vector3 rotation;
    void Start()
    {
        distance=transform.position-player.transform.position;
        distanceStart=distance;
    }

    
    void Update()
    {
       MoveCamera(distance,rotation);
         
    }

    public void MoveCamera(Vector3 distance,Vector3 rotation)
    {
        this.rotation=rotation;
        if(rotation.y==0)
        {
            this.transform.position=new Vector3(transform.position.x,transform.position.y,player.transform.position.z);
        }
        else if(rotation.y==90)
        {            
            transform.position=new Vector3(player.transform.position.x,transform.position.y,transform.position.z); 
        }
        else if(rotation.y==180)
        {
             transform.position=new Vector3(player.transform.position.x,transform.position.y,transform.position.z); 
        }
        
    }
    

    
}
