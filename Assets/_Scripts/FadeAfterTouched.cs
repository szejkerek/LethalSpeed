using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAfterTouched : MonoBehaviour
{
    public float fadeTime = 1.0f; 
    public bool  shouldReappear = false;
    public float reappearTime = 2.0f;
        
    private bool isFading = false; 
    private float fadeStartTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isFading = true;
            fadeStartTime = Time.time;
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
            isFading = false;
            fadeStartTime = 0.0f;
        }   
    }
}
