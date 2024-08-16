using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] GameObject Dice;
    public List<Transform> spawnPoints = new List<Transform>();
    public override void OnStartServer()
    {
        Dice.gameObject.SetActive(true);
        //Find all spawnPoints in the scene
        GameObject[] spawnPointsObjects = GameObject.FindGameObjectsWithTag("SpawnPoints");
        foreach (GameObject sp in spawnPointsObjects)
        {
            spawnPoints.Add(sp.transform);
        }
        base.OnStartServer();

    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

            int point = conn.connectionId % spawnPoints.Count;
            //Choose a spawn point
            Transform startPoint = spawnPoints[point];
            spawnPoints.Remove(spawnPoints[point]);
            // Instantiate the player at the chosen spawn point
            GameObject player = Instantiate(playerPrefab, startPoint.position, startPoint.rotation);
            //Changing player to it's connection id
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            //Add the player to the network
            NetworkServer.AddPlayerForConnection(conn, player);
            //Add Player to Actual Queue
            GameManager.instance.AddPlayers(player.GetComponent<PlayerMovement>());
        
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        GameManager.instance.players.Remove(GameManager.instance.players[conn.connectionId]);
        base.OnServerDisconnect(conn);
    }
}
