using UnityEngine;
using TMPro;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;
    public TextMeshPro questionText;
    public TextMeshPro[] choiceTexts;
    public Question[] questions;
    public Animator cardAnimator;

    private int currentQuestionIndex = 0;

    private void Awake()
    {
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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
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
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }

}
