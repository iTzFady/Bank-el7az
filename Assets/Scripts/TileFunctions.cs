using UnityEngine;

public class TileFunctions : MonoBehaviour
{
    enum tileStations
    {
        QUESTIONS,
        SKIPTURN,
        SCORECHANGE,
        POSITIONCHANGE,
        NOTHING,
        Start
    }

    [SerializeField] tileStations TilesStations = tileStations.NOTHING;
    CameraController cameraController;
    QuestionManager questionManager;


    private void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        questionManager = FindObjectOfType<QuestionManager>();
    }
    public void question()
    {
        GameManager.instance.playerBeingQuestioned = true;
        cameraController.ShowCard();
        Invoke("CardAnimation", 1f);
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

    public void CardAnimation()
    {
        questionManager.cardAnimator.SetTrigger("Show");
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
                        case tileStations.QUESTIONS:
                            question();
                            //Debug.LogError("question" + other.gameObject.name);
                            break;
                        case tileStations.SKIPTURN:
                            SkipTurn();
                            Debug.LogError("skip turn" + other.gameObject.name);
                            break;
                        case tileStations.SCORECHANGE:
                            scoreChange(GameManager.instance.players[currentPlayerIndex].playerScore);
                            Debug.LogError("score change" + other.gameObject.name);
                            break;
                        case tileStations.POSITIONCHANGE:
                            positionChange(GameManager.instance.players[currentPlayerIndex].steps);
                            Debug.LogError("position change" + other.gameObject.name);
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
}
