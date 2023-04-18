using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    Button _button;
    [field: SerializeField] public SceneIndexes sceneToUnload { private set; get; }
    [field: SerializeField] public SceneIndexes sceneToLoad { private set; get; }

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
    
    }
    private void Start()
    {
       _button.onClick.AddListener(delegate () { SceneLoader.Instance.LoadGame(sceneToUnload, sceneToLoad);});
    }
}
