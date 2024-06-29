using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    RollingDice dice;
    PlayerMovement playerMovement;
    CameraController cameraController;
    PlayerMovement[] objects;
    public List<PlayerMovement> players;
    public int currentPlayerindex = 0;
    public bool isSomeonePlaying;
    private float time;
    private int frameCount;
    private float pollingTime = 1f;
    private void Awake()
    {
        dice = FindObjectOfType<RollingDice>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraController = FindObjectOfType<CameraController>();
        if (instance == null)
        {
            instance = this;
        }
        else { 
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        cameraController.FollowDice();
        AddPlayers();
        StartPlayerTurn();
    }
    // Update is called once per frame
    void Update()
    {
        if (dice != null)
        {
            if (Input.GetMouseButtonDown(0) && !dice.isRolling && !GameManager.instance.isSomeonePlaying)
            {
                cameraController.FollowDice();
                dice.RollDice();
            }
        }
        if (!GameManager.instance.isSomeonePlaying && dice.num > 0)
        {
            cameraController.FollowPlayer(players[currentPlayerindex].transform);
            players[currentPlayerindex].steps = dice.num;
            dice.ResetDice();
            players[currentPlayerindex].StartMoving();
            StartCoroutine(EndPlayerTurn());
            //EndPlayerTurn();
        }
        if (!GameManager.instance.isSomeonePlaying && playerMovement.steps < 0) {
            cameraController.FollowPlayer(players[currentPlayerindex].transform);
            players[currentPlayerindex].StartMoving();
        }
        //FramePerSecond();

    }

    void AddPlayers() {
        players.Clear();
        objects = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement obj in objects)
        {
            if (obj.tag == "Player") {
                players.Add(obj);
            }
        }
    }
    void StartPlayerTurn() {
        players[currentPlayerindex].StartMoving();
    }
    IEnumerator EndPlayerTurn() {
        yield return new WaitUntil(() => !players[currentPlayerindex].isMoving);
        currentPlayerindex = (currentPlayerindex + 1) % players.Count;
        StartPlayerTurn();
    }
    public void FramePerSecond() {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime) {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            Debug.Log(frameRate);
            time -= pollingTime;
            frameCount = 0;
        }

    }
    
}

