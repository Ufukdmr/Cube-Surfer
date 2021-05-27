using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Parent : MonoBehaviour
{
    BoxCollider boxCol;
    [SerializeField]
    private GameObject pr_Cube;
    public int cubeCount;
    void Start()
    {
        boxCol=GetComponent<BoxCollider>();
        float k=0;
        InstantiateChildCube();
         for (int i = 0; i < transform.childCount; i++)
        {
            boxCol.size = new Vector3(boxCol.size.x, ((transform.GetChild(i).GetComponent<BoxCollider>().size.y * transform.GetChild(i).localScale.y) + boxCol.size.y), boxCol.size.z);
            k=i*0.3f;
            boxCol.center=new Vector3(0,k,0);
        }
    }


    void InstantiateChildCube()
    {
        for(int i=1;i<cubeCount;i++)
        {
            GameObject cube = Instantiate(pr_Cube, new Vector3(transform.GetChild(transform.childCount - 1).position.x, transform.GetChild(transform.childCount - 1).position.y + 0.60f, transform.GetChild(transform.childCount - 1).position.z), Quaternion.identity);
            cube.transform.parent = this.transform;
        }
    }

}

