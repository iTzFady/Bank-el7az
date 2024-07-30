using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;

public class PenaltyManager : MonoBehaviour
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
                    //GameManager.instance.isPlayerBusy = false;
                    cardAnimator.SetTrigger("Hide");
                    cameraManager.switchToCamera((int)CameraManager.CameraType.Dice, null);
                    Invoke("Delay", 7f);
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
        StartCoroutine(ExecutePenalty(player));

    }
    IEnumerator ExecutePenalty(PlayerMovement player)
    {
        yield return new WaitUntil(() => !GameManager.instance.isPlayerBusy && cardAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 2);
        switch (player.currentPenalty.penaltyType)
        {
            case Penalty.PenaltyType.GoBackToStart:
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
                player.steps -= player.currentPenalty.penaltyValue;
                break;
            case Penalty.PenaltyType.MoveForwardSpaces:
                player.steps += player.currentPenalty.penaltyValue;
                break;
            case Penalty.PenaltyType.GoToJail:
                break;
            case Penalty.PenaltyType.JailBreakCard:
                break;
            case Penalty.PenaltyType.CancelPenaltyCard:
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
