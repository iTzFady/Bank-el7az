using UnityEngine;
using TMPro;
using System.Linq;
using UnityEditor;
using Mirror;

public class PenaltyManager : NetworkBehaviour
{
    public static PenaltyManager instance;
    public TextMeshPro penaltyDes;
    public TextMeshPro penaltyText;
    public Penalty[] penalties;
    public Animator cardAnimator;
    private LayerMask layerMask;
    private int currentPenaityIndex = 0;
    private CameraManager cameraManager;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("PenaltyCard");
        cameraManager = FindObjectOfType<CameraManager>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Input.touchCount > 0 && GameManager.instance.isPlayerBusy)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, layerMask))
                {
                    GameManager.instance.isPlayerBusy = false;
                    cameraManager.switchToCamera((int)CameraManager.CameraType.Dice, null);
                    cardAnimator.SetTrigger("Hide");
                }
            }
        }
    }
    public void DisplayQuestion()
    {
        penaltyDes.text = penalties[currentPenaityIndex].penaltyDescription;
        penaltyText.text = penalties[currentPenaityIndex].penaltyText;
    }
    public void AssignPenalty(PlayerMovement player)
    {
        currentPenaityIndex = Random.Range(0, penalties.Count());
        player.currentPenalty = penalties[currentPenaityIndex];
        DisplayQuestion();
        ExecutePenalty(player);

    }
    void ExecutePenalty(PlayerMovement player)
    {
        player.currentPenalty.playerLastScore = player.playerScore;
        switch (player.currentPenalty.penaltyType)
        {
            case Penalty.PenaltyType.GoBackToStart:
                player.currentPenalty.playerLastLocation = player.playerPostion;
                player.steps = -player.playerPostion;
                break;
            case Penalty.PenaltyType.SkipTurns:
                break;
            case Penalty.PenaltyType.LoseStar:
                player.playerScore -= player.currentPenalty.penaltyValue;
                break;
            case Penalty.PenaltyType.GainStars:
                player.playerScore += player.currentPenalty.penaltyValue;
                break;
            case Penalty.PenaltyType.QuestionToFriend:
                break;
            case Penalty.PenaltyType.MoveBackSpaces:
                player.currentPenalty.playerLastLocation = player.playerPostion;
                player.steps -= player.currentPenalty.penaltyValue;
                break;
            case Penalty.PenaltyType.MoveForwardSpaces:
                player.currentPenalty.playerLastLocation = player.playerPostion;
                player.steps += player.currentPenalty.penaltyValue;
                break;
            case Penalty.PenaltyType.GoToJail:
                player.currentPenalty.playerLastLocation = player.playerPostion;
                player.steps = (19 - player.playerPostion);
                break;
            case Penalty.PenaltyType.JailBreakCard:
                player.steps = player.playerPostion - player.currentPenalty.playerLastLocation;
                break;
            case Penalty.PenaltyType.CancelPenaltyCard:
                player.inJail = false;
                player.loseTurn = false;
                if (player.playerPostion != player.currentPenalty.playerLastLocation)
                {
                    player.steps = (player.currentPenalty.playerLastLocation = player.playerPostion);
                }
                if (player.playerScore != player.currentPenalty.playerLastScore)
                {
                    player.playerScore -= player.currentPenalty.playerLastScore;
                }
                break;
            case Penalty.PenaltyType.JudgeFriends:
                break;
            case Penalty.PenaltyType.StayPut:
                break;
        }

    }
    void Delay()
    {
        GameManager.instance.isPlayerBusy = false;
    }
}
