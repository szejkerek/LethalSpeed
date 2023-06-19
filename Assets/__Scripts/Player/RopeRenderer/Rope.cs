using UnityEngine;

public class Rope
{
    private float _strength;
    private float _damper;
    private float _target;
    private float _velocity;
    private float _value;

    public float Strength { get => _strength; set => _strength = value; }
    public float Damper { get => _damper; set => _damper = value; }
    public float Target { get => _target; set => _target = value; }
    public float Velocity { get => _velocity; set => _velocity = value; }
    public float Value => _value;

    public void Update(float deltaTime)
    {
        var direction = _target - _value >= 0 ? 1f : -1f;
        var force = Mathf.Abs(_target - _value) * _strength;
        _velocity += (force * direction - _velocity * _damper) * deltaTime;
        _value += _velocity * deltaTime;
    }

    public void Reset()
    {
        _velocity = 0f;
        _value = 0f;
    }

}
