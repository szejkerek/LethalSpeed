using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct DebugEnemyAIText
{
    public Color titleColor;
    public string stateName;
    public string info;
}

public class DebugEnemyAI : MonoBehaviour
{
    Camera mainCamera;
    StateMachineEnemyAI enemyStates;

    [SerializeField] bool turnDebugInfo = true;
    [SerializeField] GameObject backgroundWithText;
    [SerializeField] TMP_Text StateText;
    [SerializeField] TMP_Text InfoText;

    private void Awake()
    {
        enemyStates = GetComponentInParent<StateMachineEnemyAI>();
        mainCamera = Helpers.Camera;
        backgroundWithText.gameObject.SetActive(turnDebugInfo);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        UpdateDebugText(enemyStates.CurrentState.GetDebugText());
    }

    void UpdateDebugText(DebugEnemyAIText debugText)
    {
        StateText.text = debugText.stateName;
        StateText.color = debugText.titleColor;
        InfoText.text = debugText.info;
    }
}
