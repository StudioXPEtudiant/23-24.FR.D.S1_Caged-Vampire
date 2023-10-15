using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    private Camera _mainCamera; // Reference to the main camera.
    private CinemachineVirtualCamera _virtualCamera; // Reference to the virtual camera.

    private void Start()
    {
        _mainCamera = Camera.main; // Get the main camera.
        _virtualCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>(); // Get the virtual camera.
    }

    public void ChangeFieldView(float value, float speed) {
        _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(_virtualCamera.m_Lens.OrthographicSize, value, Time.deltaTime * speed);
    }
}