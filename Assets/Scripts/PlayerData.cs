using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData 
{
    public CharacterData CharacterData { get; private set; }

    public string WalletAddress { get; private set; }
    public string Username { get; private set; }

    public PlayerData(
        CharacterData characterBlockchainData,
        string walletAddress,
        string username)
    {
        CharacterData = characterBlockchainData;

        WalletAddress = walletAddress;
        Username = username;
    }
}