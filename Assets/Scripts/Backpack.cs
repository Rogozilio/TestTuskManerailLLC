using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[Serializable]
public struct RouteItemToBackpack
{
    public Item item;
    public Transform transform;
}

public class Backpack : MonoBehaviour
{
    [Inject] private UI _ui;
    [Inject] private Input _input;
    [Inject] private Raycaster _raycaster;
    
    private RequstSystem _requstSystem;

    public UnityEvent<Item> OnItemAdded;
    public UnityEvent<Item> OnItemRemoved;

    public List<RouteItemToBackpack> RouteItemToBackpack;

    private void Awake()
    {
        _requstSystem = new RequstSystem();
        
        _input.OnHoldLMB += OpenBackpack;
        _input.OnCancelHoldLMB += CloseBackpack;
        
        OnItemAdded.AddListener((item) =>
            _requstSystem.SendInventoryEvent(item.Data.id.ToString(), "Add"));
        OnItemRemoved.AddListener((item) =>
            _requstSystem.SendInventoryEvent(item.Data.id.ToString(), "Remove"));
    }

    private void OnDestroy()
    {
        _input.OnHoldLMB -= OpenBackpack;
        _input.OnCancelHoldLMB -= CloseBackpack;
        
        OnItemAdded.RemoveListener((item) =>
            _requstSystem.SendInventoryEvent(item.Data.id.ToString(), "Add"));
        OnItemRemoved.RemoveListener((item) =>
            _requstSystem.SendInventoryEvent(item.Data.id.ToString(), "Remove"));
    }

    private void AddItem(Item item)
    {
        foreach (var route in RouteItemToBackpack)
        {
            if (route.item != item) continue;

            item.IsInBackpack = true;
            item.MoveToPoint(route.transform);
            _ui.AddItem(item);
            
            OnItemAdded?.Invoke(item);
        }
    }

    private void OpenBackpack()
    {
        if (_raycaster.GetHitObject<Backpack>())
        {
            _ui.IsShowUI = true;
        }
    }

    private void CloseBackpack()
    {
        var itemUi = _raycaster.GetHitUI<ItemUI>();

        if (itemUi)
        {
            OnItemRemoved?.Invoke(itemUi.Item);
            itemUi?.RemoveItem();
        }
        
        _ui.IsShowUI = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Item item))
        {
            AddItem(item);
        }
    }
}