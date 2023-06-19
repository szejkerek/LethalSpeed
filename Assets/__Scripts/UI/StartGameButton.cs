using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    Button _button;
    [field: SerializeField] public SceneBuildIndexes sceneToLoadBuildIndex { private set; get; }

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
    
    }
    private void Start()
    {
       _button.onClick.AddListener(() =>  SceneLoader.Instance.LoadScene(sceneToLoadBuildIndex));
    }
}
