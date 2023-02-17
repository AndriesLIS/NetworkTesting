using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;
using static Unity.Burst.Intrinsics.X86.Avx;

public static class CharacterFactory 
{
    public static GameObject PrefabBase;

    public static GameObject CreateCharacter(GameObject player, CharacterData data)
    {
        Debug.Log(PrefabBase);

        var tmp = Object.Instantiate(PrefabBase);

        var references = tmp.GetComponent<PlayerReferenceHolder>();
        references.Sphere.material.color = data.Color;
        references.usernameText.text = data.username;

        var animator = tmp.GetComponent<NetworkAnimator>();
        player.GetComponent<PlayerMovement>().animator = animator;

        tmp.transform.SetParent(player.transform, false);

        return tmp;
    }

    public static void UpdateCharacter(GameObject player, GameObject visuals, CharacterData data)
    {
        var references = visuals.GetComponent<PlayerReferenceHolder>();
        references.Sphere.material.color = data.Color;
        references.usernameText.text = data.username;

        var animator = visuals.GetComponent<NetworkAnimator>();
        player.GetComponent<PlayerMovement>().animator = animator;
    }
}
