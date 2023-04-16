using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Ease _easeType;
    [SerializeField] private Vector3 _finalPosition;
    [SerializeField] private float _duration;
    [SerializeField] private float _delay;

    private RectTransform _objectToAnimate;
    private Vector2 _startingPosition;

    private void Awake()
    {
        _objectToAnimate = GetComponent<RectTransform>();
        _startingPosition = _objectToAnimate.position;
    }

    private void OnEnable()
    {
        _objectToAnimate.DOLocalMove(_finalPosition, _duration).SetEase(_easeType).SetDelay(_delay);
    }

    private void OnDisable()
    {
        _objectToAnimate.position = _startingPosition;
    }
}
