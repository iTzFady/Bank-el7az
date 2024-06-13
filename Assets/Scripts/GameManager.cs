using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    RollingDice dice;
    PlayerMovement playerMovement;
    CameraController cameraController;
    public List<PlayerMovement> players;
    private int currentPlayerindex = 0;
    private void Awake()
    {
        dice = FindObjectOfType<RollingDice>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraController = FindObjectOfType<CameraController>();
    }
    private void Start()
    {
        //cameraController.FollowDice();
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
                //test.ChangeTarget(nextTarget);
            }
        }
        if (!playerMovement.isMoving && dice.num > 0)
        {
            cameraController.FollowPlayer();
            playerMovement.steps = dice.num;
            dice.ResetNum();
            playerMovement.StartMoving();
        }
        if (!playerMovement.isMoving&&playerMovement.steps < 0) {
            cameraController.FollowPlayer();
            playerMovement.StartMoving();
        }

    }

    void StartPlayerTurn() {
        players[currentPlayerindex].StartMoving();
    }
    public void EndPlayerTurn() {
        currentPlayerindex = (currentPlayerindex + 1) % players.Count;
        Debug.LogError(currentPlayerindex);
        StartPlayerTurn();
    }
}

