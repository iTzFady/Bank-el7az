using Mirror;
using UnityEngine;

public class NumberDetector : MonoBehaviour
{
    RollingDice rollingDice;
    PlayerMovement playerMovement;
    private void Awake()
    {
        rollingDice = FindObjectOfType<RollingDice>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (rollingDice != null && rollingDice.isRolling)
        {
            if (rollingDice.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                if (!GameManager.instance.players[GameManager.instance.currentPlayerindex].isMoving)
                {
                    rollingDice.num = int.Parse(other.name);
                }
                rollingDice.isRolling = false;
            }
        }
    }
}
