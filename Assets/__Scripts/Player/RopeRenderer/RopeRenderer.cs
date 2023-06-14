using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    private bool isActive = false;
    private Rope rope;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;

    //public GrapplingGun grapplingGun;
    [SerializeField] private int quality;
    [SerializeField] private float damper;
    [SerializeField] private float strength;
    [SerializeField] private float velocity;
    [SerializeField] private float waveCount;
    [SerializeField] private float waveHeight;
    [Space()]
    [SerializeField] private AnimationCurve affectCurve;

    public bool IsActive { get => isActive; set => isActive = value; }

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rope = new Rope();
        rope.Target = 0;
    }

    public void DrawRope(bool enabled, Vector3 start, Vector3 end)
    {
        if (!enabled)
        {
            currentGrapplePosition = start;
            rope.Reset();
            lr.positionCount = 0;  // Set position count to 0 when disabling the rope
            return;
        }

        if (lr.positionCount != quality + 1)
        {
            rope.Velocity = velocity;
            lr.positionCount = quality + 1;  // Set the correct position count when enabling the rope
        }

        rope.Damper = damper;
        rope.Strength = strength;
        rope.Update(Time.deltaTime);

        var grapplePoint = end;
        var gunTipPosition = start;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * rope.Value *
                         affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
