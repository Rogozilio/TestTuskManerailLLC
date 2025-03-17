using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[Serializable]
public struct RouteItemToBackpack
{
    public ItemType itemType;
    public Transform point;
}

public class Backpack : MonoBehaviour
{
    [Inject] private UI _ui;
    [Inject] private Input _input;
    [Inject] private Raycaster _raycaster;
    [Inject] private WebRequestSystem _webRequestSystem;

    [SerializeField] private UnityEvent<Item> _onItemAdded;
    [SerializeField] private UnityEvent<Item> _onItemRemoved;

    [SerializeField] private List<RouteItemToBackpack> RouteItemToBackpack;

    private bool _isHitInBackpack;

    private void Awake()
    {
        _input.OnStartHoldLMB += FirstHitInBackpack;
        _input.OnHoldLMB += OpenBackpack;
        _input.OnCancelHoldLMB += CloseBackpack;
        
        _onItemAdded.AddListener((item) =>
            _webRequestSystem.SendInventoryEvent(item.Data.id.ToString(), "Add"));
        _onItemRemoved.AddListener((item) =>
            _webRequestSystem.SendInventoryEvent(item.Data.id.ToString(), "Remove"));
    }

    private void OnDestroy()
    {
        _input.OnStartHoldLMB -= FirstHitInBackpack;
        _input.OnHoldLMB -= OpenBackpack;
        _input.OnCancelHoldLMB -= CloseBackpack;
        
        _onItemAdded.RemoveListener((item) =>
            _webRequestSystem.SendInventoryEvent(item.Data.id.ToString(), "Add"));
        _onItemRemoved.RemoveListener((item) =>
            _webRequestSystem.SendInventoryEvent(item.Data.id.ToString(), "Remove"));
    }

    private void AddItem(Item item)
    {
        foreach (var route in RouteItemToBackpack)
        {
            if (route.itemType != item.Data.type) continue;

            item.IsInBackpack = true;
            item.MoveToPoint(route.point);
            _ui.AddItem(item);
            
            _onItemAdded?.Invoke(item);
        }
    }

    private void FirstHitInBackpack()
    {
        if (_raycaster.GetHitObject<Backpack>())
        {
            _isHitInBackpack = true;
        }
    }

    private void OpenBackpack()
    {
        if (_raycaster.GetHitObject<Backpack>() && _isHitInBackpack)
        {
            _ui.IsShowUI = true;
        }
    }

    private void CloseBackpack()
    {
        var itemUi = _raycaster.GetHitUI<ItemUI>();

        if (itemUi)
        {
            _onItemRemoved?.Invoke(itemUi.Item);
            itemUi?.RemoveItem();
        }
        
        _ui.IsShowUI = false;
        _isHitInBackpack = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Item item))
        {
            AddItem(item);
        }
    }
}