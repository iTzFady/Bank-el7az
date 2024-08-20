using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomManager : NetworkRoomManager
{

    public override void OnStartHost()
    {
        base.OnStartHost();

    }
    public override void OnRoomServerPlayersReady()
    {
        //Automatically start the game when all players are ready
        if (allPlayersReady)
        {
            ServerChangeScene(GameplayScene);
        }
    }
    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
    }
    public override void OnRoomClientExit()
    {
        base.OnRoomClientExit();
    }
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
    }
    // #region  ServerSide
    // public override void OnServerDisconnect(NetworkConnectionToClient conn)
    // {
    //     GameManager.instance.players.Remove(GameManager.instance.players[conn.connectionId]);
    //     base.OnServerDisconnect(conn);
    // }
    // #endregion
}

