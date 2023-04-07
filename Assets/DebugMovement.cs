using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class DebugMovement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _movementDebugText;
    [SerializeField] private TextMeshProUGUI _keyBinds;

    private PlayerMovement _pm;
    void Awake()
    {
        _pm = GetComponent<PlayerMovement>();
        _keyBinds.enabled = true;

#if UNITY_EDITOR
        _movementDebugText.enabled = true;
#endif
    }

    void Start()
    {
        PopulateKeyBindsInfoText();
    }

    void Update()
    {
        PopulateDebugText();
    }

    void PopulateDebugText()
    {
        if (_movementDebugText is null)
        {
            return;
        }
        _movementDebugText.text = $"State: {_pm.CurrentMovementState.GetStateName()}\n";
        _movementDebugText.text += $"Velocity: {_pm.Velocity.magnitude}\n";
        _movementDebugText.text += $"Y velocity: {_pm.Velocity.y}\n";
        _movementDebugText.text += $"XZ velocity: {_pm.FlatVelocity.magnitude}\n";
    }

    void PopulateKeyBindsInfoText()
    {
        if (_keyBinds is null)
        {
            return;
        }
        _keyBinds.text =  $"Jump key: {_pm.JumpKey}\n";
        _keyBinds.text += $"Crouch key: {_pm.CrouchKey}\n";
        _keyBinds.text += $"Dash key: {_pm.DashKey}\n";
        _keyBinds.text += $"Hook key: {_pm.HookKey}\n";
    }
}