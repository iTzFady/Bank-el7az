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
    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
    }
    public void RollDice()
    {
        if (!isServer) return; // Only the server can roll the dice

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
        ResetPosition();
    }
    [ClientRpc]
    public void RpcRollDice()
    {
        if (isServer)
        {
            RollDice();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdRequestRollDice()
    {
        RpcRollDice(); // Request the server to roll the dice
    }
    [Command(requiresAuthority = false)]
    public void CmdRequestOwnershipChange(NetworkConnectionToClient newOwner)
    {
        if (!isServer) return;

        ServerChangeOwnership(newOwner);

        // Inform all clients about the ownership change
        RpcNotifyOwnershipChange(newOwner.connectionId);
    }

    [Server]
    private void ServerChangeOwnership(NetworkConnectionToClient newOwner)
    {
        NetworkIdentity netIdentity = GetComponent<NetworkIdentity>();
        netIdentity.RemoveClientAuthority();
        Debug.Log(newOwner);
        // Assign authority to the new owner
        netIdentity.AssignClientAuthority(newOwner);
    }

    [ClientRpc]
    private void RpcNotifyOwnershipChange(int newOwnerId)
    {
        // Log or handle the new owner on each client
        Debug.Log($"New owner assigned with connection ID: {newOwnerId}");
    }

}
