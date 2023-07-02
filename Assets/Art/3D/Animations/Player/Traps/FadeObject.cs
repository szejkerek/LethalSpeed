using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeObject : MonoBehaviour
{
    [SerializeField] bool  _shouldReappear = false;
    [SerializeField] float _disappearedHeight = -1;
    [SerializeField] float _appearedHeight = 10f;
    [SerializeField] float _playerTooCloseDistance = 5f;
    [Header("Timers")]
    [SerializeField] float _fadeOutTime; 
    [SerializeField] float _fadeInTime;
    [SerializeField] float _waitBeforeFadeOut;
    [SerializeField] float _waitBeforeFadeIn;
    [SerializeField] float _stableStructureTime;
    [SerializeField] float _ColliderRemoveTime;

    private Material _material;
    private Collider _collider;
    private float _objectHeight;
    private Transform _player;

    private float _lastFadeOut = 0;
    private float _timeBeforeFadeOut = 0;
    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        _player = FindObjectOfType<Player>().transform;
        _collider = GetComponent<Collider>();
        _material = renderer.material;
        _objectHeight = renderer.bounds.size.y;
        _objectHeight += _appearedHeight;

        SetHeight(_objectHeight);
       
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time - _lastFadeOut >= _timeBeforeFadeOut)
            {
                _timeBeforeFadeOut = _fadeOutTime + _fadeInTime + _waitBeforeFadeIn + _stableStructureTime + _waitBeforeFadeOut;
                StartCoroutine(FadeOut());
            }
        }
    }


    private void OnValidate()
    {
        _ColliderRemoveTime = _ColliderRemoveTime > _fadeOutTime ? _fadeOutTime : _ColliderRemoveTime; //Clamp 
    }

    IEnumerator FadeOut()
    {
        _lastFadeOut = Time.time;
        float elapsedTime = 0;
        float startHeight = _objectHeight;
        float targetHeight = _disappearedHeight;

        bool colliderDisabled = false;
        yield return new WaitForSeconds(_stableStructureTime);
        while (elapsedTime < _fadeOutTime)
        {
            if (!colliderDisabled && elapsedTime > _ColliderRemoveTime)
            {
                _collider.enabled = false;
                colliderDisabled = true;
            }

            float height = Mathf.Lerp(startHeight, targetHeight, elapsedTime / _fadeOutTime);
            SetHeight(height);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetHeight(targetHeight);

        if (_shouldReappear)
        {
            yield return new WaitForSeconds(_waitBeforeFadeIn);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        float startHeight = _disappearedHeight;
        float targetHeight = _objectHeight;

        while (Vector3.Distance(_player.position, transform.position) <= _playerTooCloseDistance)
        {
            yield return null;
        }
        _collider.enabled = true;

        while (elapsedTime < _fadeInTime)
        {
            float height = Mathf.Lerp(startHeight, targetHeight, elapsedTime / _fadeInTime);
            SetHeight(height);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetHeight(targetHeight);
    }

    private void SetHeight(float height)
    {
        _material.SetFloat("_CutoffHeight", height);
    }
}
