using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAIWeapon : MonoBehaviour
{
    private void Awake()
    {
        SetUpRig();
    }

    private void SetUpRig()
    {
        Transform source = FindObjectOfType<Player>().PlayerCamera.EnemyAimTarget;
        if (source != null)
        {
            foreach (MultiAimConstraint component in GetComponentsInChildren<MultiAimConstraint>())
            {
                var data = component.data.sourceObjects;
                data.SetTransform(0, source);
                component.data.sourceObjects = data;
            }
            RigBuilder rigs = GetComponent<RigBuilder>();
            rigs.Build();
        }
        else
        {
            Debug.LogError("Couldn't find player");
        }
    }
}
