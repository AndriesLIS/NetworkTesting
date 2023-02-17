using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager 
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject VisualPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        var tmp = $"Player {numPlayers}";

        CharacterData message = new CharacterData {};

        message.Color = new Color(
            Random.Range(0, 1f),
            Random.Range(0, 1f),
            Random.Range(0, 1f));

        message.username = tmp;
        message.networkId = conn.connectionId;

        Debug.Log($"{tmp} Joined the Game");

        //THIS SPAWNS THE PLAYER
        OnCreateCharacter(conn, message);
    }

    [Server]
    public void OnCreateCharacter(NetworkConnectionToClient conn, CharacterData message)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

        var Visuals = CharacterFactory.CreateCharacter(VisualPrefab, message);
        Visuals.transform.SetParent(player.transform, false);

        NetworkServer.AddPlayerForConnection(conn, player);
        NetworkServer.Spawn(Visuals);
        
        var tmp = player.GetComponent<PlayerDataContainer>();
        tmp.CrpcSetupClient(message);
    }
}
