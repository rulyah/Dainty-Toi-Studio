using Cinemachine;
using UnityEngine;
using Utils.FactoryTool;

public class CameraController : PoolableMonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    public CinemachineVirtualCamera camera => _camera;
    public void SetTarget(Transform target)
    {
        _camera.Follow = target;
        _camera.LookAt = target;
    }
}