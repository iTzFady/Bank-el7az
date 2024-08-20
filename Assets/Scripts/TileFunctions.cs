using Mirror;
using UnityEngine;

public class TileFunctions : NetworkBehaviour
{
    enum tileStations
    {
        Start,
        Questions,
        SkipTurn,
        ScoreChange,
        PositionChange,
        Nothing,
        Penalty
    }

    [SerializeField] tileStations TilesStations = tileStations.Nothing;
    //CameraController cameraController;
    CameraManager cameraManager;
    QuestionManager questionManager;
    PenaltyManager penaltyManager;


    private void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        penaltyManager = FindObjectOfType<PenaltyManager>();
    }
    public void question()
    {
        cameraManager.switchToCamera((int)CameraManager.CameraType.Question, null);
        Debug.Log((int)CameraManager.CameraType.Question);
        GameManager.instance.isPlayerQuestioned = true;
        questionManager.DisplayQuestion();
        Invoke("QuestionCardAnimation", 1f);
    }
    public void penalty()
    {
        GameManager.instance.isPlayerBusy = true;
        cameraManager.switchToCamera((int)CameraManager.CameraType.Penalty, null);
        Invoke("PenaltyCardAnimation", 1f);
    }


    public void SkipTurn()
    {
        GameManager.instance.currentPlayerindex++; // Move to the next player

        if (GameManager.instance.currentPlayerindex >= GameManager.instance.players.Count)
        {
            GameManager.instance.currentPlayerindex = 0; // Wrap around to the first player if the last player was reached
        }

        // Additional logic for skipping multiple turns, if needed
        Debug.Log("Player " + GameManager.instance.currentPlayerindex + " turn skipped.");
    }

    public int scoreChange(int score)
    {
        return GameManager.instance.players[GameManager.instance.currentPlayerindex].GetComponent<PlayerMovement>().playerScore += score;
    }

    public int positionChange(int position)
    {
        return GameManager.instance.players[GameManager.instance.currentPlayerindex].GetComponent<PlayerMovement>().steps += position;
    }

    private void OnTriggerEnter(Collider other)
    {
        int currentPlayerIndex = GameManager.instance.currentPlayerindex;
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (!player.isMoving)
        {
            if (TilesStations != tileStations.Start)
            {
                if (!player.activatedTiles.Contains(int.Parse(gameObject.name)))
                {
                    if (player.GetComponent<NetworkIdentity>().isOwned)
                    {
                        player.activatedTiles.Add(int.Parse(gameObject.name));
                    }
                    switch (TilesStations)
                    {
                        case tileStations.Questions:
                            question();
                            break;
                        case tileStations.SkipTurn:
                            SkipTurn();
                            Debug.LogError("skip turn" + other.gameObject.name);
                            break;
                        case tileStations.ScoreChange:
                            scoreChange(GameManager.instance.players[currentPlayerIndex].GetComponent<PlayerMovement>().playerScore);
                            Debug.LogError("score change" + other.gameObject.name);
                            break;
                        case tileStations.PositionChange:
                            positionChange(GameManager.instance.players[currentPlayerIndex].GetComponent<PlayerMovement>().steps);
                            Debug.LogError("position change" + other.gameObject.name);
                            break;
                        case tileStations.Penalty:
                            //PenaltyManager.instance.AssignPenalty(player);
                            penaltyManager.AssignPenalty(player);
                            penalty();
                            break;
                        default:
                            break;
                    }
                    player = null;
                }
                else
                {
                    //Debug.Log("This player has entered this tile before");

                }
            }
        }
    }
    public void QuestionCardAnimation()
    {
        QuestionManager.instance.cardAnimator.SetTrigger("Show");
    }
    public void PenaltyCardAnimation()
    {
        PenaltyManager.instance.cardAnimator.SetTrigger("Show");
    }
}
