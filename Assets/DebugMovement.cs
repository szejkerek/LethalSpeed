using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class DebugMovement : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private TextMeshProUGUI _debugText;

    private PlayerMovement _pm;

    public bool displayDebugInfo = true;

    void Awake()
    {
        _pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(_debugText is null || !displayDebugInfo)
        {
            return;
        }

        _debugText.text = $"State: {_pm.CurrentMovementState.GetStateName()}\n";
        _debugText.text += $"Velocity: {_pm.Velocity.magnitude}\n";
        _debugText.text += $"Y velocity: {_pm.Velocity.y}\n";
        _debugText.text += $"XZ velocity: {_pm.FlatVelocity.magnitude}\n";
    }
#endif
}