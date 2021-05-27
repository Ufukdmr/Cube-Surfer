using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class sc_CharacterController : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private float horizontalSpeed;

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private GameObject pr_Cube;

    private BoxCollider boxCol;

    private GameObject platform;

    private int childCount=0;

  private bool colWall = false;

    private bool canMove = true;

    private bool moveForward = true;
    private bool moveRight = false;

    private float magmaTime = 0.5f;
    private float nextBreakTime = 0f;

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
        boxCol = gameObject.GetComponent<BoxCollider>();
        ColliderHeight();
    }


    void Update()
    {
        if (GameManager._Instance.isStart)
        {
            MovementCharacter();          
        }      
        
    }

    private void MovementCharacter()
    {
        if (!GameManager._Instance.isGameOver && !GameManager._Instance.isPassLevel)
        {
            if (Input.GetMouseButton(0) && canMove)
            {
                transform.Translate(Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime, 0f, forwardSpeed * Time.deltaTime);
                MoveLimit();
            }
            else
            {
                transform.Translate(0, 0f, forwardSpeed * Time.deltaTime);
            }
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cube")
        {
            
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x, (transform.GetChild(0).position.y + (other.gameObject.transform.GetChild(0).transform.localScale.y * other.gameObject.transform.childCount * 0.6f) + 0.6f), transform.GetChild(0).position.z);

            for (int i = 0; i < other.gameObject.transform.childCount; i++)
            {

                GameObject cube = Instantiate(pr_Cube, new Vector3(transform.GetChild(transform.childCount - 1).position.x, transform.GetChild(transform.childCount - 1).position.y + 0.60f, transform.GetChild(transform.childCount - 1).position.z), Quaternion.identity);
                cube.transform.parent = this.transform;
            }
            GameObject.Destroy(other.gameObject);
            transform.GetChild(0).Translate(0, 0, 0);
            ColliderHeight();
        }

        if (other.tag == "Wall")
        {
            canMove = false;

            if (!colWall)
            {
                int childCount = other.gameObject.transform.childCount;
                BreakPlayerCube(childCount);
            }
        }
        if (other.tag == "HighPlatform")
        {
            int a = Convert.ToInt32(other.name);
            if(GameManager._Instance.isFinish)
            {
                childCount=a;
            }
            
            BreakPlayerCube(a);  
            
            
         
        }      
        if (other.tag == "Cristal")
        {
            GameObject.Destroy(other.gameObject);
            GameManager._Instance.Score();
        }
        if (other.tag == "Platform")
        {
            platform = other.gameObject;      
        }
        if (other.tag == "Right")
        {

            StartCoroutine(Rotate(Vector3.up * 90, 0.6f, this.gameObject));
            StartCoroutine(Rotate(Vector3.up * 90, 0.8f, cam));
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + 3.45f, transform.position.z);
            canMove = false;


        }
        if (other.tag == "Left")
        {

            StartCoroutine(Rotate(Vector3.up * -90, 0.6f, this.gameObject));
            StartCoroutine(Rotate(Vector3.up * -90, 0.8f, cam));
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + 3.45f, transform.position.z);
            canMove = false;

        }
        if (other.tag == "MinLeft")
        {

            StartCoroutine(Rotate(Vector3.up * -90, 0.6f, this.gameObject));
            StartCoroutine(Rotate(Vector3.up * -90, 0.8f, cam));
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + 3.45f, transform.position.z);
            canMove = false;

        }
        if (other.tag == "Finish")
        {
            if (GameManager._Instance.isFinish)
            {
                GameManager._Instance.isPassLevel = true;              
                GameManager._Instance.NextLevel();
            }
            else
            {
                GameManager._Instance.isFinish = true;             
            }
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Magma")
        {
            if (Time.time > nextBreakTime)
            {
                nextBreakTime = Time.time + magmaTime;
                if (transform.childCount > 1)
                {
                    GameObject DestroyCube = transform.GetChild(transform.childCount - 1).gameObject;
                    GameObject.Destroy(DestroyCube);
                }
                else
                {
                    GameManager._Instance.GameOver();
                }


            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            canMove = true;
            colWall = false;
        }
        if (other.tag == "Right")
        {
            Vector3 distance = cam.transform.position - transform.position;
            cam.GetComponent<sc_CameraMovement>().MoveCamera(distance, new Vector3(0, 90, 0));
            moveRight = true;
            moveForward = false;
            canMove = true;
        }
        if (other.tag == "Left")
        {
            Vector3 distance = cam.transform.position - transform.position;
            cam.GetComponent<sc_CameraMovement>().MoveCamera(distance, new Vector3(0, 0, 0));
            moveForward = true;
            moveRight = false;
            canMove = true;
        }
        if (other.tag == "MinLeft")
        {
            Vector3 distance = cam.transform.position - transform.position;
            cam.GetComponent<sc_CameraMovement>().MoveCamera(distance, new Vector3(0, 180, 0));
            moveForward = false;
            moveRight = true;
            canMove = true;
        }
        if (other.tag == "HighPlatform")
        {               
            // int childCount = Convert.ToInt32(other.name);
            
                
            if (GameManager._Instance.isFinish)
            {
                if (moveForward)
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (childCount * 0.60f), this.transform.position.z);

                }
                else if (moveRight)
                {
                    cam.transform.position = new Vector3(this.transform.position.x, cam.transform.position.y + (childCount * 0.60f), cam.transform.position.z);
                }

                boxCol.center=new Vector3(boxCol.center.x,boxCol.center.y+(0.61f*childCount),boxCol.center.z);  
            }           
            colWall = false;

        }
    }

    private void ColliderHeight()
    {
        boxCol.size = new Vector3(boxCol.size.x, 0, boxCol.size.z);
        
        float k = 0;
        for (int i = 1; i < transform.childCount; i++)
        {
            boxCol.size = new Vector3(boxCol.size.x, ((transform.GetChild(i).GetComponent<BoxCollider>().size.y * transform.GetChild(i).localScale.y) + boxCol.size.y), boxCol.size.z);
              k = i * 0.3f;
              boxCol.center = new Vector3(0,(k + 0.1f), 0);
           
        }      
        boxCol.size = new Vector3(boxCol.size.x, boxCol.size.y + (transform.GetChild(0).GetComponent<BoxCollider>().size.y * transform.GetChild(0).localScale.y) + 0.2f, boxCol.size.z);
    }

    IEnumerator Rotate(Vector3 byAngles, float inTime, GameObject gameObject)
    {
        var fromAngle = gameObject.transform.rotation;
        var toAngle = Quaternion.Euler(gameObject.transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            gameObject.transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    private void MoveLimit()
    {
        if (moveForward)
        {
            if (this.transform.position.x >= platform.transform.position.x + 1.24f)
            {
                this.transform.position = new Vector3(platform.transform.position.x + 1.23f, transform.position.y, transform.position.z);
            }
            else if (this.transform.position.x <= platform.transform.position.x - 1.26f)
            {
                this.transform.position = new Vector3(platform.transform.position.x - 1.23f, transform.position.y, transform.position.z);
            }
        }
        else if (moveRight)
        {
            if (this.transform.position.z >= platform.transform.position.z + 1.24f)
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y, platform.transform.position.z + 1.23f);
            }
            else if (this.transform.position.z <= platform.transform.position.z - 1.26f)
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y, platform.transform.position.z - 1.23f);
            }
        }
    }

    private void BreakPlayerCube(int a)
    {
        int j = 1;

        if (a >= transform.childCount - 1)
        {
            if (!GameManager._Instance.isFinish)
            {
                GameManager._Instance.GameOver();
            }
            else
            {
                GameManager._Instance.NextLevel();
            }

        }
        else
        {
            for (int i = 1; i <= a; i++)
            {
                transform.GetChild(j).parent = null;
            }
            for (int k = 1; k < transform.childCount; k++)
            {
                transform.GetChild(k).GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
            }
            colWall = true;
            ColliderHeight();
        }
    }
}