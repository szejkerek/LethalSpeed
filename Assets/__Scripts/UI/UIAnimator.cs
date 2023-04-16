using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public Ease easeType;
    public Vector3 finalPosition;
    public float duration;
    public float delay;

    private RectTransform _objectToAnimate;
    private Vector2 _startingPosition;

    private void Awake()
    {
        _objectToAnimate = GetComponent<RectTransform>();
        _startingPosition = _objectToAnimate.position;
    }

    private void OnEnable()
    {
        _objectToAnimate.DOLocalMove(finalPosition, duration).SetEase(easeType).SetDelay(delay);
    }

    private void OnDisable()
    {
        _objectToAnimate.position = _startingPosition;
    }
}
