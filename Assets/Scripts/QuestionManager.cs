using UnityEngine;
using TMPro;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;
    private CameraController cameraController;
    public TextMeshPro questionText;
    public TextMeshPro[] choiceTexts;
    public Question[] questions;
    public Animator cardAnimator;
    private LayerMask layerMask;
    private int currentQuestionIndex = 0;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Card");
        cameraController = FindObjectOfType<CameraController>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach (var choice in choiceTexts)
        {
            var collider = choice.gameObject.AddComponent<BoxCollider>();
            var textRenderer = choice.GetComponent<Renderer>();
            collider.size = textRenderer.bounds.size;
        }
        currentQuestionIndex = Random.Range(0, questions.Count());
        Debug.Log(currentQuestionIndex);
        DisplayQuestion(currentQuestionIndex);
    }
    private void Update()
    {
        if (Input.touchCount > 0 && GameManager.instance.playerBeingQuestioned)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit , layerMask))
                {
                    for (int i = 0; i < choiceTexts.Length; i++)
                    {
                        if (hit.collider.gameObject == choiceTexts[i].gameObject)
                        {
                            CheckAnswer(i);
                        }
                    }
                }
            }
        }
    }

    void DisplayQuestion(int index)
    {
        Question question = questions[index];
        questionText.text = question.questionText;

        for (int i = 0; i < question.choices.Length; i++)
        {
            choiceTexts[i].text = question.choices[i];
        }

    }

    public void CheckAnswer(int choiceIndex)
    {
        if (choiceIndex == questions[currentQuestionIndex].correctChoiceIndex)
        {
            Debug.Log("Correct!");
            cameraController.FollowDice();
            Invoke("Delay" , 1f);
        }
        else
        {
            Debug.Log("Wrong!");
            cameraController.FollowDice();
            Invoke("Delay", 1f);
        }
        cardAnimator.SetTrigger("Hide");
    }

    void Delay() {
        GameManager.instance.playerBeingQuestioned = false;
    }
}
