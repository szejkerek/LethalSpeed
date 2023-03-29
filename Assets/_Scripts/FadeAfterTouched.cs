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
    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isFading = true;
            fadeStartTime = Time.time;
            StartCoroutine(FadeOutAndDisappear());
        }
    }
    IEnumerator FadeOutAndDisappear()
    {
        yield return new WaitForSeconds(fadeTime);

        gameObject.SetActive(false);

        if (shouldReappear)
        {
            yield return new WaitForSeconds(reappearTime);

            gameObject.SetActive(true);
            isFading = false;
            fadeStartTime = 0.0f;
        }
      
    }

    void Update()
    {
        if (isFading)
        {
            /* if material supports transparency, make it less opaque over fading time
             
            // calculate how much time has passed since fading started
            float timeSinceFadeStarted = Time.time - fadeStartTime;

            // calculate the opacity of the object based on how much time has passed
            float opacity = 1.0f - Mathf.Clamp01(timeSinceFadeStarted / fadeTime);

            // set the opacity of the renderer
            Color currentColor = renderer.material.color;
            currentColor.a = opacity;
            renderer.material.color = currentColor;
            */
        }

    }

}
