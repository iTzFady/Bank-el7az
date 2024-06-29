using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFunctions : MonoBehaviour
{
    enum tileStations
    {
        QUESTIONS,
        SKIPTURN,
        SCORECHANGE,
        POSITIONCHANGE,
        NOTHING
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
        Invoke("CardAnimation" , 3f);
        //QuestionManager.instance.CardAnimation();
        
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
        if (!GameManager.instance.players[GameManager.instance.currentPlayerindex].isMoving)
        {
            switch (TilesStations)
            {
                case tileStations.QUESTIONS:
                    question(); 
                    Debug.LogError("question" + other.gameObject.name);
                    break;
                case tileStations.SKIPTURN:
                    SkipTurn(); 
                    Debug.LogError("skip turn" + other.gameObject.name); 
                    break;
                case tileStations.SCORECHANGE:
                    scoreChange(GameManager.instance.players[GameManager.instance.currentPlayerindex].playerScore); 
                    Debug.LogError("score change" + other.gameObject.name); 
                    break;
                case tileStations.POSITIONCHANGE:
                    positionChange(GameManager.instance.players[GameManager.instance.currentPlayerindex].steps); 
                    Debug.LogError("position change" + other.gameObject.name); 
                    break;
                default:
                    break;
            }
        }
    }
}