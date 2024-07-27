using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 Probably one of the messier pieces here. Most of the methods in this class were made to integrate with the DialogueEditor.
 */

[CreateAssetMenu(menuName = "Dialogue/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
    public new string name;
    public List<DialogueSprite> sprites;

    public Sprite GetSprite(string name)
    {
        foreach (DialogueSprite d in sprites)
        {
            if (d.name == name)
                return d.sprite;
        }
        return null;
    }
    public Sprite GetSpriteAt(int index)
    {
        return sprites[index].sprite;
    }

    public string GetSpriteNameAt(int index)
    {
        return sprites[index].name;
    }
    public int GetSpriteIndex(string name)
    {
        for (int i = 0; i< sprites.Count; i++)
        {
            if (name == sprites[i].name)
                return i;
        }
        return 0;
    }
    public string[] GetSpriteNames()
    {
        string[] names = new string[sprites.Count];
        for (int i = 0; i < sprites.Count; i++)
        {
            names[i] = sprites[i].name;
        }
        return names;
    }

    public void RemoveSprite(string name)
    {
        for (int i = 0; i<sprites.Count; i++)
        {
            if (sprites[i].name == name)
            {
                sprites.RemoveAt(i);
            }
        }
    }


    //A key-value pairing of name and sprite, this is done because Dictionaries in Unity cannot be serialized (i.e., displayed in the Editor)
    [Serializable]
    public class DialogueSprite
    {
        public string name;
        public Sprite sprite;
    }
}

