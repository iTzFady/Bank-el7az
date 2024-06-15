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
            if (Input.GetMouseButtonDown(0) && !dice.isRolling && !playerMovement.isMoving)
            {
                cameraController.FollowDice();
                dice.RollDice();
            }
        }
        if (!playerMovement.isMoving && dice.num > 0)
        {
            cameraController.FollowPlayer(players[currentPlayerindex].transform);
            players[currentPlayerindex].steps = dice.num;
            dice.ResetNum();
            players[currentPlayerindex].StartMoving();
            EndPlayerTurn();
        }
        Debug.LogError(players[currentPlayerindex].name + players[currentPlayerindex].steps);
        /*if (!playerMovement.isMoving&&playerMovement.steps < 0) {
            cameraController.FollowPlayer();
            players[currentPlayerindex].StartMoving();
        }*/

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
    public void EndPlayerTurn() {
        currentPlayerindex = (currentPlayerindex + 1) % players.Count;
        StartPlayerTurn();
    }
    
}

