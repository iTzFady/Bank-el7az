using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public TextMeshPro questionText;
    public TextMeshPro[] choiceTexts;
    public Question[] questions;
    public Animator cardAnimator;

    private int currentQuestionIndex = 0;

    void Start()
    {
        DisplayQuestion(currentQuestionIndex);
    }

    void DisplayQuestion(int index)
    {
        Question question = questions[index];
        questionText.text = question.questionText;

        for (int i = 0; i < question.choices.Length; i++)
        {
            choiceTexts[i].text = question.choices[i];
        }

        // Trigger the animation to show the card
        cardAnimator.SetTrigger("Show");
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
