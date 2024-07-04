using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _transform;
    private Transform _playerTransform;
    private float _offsetY;
    private bool lockState = true;
    private Vector3 _tempVector;
    private void Start()
    {
        _transform = transform;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _offsetY = _playerTransform.position.y - _transform.position.y;
    }
    public void LateUpdate()
    {
        _tempVector = _transform.position;
        _tempVector.y = _playerTransform.position.y - _offsetY;
        _transform.position = _tempVector;
    }
    public void UnLock()
    {
        lockState = false;
    }
    public void Locked()
    {
        lockState = true;
    }
}
