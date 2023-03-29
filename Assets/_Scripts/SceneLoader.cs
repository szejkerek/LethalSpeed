using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SceneLoader : Singleton<SceneLoader>
{
    public Slider ProgressBar;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    // Update is called once per frame
    private void LoadGame()
    {
        
        //SceneManager.LoadSceneAsync(PersistentSingleton)
    }
}
