using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    RollingDice dice;
    CameraController cameraController;
    PlayerMovement[] objects;
    public List<PlayerMovement> players;
    public int currentPlayerindex = 0;
    public bool isSomeonePlaying;
    public bool isPlayerBusy;
    public bool isPlayerQuestioned;
    /*private float time;
    private int frameCount;
    private float pollingTime = 1f;
    [SerializeField] Text fps;*/
    private void Awake()
    {
        dice = FindObjectOfType<RollingDice>();
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
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        //Application.targetFrameRate = 60;
        cameraController.FollowDice();
        AddPlayers();
        StartPlayerTurn();
    }
    // Update is called once per frame
    void Update()
    {
        if (dice != null)
        {
            if (Input.GetMouseButtonDown(0) && !dice.isRolling && !GameManager.instance.isSomeonePlaying && !GameManager.instance.isPlayerBusy && !GameManager.instance.isPlayerQuestioned)
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
        }
        if (!GameManager.instance.isSomeonePlaying && players[currentPlayerindex].steps < 0) {
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
        yield return new WaitUntil(() => !players[currentPlayerindex].isMoving && !isPlayerBusy);
        currentPlayerindex = (currentPlayerindex + 1) % players.Count;
        StartPlayerTurn();
    }
/*    public void FramePerSecond() {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime) {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            Debug.Log(frameRate);
            Debug.Log(Screen.currentResolution.refreshRate);
            fps.text = frameRate.ToString();
            time -= pollingTime;
            frameCount = 0;
        }

    }*/

    
}

