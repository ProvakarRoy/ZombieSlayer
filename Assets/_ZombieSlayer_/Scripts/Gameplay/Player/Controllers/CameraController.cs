using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public bool UsingOrbitalCamera { get; private set; } = false;

    [SerializeField] HumanoidLandInput _input;
    [SerializeField] float _cameraZoomModifier = 32.0f;

    float _minCameraZoomDistance = 0.0f;
    float _maxCameraZoomDistance = 12.0f;
    float _minOrbitCameraZoomDistance = 1.0f;
    float _maxOrbitCameraZoomDistance = 32.0f;

    CinemachineVirtualCamera _activeCamera;
    int _activeCameraPriorityModifier = 31337;

    public Camera mainCamera;
    public CinemachineVirtualCamera Cinemachine1stPerson;
    public CinemachineVirtualCamera Cinemachine3rdPerson;
    private CinemachineFramingTransposer _cinemachineFramingTransposer3rdPerson;
    public CinemachineVirtualCamera CinemachineOrbit;
    private CinemachineFramingTransposer _cinemachineFramingTransposerOrbit;

    private void Awake()
    {
        _cinemachineFramingTransposer3rdPerson = Cinemachine3rdPerson.GetComponent<CinemachineFramingTransposer>();
        _cinemachineFramingTransposerOrbit = CinemachineOrbit.GetComponent<CinemachineFramingTransposer>();

    }

    private void Start()
    {
        ChangeCamera(); // First Time Through, let's Set the Default camera
    }

    private void Update()
    {
        if(!(_input.ZoomCameraInput == 0.0f))
        {
            ZoomCamera();
        }
        {

        }
        if(_input.CameraChangeWasPressedThisFrame)
        {
            ChangeCamera();
        }
    }

    private void ZoomCamera()
    {
        if (_activeCamera == Cinemachine3rdPerson)
        {
            _cinemachineFramingTransposer3rdPerson.m_CameraDistance += Mathf.Clamp(_cinemachineFramingTransposer3rdPerson.m_CameraDistance +
                    (_input.InvertScroll ? _input.ZoomCameraInput : -_input.ZoomCameraInput) / _cameraZoomModifier,
                    _minCameraZoomDistance,
                    _maxCameraZoomDistance);
        }
        else if (_activeCamera == CinemachineOrbit)
        {
            _cinemachineFramingTransposerOrbit.m_CameraDistance += Mathf.Clamp(_cinemachineFramingTransposerOrbit.m_CameraDistance +
                    (_input.InvertScroll ? _input.ZoomCameraInput : -_input.ZoomCameraInput) / _cameraZoomModifier,
                    _minOrbitCameraZoomDistance,
                    _maxOrbitCameraZoomDistance);
        }
    }

    private void ChangeCamera()
    {
        if (Cinemachine3rdPerson == _activeCamera)
        {
            SetCameraPriorities(Cinemachine3rdPerson, Cinemachine1stPerson);
            UsingOrbitalCamera = false;
        }
        else if (Cinemachine1stPerson == _activeCamera)
        {
            SetCameraPriorities(Cinemachine1stPerson, CinemachineOrbit);
            UsingOrbitalCamera = true;
        }
        else if (CinemachineOrbit == _activeCamera)
        {
            SetCameraPriorities(CinemachineOrbit, Cinemachine3rdPerson);
            UsingOrbitalCamera = false;
            _activeCamera = Cinemachine3rdPerson;
        }
        else
        {
            Cinemachine3rdPerson.Priority += _activeCameraPriorityModifier;
            _activeCamera = Cinemachine3rdPerson;
        }
    }

    private void SetCameraPriorities(CinemachineVirtualCamera CurrentCameraMode, CinemachineVirtualCamera NewCameraMode)
    {
        CurrentCameraMode.Priority -= _activeCameraPriorityModifier;
        NewCameraMode.Priority += _activeCameraPriorityModifier;
        _activeCamera = NewCameraMode;
    }

}
