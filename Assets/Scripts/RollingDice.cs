using UnityEngine;
using Mirror;
[RequireComponent(typeof(Rigidbody))]
public class RollingDice : NetworkBehaviour
{
    Rigidbody diceBody;
    [SyncVar][SerializeField] private float maxRandomForceValue, rollingForce;
    [SyncVar] private float forceX, forceY, forceZ;
    [SyncVar] public int num;
    [SyncVar] public bool isRolling;
    NetworkIdentity networkIdentity;
    NetworkConnectionToClient connection;
    private void Awake()
    {
        Initialize();
    }

    [Command(requiresAuthority = false)]
    public void RollDice()
    {
        Debug.Log("TextDice");
        isRolling = true;
        diceBody.isKinematic = false;
        forceX = Random.Range(0, maxRandomForceValue);
        forceY = Random.Range(0, maxRandomForceValue);
        forceZ = Random.Range(0, maxRandomForceValue);
        diceBody.AddForce(Vector3.up * rollingForce);
        diceBody.AddTorque(forceX, forceY, forceZ);
    }
    private void Initialize()
    {
        diceBody = GetComponent<Rigidbody>();
        diceBody.isKinematic = true;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
    }
    private void ResetPosition()
    {
        diceBody.isKinematic = true;
        isRolling = false;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
        transform.position = new Vector3(20, 8, 30);
    }
    public void ResetDice()
    {
        num = 0;
        Invoke("ResetPosition", 1f);
    }
}
