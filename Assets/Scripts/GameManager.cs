using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    RollingDice dice;
    CameraManager cameraManager;
    PlayerMovement[] objects;
    public List<PlayerMovement> players;
    public int currentPlayerindex = 0;
    [SyncVar] public bool isSomeonePlaying;
    [SyncVar] public bool isPlayerBusy;
    [SyncVar] public bool isPlayerQuestioned;
    private MyNetworkManager myNetworkServer;
    private float time;
    private int frameCount;
    private float pollingTime = 1f;
    [SerializeField] Text fps;
    private void Awake()
    {
        dice = FindObjectOfType<RollingDice>();
        cameraManager = FindObjectOfType<CameraManager>();
        myNetworkServer = (MyNetworkManager)NetworkManager.singleton;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //Application.targetFrameRate = Screen.currentResolution.refreshRate;
        //AddPlayers();
        //StartPlayerTurn();
    }
    // Update is called once per frame
    void Update()
    {

        if (dice != null)
        {
            if (Input.GetMouseButtonDown(0) && !dice.isRolling && !GameManager.instance.isSomeonePlaying && !GameManager.instance.isPlayerBusy && !GameManager.instance.isPlayerQuestioned)
            {
                cmdRollingDice();
            }
        }
        if (!GameManager.instance.isSomeonePlaying && dice.num > 0)
        {
            cameraManager.switchToCamera((int)(CameraManager.CameraType.Player), players[currentPlayerindex]);
            players[currentPlayerindex].steps = dice.num;
            dice.ResetDice();
            players[currentPlayerindex].StartMoving();
            StartCoroutine(EndPlayerTurn());
        }
        if (!GameManager.instance.isSomeonePlaying && players[currentPlayerindex].steps < 0)
        {
            cameraManager.switchToCamera((int)(CameraManager.CameraType.Player), players[currentPlayerindex]);
            players[currentPlayerindex].StartMoving();
        }
        if (!GameManager.instance.isPlayerBusy && !GameManager.instance.isPlayerQuestioned && !GameManager.instance.isSomeonePlaying && (cameraManager.currentCameraIndex != (int)CameraManager.CameraType.Dice))
        {
            cameraManager.switchToCamera((int)CameraManager.CameraType.Dice, null);
        }
        // FramePerSecond();
    }
    public void AddPlayers(PlayerMovement currentPlayer)
    {
        //players.Clear();
        objects = FindObjectsOfType<PlayerMovement>();
        // foreach (PlayerMovement obj in objects)
        // {
        //     if (obj.tag == "Player")
        //     {
        //         players.Add(obj);
        //     }
        // }
        players.Add(currentPlayer);
    }
    void StartPlayerTurn()
    {
        players[currentPlayerindex].StartMoving();
    }
    IEnumerator EndPlayerTurn()
    {
        yield return new WaitUntil(() => !players[currentPlayerindex].isMoving && !isPlayerBusy);
        currentPlayerindex = (currentPlayerindex + 1) % players.Count;
        StartPlayerTurn();
    }
    [Command(requiresAuthority = false)]
    void cmdRollingDice()
    {
        cameraManager.switchToCamera((int)CameraManager.CameraType.Dice, null);
        dice.RollDice();
    }
    public void FramePerSecond()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            // Debug.Log(frameRate);
            // Debug.Log(Screen.currentResolution.refreshRate);
            fps.text = frameRate.ToString();
            time -= pollingTime;
            frameCount = 0;
        }
    }
}

