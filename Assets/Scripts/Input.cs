using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    [SerializeField] public InputActionAsset _inputActionAsset;

    public Vector2 GetMousePosition => _inputActionAsset.FindAction("MousePosition").ReadValue<Vector2>();

    public event Action OnDownLMB;
    public event Action OnUpLMB;
    public event Action OnStartHoldLMB;
    public event Action OnHoldLMB;
    public event Action OnCancelHoldLMB;

    private void Awake()
    {
        _inputActionAsset.Enable();

        _inputActionAsset.FindAction("LMB").performed += (ctx) => { OnDownLMB(); };
        _inputActionAsset.FindAction("LMB").canceled += (ctx) => { OnUpLMB(); };
        
        _inputActionAsset.FindAction("LMBHold").started += (ctx) => { OnStartHoldLMB(); };
        _inputActionAsset.FindAction("LMBHold").performed += (ctx) => { OnHoldLMB(); };
        _inputActionAsset.FindAction("LMBHold").canceled += (ctx) => { OnCancelHoldLMB(); };
    }

    private void OnDestroy()
    {
        _inputActionAsset.FindAction("LMB").performed -= (ctx) => { OnDownLMB(); };
        _inputActionAsset.FindAction("LMB").canceled -= (ctx) => { OnUpLMB(); };
        
        _inputActionAsset.FindAction("LMBHold").started -= (ctx) => { OnStartHoldLMB(); };
        _inputActionAsset.FindAction("LMBHold").performed -= (ctx) => { OnHoldLMB(); };
        _inputActionAsset.FindAction("LMBHold").canceled -= (ctx) => { OnCancelHoldLMB(); };
        
        _inputActionAsset.Disable();
    }
}