using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerDataContainer : NetworkBehaviour
{
    #region Vars
    [SyncVar(hook = nameof(HandleCharacterDataChange))]
    private CharacterData data;
    #endregion

    #region Server

    #endregion

    #region Client
    [Server]
    public void CrpcSetupClient(CharacterData message)
    {
        data = message;
    }

    public void HandleCharacterDataChange(CharacterData _, CharacterData newData)
    {
        Debug.Log($"{netId} | {newData}");
        data = newData;

        Debug.Log("1");
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