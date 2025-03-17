using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Raycaster : MonoBehaviour
{
    [Inject] private UI _ui;
    [Inject] private Input _input;

    private Camera _camera;
    private PointerEventData _pointerEventData;
    
    private RaycastHit[] _hits;
    private List<RaycastResult> _hitsUI;

    private void Awake()
    {
        _hitsUI = new List<RaycastResult>();
        _pointerEventData = new PointerEventData(_ui.EventSystem);
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Raycast();
        RaycastUI();
    }
    private void RaycastUI()
    {
        _hitsUI.Clear();
        
        _pointerEventData.position = _input.GetMousePosition;
        
        _ui.Raycaster.Raycast(_pointerEventData, _hitsUI);
    }
    private void Raycast()
    {
        var ray = _camera.ScreenPointToRay(_input.GetMousePosition);

        _hits = Physics.RaycastAll(ray);
    }

    public T GetHitUI<T>() where T : class
    {
        foreach (var hit in _hitsUI)
        {
            if (hit.gameObject.TryGetComponent(out T component))
                return component;
        }

        return null;
    }
    
    public T GetHitObject<T>() where T : class
    {
        foreach (var hit in _hits)
        {
            if (hit.transform.TryGetComponent(out T component))
                return component;
        }

        return null;
    }

    public bool TryGetPositionHit(out Vector3 position, string tag)
    {
        position = Vector3.zero;
        
        foreach (var hit in _hits)
        {
            if (hit.transform.CompareTag(tag))
            {
                position = hit.point;
                return true;
            }
        }
    
        return false;
    }
    
}