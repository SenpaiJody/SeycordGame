using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The Interactable class is a Component that all objects that can be interacted with will have.
 */

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("This is a piano. Plunk plunk plink");
        StartCoroutine(StretchAndSqueeze());
    }

    //simple visual effect, not optimal just to show it working
    IEnumerator StretchAndSqueeze()
    {
        Vector3 scale = transform.localScale;
        for (int i = 0; i < 10; i++)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.2f, transform.localScale.y + 0.4f, transform.localScale.z);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y - 0.4f, transform.localScale.z);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}
