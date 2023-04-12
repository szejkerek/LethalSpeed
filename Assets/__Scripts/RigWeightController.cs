using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rig))]
public class RigWeightController : MonoBehaviour
{
    [SerializeField] private float interpolationTime = 1f;

    private Rig rig;

    private void Awake()
    {
        rig = GetComponent<Rig>();
        TurnOffRig();
    }
    public void TurnOnRig(float duration = 0)
    {
        ChangeRigWeightValueInTime(1f, duration);
    }

    public void TurnOffRig(float duration = 0)
    {
        ChangeRigWeightValueInTime(0f, duration);
    }

    private void ChangeRigWeightValueInTime(float value, float interpolationTime)
    {
        float currentWeight = rig.weight;
        Tween weightTween = DOTween.To(() => currentWeight, x => currentWeight = x, value, interpolationTime)
            .SetUpdate(true)
            .OnUpdate(() => rig.weight = currentWeight);
    }
}
