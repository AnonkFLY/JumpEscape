using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _transform;
    private Transform _playerTransform;
    private float _offsetY = 8;
    private bool lockState = true;
    private Vector3 _tempVector;
    //[SerializeField] private Transform _worldPos;
    //[SerializeField] private Transform _uiPos;
    private void Start()
    {
        _transform = transform;
        //_playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        //_offsetY = _playerTransform.position.y - _transform.position.y;
    }
    public void LateUpdate()
    {
        if (lockState && _playerTransform != null)
        {
            _tempVector = _transform.position;
            _tempVector.y = _playerTransform.position.y - _offsetY;
            _transform.position = _tempVector;
        }

        //_uiPos.position = Camera.main.WorldToScreenPoint(_worldPos.position);
    }
    public void SetPlayerManager(PlayerManager playerManager)
    {
        _playerTransform = playerManager.transform;
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
