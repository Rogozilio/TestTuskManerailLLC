using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Raycaster : MonoBehaviour
{
    [Inject] private UI _ui;
    [Inject] private Input _input;

    private RaycastHit[] _hits;
    private List<RaycastResult> _hitsUI;

    private void Awake()
    {
        _hitsUI = new List<RaycastResult>();
    }

    private void FixedUpdate()
    {
        Raycast();
        RaycastUI();
    }
    private void RaycastUI()
    {
        _hitsUI.Clear();
        var pointerEventData = new PointerEventData(_ui.EventSystem);
        pointerEventData.position = _input.GetMousePosition;
        
        _ui.Raycaster.Raycast(pointerEventData, _hitsUI);
    }
    private void Raycast()
    {
        var ray = Camera.main.ScreenPointToRay(_input.GetMousePosition);

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

    public bool TryGetPositionHit(out Vector3 position, string tag = "")
    {
        position = Vector3.zero;

        if (tag == string.Empty)
        {
            position = _hits[0].point;
            return true;
        }

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