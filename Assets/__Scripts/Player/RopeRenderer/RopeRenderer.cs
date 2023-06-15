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
        lr = GetComponent<LineRenderer>();
        rope = new Rope();
        rope.Target = 0;
        rope.Reset();
        if (lr.positionCount > 0)
            lr.positionCount = 0;
    }

    //Called after Update
    //void LateUpdate()
    //{
    //    DrawRope(true, Vector3.zero, new Vector3(20,20,20));
    //}

    void DrawRope(bool isGrappling, Vector3 start, Vector3 end)
    {
        //If not grappling, don't draw rope
        if (!isGrappling)
        {
            currentGrapplePosition = start;
            rope.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            rope.Velocity = velocity;
            lr.positionCount = quality + 1;
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
            var right = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.right;

            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * rope.Value *
                                     affectCurve.Evaluate(delta) +
                                     right * waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * rope.Value *
                                     affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
