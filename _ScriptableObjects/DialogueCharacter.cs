using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
    public new string name;
    public Dictionary<string, Sprite> sprites;
}
