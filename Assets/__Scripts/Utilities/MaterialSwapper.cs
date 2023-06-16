using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] Material _materialToApply;

    bool _applyThisFrame = false;
    bool _isApplied = false;
    Renderer _renderer;
    readonly List<Material> materials = new List<Material>();

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        materials.AddRange(_renderer.sharedMaterials);
    }

    public void ApplyThisFrame()
    {
        _applyThisFrame = true;
    }

    public void ApplyMaterial()
    {
        if (_materialToApply != null)
        {
            materials.Add(_materialToApply);
            _renderer.materials = materials.ToArray();
            _isApplied = true;
        }
    }

    public void FreeMaterial()
    {
        if (_isApplied)
        {
            materials.Remove(_materialToApply);
            _renderer.materials = materials.ToArray();
            _isApplied = false;
        }
    }

    private void LateUpdate()
    {
        if(_applyThisFrame)
        {
            if(!_isApplied)
                ApplyMaterial();

            _applyThisFrame = false;
        }
        else
        {
            if(_isApplied)
                FreeMaterial();
        }
    }
}
