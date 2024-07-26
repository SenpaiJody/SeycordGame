using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/DialogueEffects")]
public class DialogueEffects : ScriptableObject
{
    public static void TestEffect(Dialogue d)
    {
        Debug.Log("WAHAHA TEST EFFECT!");

    }

    public static void TestEffect2(Dialogue d)
    {
        Debug.Log("WAHAHA TEST EFFECT!");

    }

    public void test()
    {

        Dialogue d = new Dialogue();
        d.OnDialogueStart.AddListener(TestEffect);
    }
}
