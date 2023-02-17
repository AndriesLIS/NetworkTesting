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

        CharacterFactory.PrefabBase = VisualPrefab;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        CharacterData message = new()
        {
            Color = new Color(
            Random.Range(0, 1f),
            Random.Range(0, 1f),
            Random.Range(0, 1f)),

            username = $"Player {numPlayers}",
            networkId = conn.connectionId
        };

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
        NetworkServer.AddPlayerForConnection(conn, player);

        var visuals = CharacterFactory.CreateCharacter(player, message);
        visuals.transform.SetParent(player.transform, false);

        var playerDataContainer = player.GetComponent<PlayerDataContainer>();

        NetworkServer.Spawn(visuals, playerDataContainer.connectionToClient);
        playerDataContainer.CrpcSetupClient(visuals, message);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        int i = 0;
        foreach (var item in NetworkServer.spawned.Values)
        {
            var tmp = item.gameObject.GetComponent<PlayerDataContainer>();

            if (tmp)
                tmp.UpdateUsername($"Player {i}");

            i++;
        }
    }
}
