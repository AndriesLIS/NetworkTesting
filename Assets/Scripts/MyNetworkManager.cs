using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager 
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject VisualPrefab;
    [SerializeField] private PlayerDataContainer myPlayer = default;

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

            username = $"Player {numPlayers}"
        };

        CreateCharacter(conn, message);
    }

    [Server]
    public void CreateCharacter(NetworkConnectionToClient conn, CharacterData message)
    {
        Transform startPos = GetStartPosition();
        PlayerDataContainer player = startPos != null
            ? Instantiate(myPlayer, startPos.position, startPos.rotation)
            : Instantiate(myPlayer);

        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        var visuals = CharacterFactory.CreateCharacter(player.gameObject, message);
        visuals.transform.SetParent(player.transform, false);

        NetworkServer.Spawn(visuals, player.connectionToClient);
        player.CrpcSetupClient(visuals, message);
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
