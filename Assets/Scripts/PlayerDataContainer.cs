using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mirror;

public class PlayerDataContainer : NetworkBehaviour
{
    #region Vars
    [SyncVar(hook = nameof(HandleCharacterDataChange))]
    private CharacterData data;
    [SyncVar(hook = nameof(HandleVisualsChange))]
    private GameObject visuals;
    [SyncVar(hook = nameof(HandleVisualsIDChange))]
    private uint visualsNetworkId;
    #endregion


    #region Server
    [Server]
    public void UpdateUsername(string message)
    {
        var tmp = data;

        tmp.username = message;

        data = tmp;
    }

    [Server]
    public void CrpcSetupClient(GameObject visuals, CharacterData message)
    {
        this.visuals = visuals;
        visualsNetworkId = visuals.GetComponent<NetworkIdentity>().netId;
        data = message;
    }
    #endregion


    #region Client
    public void HandleCharacterDataChange(CharacterData _, CharacterData newData)
    {
        data = newData;
        StartCoroutine(UpdateVisualsForAllClients());
    }
    public void HandleVisualsChange(GameObject _, GameObject newData)
    {
        visuals = newData;
    }
    public void HandleVisualsIDChange(uint _, uint newData)
    {
        visualsNetworkId = newData;
    }

    public IEnumerator UpdateVisualsForAllClients()
    {
        while (visuals == null)
        {
            yield return null;

            if (NetworkClient.spawned.TryGetValue(visualsNetworkId, out NetworkIdentity identity))
                visuals = identity.gameObject;
        }

        CharacterFactory.UpdateCharacter(gameObject, visuals, data);
        visuals.transform.SetParent(transform, false);
    }
    #endregion
}

public struct CharacterData
{
    public string username;
    public int networkId;

    public Color Color;

    public int BodyPreset;
    public int EyeColorOne;
    public int EyeColorTwo;
    public int HairColor;
    public int HairType;
    public int SkinColor;
    public int ShirtColor;
    public int PantsColor;

    public CharacterData(
        string username,
        int networkId,
        Color color,
        int bodyPreset,
        int eyeColorOne,
        int eyeColorTwo,
        int hairColor,
        int hairType,
        int skinColor,
        int shirtColor,
        int pantsColor)
    {
        this.username = username;
        this.networkId = networkId;
        Color = color;
        BodyPreset = bodyPreset;
        EyeColorOne = eyeColorOne;
        EyeColorTwo = eyeColorTwo;
        HairColor = hairColor;
        HairType = hairType;
        SkinColor = skinColor;
        ShirtColor = shirtColor;
        PantsColor = pantsColor;
    }
}
