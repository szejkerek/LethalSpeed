using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    Button _button;

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();

    }
    private void Start()
    {
        _button.onClick.AddListener(delegate () { SceneLoader.Instance.ReloadScene(); });
    }
}