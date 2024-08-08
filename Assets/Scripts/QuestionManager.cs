using UnityEngine;
using TMPro;
using System.Linq;
using RTLTMPro;
using Mirror;

public class QuestionManager : NetworkBehaviour
{
    public static QuestionManager instance;
    private CameraManager cameraManager;
    public TextMeshPro questionText;
    public TextMeshPro[] choiceTexts;
    public Question[] questions;
    public Animator cardAnimator;
    private LayerMask layerMask;
    private int currentQuestionIndex = 0;
    [SerializeField] Material greekMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] Material defaultMaterial;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("QuestionCard");
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
    void Start()
    {
        foreach (var choice in choiceTexts)
        {
            var collider = choice.gameObject.GetComponentInParent<BoxCollider>();
            var textRenderer = choice.GetComponent<Renderer>();
        }
    }
    private void Update()
    {
        if (Input.touchCount > 0 && GameManager.instance.isPlayerQuestioned)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, layerMask))
                {
                    for (int i = 0; i < choiceTexts.Length; i++)
                    {
                        if (hit.collider.gameObject.GetComponentInChildren<RTLTextMeshPro3D>().text == choiceTexts[i].text)
                        {
                            CheckAnswer(i);
                        }
                    }
                    cameraManager.switchToCamera((int)CameraManager.CameraType.Dice, null);
                }
            }
        }
    }

    public void DisplayQuestion()
    {
        currentQuestionIndex = Random.Range(0, questions.Count());
        Question question = questions[currentQuestionIndex];
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
            //Debug.Log("Correct!");
            choiceTexts[choiceIndex].transform.parent.gameObject.GetComponent<MeshRenderer>().material = greekMaterial;
            Invoke("Delay", 2f);
        }
        else
        {
            //Debug.Log("Wrong!");
            choiceTexts[choiceIndex].transform.parent.gameObject.GetComponent<MeshRenderer>().material = redMaterial;
            Invoke("Delay", 1f);
        }
        cardAnimator.SetTrigger("Hide");
    }

    void Delay()
    {
        GameManager.instance.isPlayerQuestioned = false;
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            choiceTexts[i].transform.parent.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }
}
