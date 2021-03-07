// Roman Baranov 07.03.2021
using UnityEngine;

public class GridTileHighlight : MonoBehaviour
{
    private MeshRenderer _meshRenderer = null;
    private Color _defaultColor;
    private Color _mousoverColor;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _defaultColor = _meshRenderer.material.color;
        _mousoverColor = Color.yellow;
    }

    void OnMouseEnter()
    {
        _meshRenderer.material.color = _mousoverColor;
    }
    void OnMouseExit()
    {
        _meshRenderer.material.color = _defaultColor;
    }
}
