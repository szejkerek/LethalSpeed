using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rig))]
public class RigWeightController : MonoBehaviour
{
    [SerializeField] private float interpolationTime = 2f;

    private Rig rig;

    private void Awake()
    {
        rig = GetComponent<Rig>();
        ChangeRigWeightValueInTime(0f, 0f);
    }
    public void TurnOnRig()
    {
        ChangeRigWeightValueInTime(1f, interpolationTime);
    }

    public void TurnOffRig()
    {
        ChangeRigWeightValueInTime(0f, interpolationTime);
    }

    private void ChangeRigWeightValueInTime(float value, float interpolationTime)
    {
        float currentWeight = rig.weight;
        Tween weightTween = DOTween.To(() => currentWeight, x => currentWeight = x, value, interpolationTime)
            .SetUpdate(true)
            .OnUpdate(() => rig.weight = currentWeight);
    }
}
