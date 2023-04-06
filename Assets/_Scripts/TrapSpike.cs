using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : Trap
{
    protected override void Start()
    {
        base.Start();
        HideSpikes();
    }

    protected override void ActivateTrap()
    {
        base.ActivateTrap();
        LaunchSpikes();
    }

    protected override void DeactivateTrap()
    {
        base.DeactivateTrap();
        HideSpikes();
    }

    private void LaunchSpikes()
    {
        transform.position = transform.position + new Vector3(0, 0.25f, 0);
    }

    private void HideSpikes()
    {
        transform.position = transform.position + new Vector3(0, -0.25f, 0);
    }
}
