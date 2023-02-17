using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterFactory 
{
    public static GameObject CreateCharacter(GameObject prefabBase, CharacterData data)
    {
        var tmp = Object.Instantiate(prefabBase);

        var references = tmp.GetComponent<PlayerReferenceHolder>();
        references.Sphere.material.color = data.Color;
        references.usernameText.text = data.username;

        return tmp;
    }
}
