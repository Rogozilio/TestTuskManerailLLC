using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class DragAndDropController : MonoBehaviour
{
    [Inject] private Input _input;
    [Inject] private Raycaster _raycaster;
    
    public string tagFloor;

    private Item _item;

    private void Awake()
    {
        _input.OnDownLMB += SetItem;
        _input.OnUpLMB += ClearItem;
    }

    private void OnDestroy()
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
            if (_raycaster.TryGetPositionHit(out var positionHit, tagFloor))
                _item.MoveToVelocity(positionHit);
            
            if(_item.IsInBackpack) ClearItem();
            
            await UniTask.WaitForFixedUpdate();
        }
    }
    
    private void ClearItem()
    {
        _item = null;
    }
    
    #if (UNITY_EDITOR)
    [CustomEditor(typeof(DragAndDropController))]
    public class DragAndDropControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DragAndDropController tagSelector = (DragAndDropController)target;
            
            var tags = UnityEditorInternal.InternalEditorUtility.tags;
            
            var selectedIndex = System.Array.IndexOf(tags, tagSelector.tagFloor);
            if (selectedIndex == -1) selectedIndex = 0;

            selectedIndex = EditorGUILayout.Popup("Tag for Floor", selectedIndex, tags);

            // Сохраняем выбранный тег
            tagSelector.tagFloor = tags[selectedIndex];
        }
    }
    #endif
}