using UnityEngine;

public class ChoiceInput : MonoBehaviour
{
    private QuestionManager questionManager;

    void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();
    }

    void OnMouseDown()
    {
        int index = int.Parse(gameObject.name.Replace("Choice", ""));
        questionManager.CheckAnswer(index);
    }
}
