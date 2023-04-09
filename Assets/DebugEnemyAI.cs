using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

struct DebugEnemyAIText
{
    public Color titleColor;
    public string stateName;
    public string info;
}

public class DebugEnemyAI : MonoBehaviour
{
    Camera mainCamera;
    EnemyAIStateMachine enemyStates;

    [SerializeField] bool turnDebugInfo = true;
    [SerializeField] Canvas debugInfoCanvas;
    [SerializeField] TMP_Text StateText;
    [SerializeField] TMP_Text InfoText;

#if UNITY_EDITOR

    private void Awake()
    {
        enemyStates = GetComponent<EnemyAIStateMachine>();
        mainCamera = Helpers.Camera;
        debugInfoCanvas.gameObject.SetActive(turnDebugInfo);
    }

    private void Update()
    {
        debugInfoCanvas.transform.rotation = Quaternion.LookRotation(debugInfoCanvas.transform.position - mainCamera.transform.position);
        //UpdateDebugText(enemyStates.CurrentState);
    }

    void UpdateDebugText(DebugEnemyAIText debugText)
    {
        StateText.text = debugText.stateName;
        StateText.color = debugText.titleColor;
        InfoText.text = debugText.info;
    }

#endif
}
