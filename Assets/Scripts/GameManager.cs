using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] CirclePlatforms;

    [SerializeField]
    GameObject player;

    private int score;

    public bool isGameOver = false;

    public bool isStart = false;

    public bool isFinish = false;

    public bool isPassLevel = false;

    private static GameManager instance;

    public static GameManager _Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    void Start()
    {

    }


    void Update()
    {
        if (!isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isStart = true;
            }
        }
        CirclePlatformsTurn();
    }

    void CirclePlatformsTurn()
    {
        for (int i = 0; i < CirclePlatforms.Length; i++)
        {
            CirclePlatforms[i].transform.transform.Rotate(new Vector3(0, 0, 0.3f));
        }
    }

    public void Score()
    {
        score += 5;
        

    }
    public void GameOver()
    {
        player.transform.Translate(0, 0f, 0f);
        isGameOver = true;
        Debug.Log("GameOver");
    }
    public void NextLevel()
    {
        player.transform.Translate(0, 0f, 0f);
        isPassLevel = true;
        Debug.Log("NextLevel");

    }
}
