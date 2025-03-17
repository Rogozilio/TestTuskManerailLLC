using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DragAndDropController : MonoBehaviour
{
    [Inject] private Input _input;
    [Inject] private Raycaster _raycaster;

    private Item _item;

    private void OnEnable()
    {
        _input.OnDownLMB += SetItem;
        _input.OnUpLMB += ClearItem;
    }

    private void OnDisable()
    {
        _input.OnDownLMB -= SetItem;
        _input.OnUpLMB -= ClearItem;
    }

    private async void SetItem()
    {
        if (_item) return;

        _item = _raycaster.GetHitObject<Item>();

        await DragItem();
    }

    private async UniTask DragItem()
    {
        while (_item)
        {
            if (_raycaster.TryGetPositionHit(out var positionHit, "Floor"))
                _item.MoveTo(positionHit);
            
            if(_item.IsInBackpack) ClearItem();
            
            await UniTask.WaitForFixedUpdate();
        }
    }
    
    private void ClearItem()
    {
        _item = null;
    }
}