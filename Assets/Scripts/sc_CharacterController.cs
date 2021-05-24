using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_CharacterController : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private float horizontalSpeed;

    private List<GameObject> cubes;

    [SerializeField]
    private GameObject pr_Cube;

    private BoxCollider boxCol;

    private bool addCube = false;
    private bool colWall = false;

    private static sc_CharacterController instance;

    public static sc_CharacterController _Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<sc_CharacterController>();
            }
            return instance;
        }
    }


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        cubes = new List<GameObject>();
        boxCol = gameObject.GetComponent<BoxCollider>();
        ColliderHeight();
    }


    void Update()
    {

        MovementCharacter();


    }

    private void MovementCharacter()
    {

        if (Input.GetMouseButton(0))
        {
            transform.Translate(Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime, 0f, forwardSpeed * Time.deltaTime);

        }
        else
        {
            transform.Translate(0, 0f, forwardSpeed * Time.deltaTime);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cube")
        {
            transform.GetChild(0).position=new Vector3(transform.GetChild(0).position.x,(transform.GetChild(0).position.y+(other.gameObject.transform.GetChild(0).transform.localScale.y*other.gameObject.transform.childCount*0.5f)+0.5f),transform.GetChild(0).position.z);

            for (int i = 0; i < other.gameObject.transform.childCount; i++)
            {
                
                GameObject cube = Instantiate(pr_Cube, new Vector3(transform.GetChild(transform.childCount - 1).position.x, transform.GetChild(transform.childCount - 1).position.y + 0.50f, transform.GetChild(transform.childCount - 1).position.z), Quaternion.identity);
                cube.transform.parent = this.transform;
            }
            addCube = true;
            GameObject.Destroy(other.gameObject);
            transform.GetChild(0).Translate(0, 0, 0);
            ColliderHeight();
        }

        if (other.tag == "Wall")
        {
            if (!colWall)
            {
                int a = other.transform.childCount;

                for (int i = 1; i <= a; i++)
                {
                    transform.GetChild(i).parent = null;

                }
                for (int k = 1; k < transform.childCount; k++)
                {
                    transform.GetChild(k).GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
                }
                colWall = true;
            }
        }


    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            colWall = false;
        }
    }

    private void ColliderHeight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            boxCol.size = new Vector3(boxCol.size.x, ((transform.GetChild(i).GetComponent<BoxCollider>().size.y * transform.GetChild(i).localScale.y) + boxCol.size.y), boxCol.size.z);
        }
    }

}
