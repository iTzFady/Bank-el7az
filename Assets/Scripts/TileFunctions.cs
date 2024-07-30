using UnityEngine;

public class TileFunctions : MonoBehaviour
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


    private void Awake()
    {
        //cameraController = FindObjectOfType<CameraController>();
        questionManager = FindObjectOfType<QuestionManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }
    public void question()
    {
        GameManager.instance.isPlayerQuestioned = true;
        questionManager.DisplayQuestion();
        cameraManager.switchToCamera((int)CameraManager.CameraType.Question, null);
        Invoke("QuestionCardAnimation", 1f);
    }
    public void penalty()
    {
        GameManager.instance.isPlayerBusy = true;
        questionManager.DisplayQuestion();
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
        return GameManager.instance.players[GameManager.instance.currentPlayerindex].playerScore += score;
    }

    public int positionChange(int position)
    {
        return GameManager.instance.players[GameManager.instance.currentPlayerindex].steps += position;
    }

    private void OnTriggerStay(Collider other)
    {
        int currentPlayerIndex = GameManager.instance.currentPlayerindex;
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (!player.isMoving)
        {
            if (TilesStations != tileStations.Start)
            {
                if (!player.activatedTiles.Contains(int.Parse(gameObject.name)))
                {
                    player.activatedTiles.Add(int.Parse(gameObject.name));
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
                            scoreChange(GameManager.instance.players[currentPlayerIndex].playerScore);
                            Debug.LogError("score change" + other.gameObject.name);
                            break;
                        case tileStations.PositionChange:
                            positionChange(GameManager.instance.players[currentPlayerIndex].steps);
                            Debug.LogError("position change" + other.gameObject.name);
                            break;
                        case tileStations.Penalty:
                            PenaltyManager.instance.AssignPenalty(player);
                            penalty();
                            break;
                        default:
                            break;
                    }
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
