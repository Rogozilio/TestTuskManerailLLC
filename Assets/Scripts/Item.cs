using System;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO data;

    private bool _isInBackpack;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private Vector3 _respawnPosition;
    private Vector3 _respawnEulerRotation;

    public bool IsInBackpack
    {
        set
        {
            _isInBackpack = value;
            _collider.enabled = !value;
            _rigidbody.isKinematic = value;
        }
        get => _isInBackpack;
    }
    
    public ItemSO Data => data;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _respawnPosition = transform.position;
        _respawnEulerRotation = transform.eulerAngles;
    }

    public void MoveTo(Vector3 point)
    {
        _rigidbody.velocity = point - transform.position;
    }

    public void MoveToPoint(Transform point)
    {
        transform.DOMove(point.position, 1f);
        transform.DORotate(point.eulerAngles, 1f);
    }

    public void MoveToRespawn(TweenCallback onFinishedMove)
    {
        transform.DOMove(_respawnPosition, 1f).onComplete = onFinishedMove;
        transform.DORotate(_respawnEulerRotation, 1f);
    }
}
