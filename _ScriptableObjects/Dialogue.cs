using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;


/*
The Dialogue ScriptableObject is an asset that represents one dialogue from start to finish.

A Dialogue consists of:

    > An OnDialogueStart Function
    > An array of DialoguePage's
    > An OnDialogueFinish Function

A DialoguePage represents one "page" of a dialogue.

Each DialoguePage consists of:
    > A string of text
    > 0-4 DialogueCharacterContext's
    > An OnPageStart Function
    > An OnPageFinish Function
    > Font size
    > (TODO; optional) Text Effects (such as colored text, shaky text, wavy text etc...)

A DialogueCharacterContext is a small object that contains information about a DialogueCharacter and how it's currently positioned in the dialogue.

Each DialogueCharacterContext consists of:
    > A DialogueCharacter
    > A string denoting the DialogueCharacter's current sprite
    > A DialogueCharacter's current position as a percentage (0 = all the way to the left, 1.0 = all the way to the right)
    > A boolean denoting whether or not the DialogueCharacter is horizontally flipped
    > A boolean denoting whether or not the DialogueCharacter is currently active (fully colored) or inactive (greyed out)

A DialogueCharacter is a ScriptableObject that represents a speaker in a Dialogue.

Each DialogueCharacter consists of:
    > A Name
    > A Key-Value Pairing of Descriptions and Sprites (*)

        (*) An example of this would be having different sprites for Seychan's happy face and Seychan's sad face.

    -Jody 26/07/24
*/


/*
    Note: Dialogue and DialogueCharacter both inherit from ScriptableObject as they are the two classes that assets should be made out of.
    DialoguePage and DialogueCharacterContext will not require assets.

    DialoguePages and their associated DialogueCharacterContext's will be created as a part of the Dialogue creation process.

    Soon, I will try to create an Dialogue Creation Tool.
   
    -Jody 26/07/24
 */


[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public UnityEvent<Dialogue> OnDialogueStart;
    public UnityEvent<Dialogue> OnDialogueFinish;

    public List<DialoguePage> pages;
}

[System.Serializable]
public class DialoguePage
{
    public string content;
    public List<DialogueCharacterContext> characters = new List<DialogueCharacterContext>();

    public UnityEvent<DialoguePage> OnPageStart;
    public UnityEvent<DialoguePage> OnPageFinish;
    public int FontSize = 16;
}

[Serializable]
public class DialogueCharacterContext
{
    public DialogueCharacter character;
    public string currentSpriteName;
    public float position;
    public bool isFlipped;
    public bool isActive;
}

