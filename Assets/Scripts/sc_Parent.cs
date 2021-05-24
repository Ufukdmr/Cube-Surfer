using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Parent : MonoBehaviour
{
    BoxCollider boxCol;
    void Start()
    {
        boxCol=GetComponent<BoxCollider>();
        float k=0;
         for (int i = 0; i < transform.childCount; i++)
        {
            boxCol.size = new Vector3(boxCol.size.x, ((transform.GetChild(i).GetComponent<BoxCollider>().size.y * transform.GetChild(i).localScale.y) + boxCol.size.y), boxCol.size.z);
            k=i*0.5f;
            boxCol.center=new Vector3(0,k,0);
        }
    }

}

