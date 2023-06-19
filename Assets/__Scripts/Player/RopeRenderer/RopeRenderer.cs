using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
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


    void Awake()
    {
        rope = new Rope();
        lr = GetComponent<LineRenderer>();     
        RestartRope();
    }

    public void RestartRope()
    {

        rope.Target = 0;
        rope.Reset();
        if (lr.positionCount > 0)
            lr.positionCount = 0;
    }

    public void DrawRope(bool isGrappling, Vector3 end)
    {
        if (lr.positionCount == 0)
        {
            currentGrapplePosition = transform.position;
            rope.Velocity = velocity;
            lr.positionCount = quality + 1;
        }

        rope.Damper = damper;
        rope.Strength = strength;
        rope.Update(Time.deltaTime);

        var grapplePoint = end;
        var gunTipPosition = transform.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var right = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.right;

            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * rope.Value *
                                     affectCurve.Evaluate(delta) +
                                     right * waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * rope.Value *
                                     affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
