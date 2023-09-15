using Cinemachine;
using UnityEngine;
using Utils.FactoryTool;

public class CameraController : PoolableMonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    public CinemachineVirtualCamera camera => _camera;
    
    public static CameraController instance { get; private set; }
    
    private void Awake()
    {
        instance = this;
    }
    
    public void SetTarget(Transform target)
    {
        _camera.Follow = target;
        _camera.LookAt = target;
    }
}