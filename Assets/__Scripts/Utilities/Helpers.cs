using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if(_camera == null)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if(WaitDictionary.TryGetValue(time, out WaitForSeconds wait))
        {
            return wait;
        }
        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            Object.Destroy(child);
        }
    }

    public static T PickRandomElement<T>(this List<T> list)
    {
        if (list is null)
        {
            throw new System.ArgumentNullException(nameof(list));
        }

        int index = Random.Range(0,list.Count);
        return list[index];
    }

    public static void PlayRandomized(this List<Sound> list, AudioSource audioSource)
    {
        if (audioSource is null)
        {
            throw new System.ArgumentNullException(nameof(list));
        }

        list.PickRandomElement().PlayRandomized(audioSource);
    }
}
