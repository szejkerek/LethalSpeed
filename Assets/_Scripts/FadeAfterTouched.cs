using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAfterTouched : MonoBehaviour
{
    public float fadeTime = 1.0f; 
    public bool  shouldReappear = false;
    public float reappearTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndDisappear());
        }
    }

    IEnumerator FadeOutAndDisappear()
    {
        yield return Helpers.GetWait(fadeTime);
        gameObject.SetActive(false);

        if (shouldReappear)
        {
            yield return Helpers.GetWait(reappearTime);

            gameObject.SetActive(true);
        }   
    }
}
