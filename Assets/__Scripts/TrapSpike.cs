using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : Trap
{
    public Ease Ease;

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
        transform.DOMoveY(transform.position.y + 0.25f, 0.1f);
    }

    private void HideSpikes()
    {
        transform.DOMoveY(transform.position.y - 0.25f, 0.3f).SetEase(Ease);
    }
}
