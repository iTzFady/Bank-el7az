using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraManager : NetworkBehaviour
{
    public int currentCameraIndex = 0;
    public CinemachineVirtualCamera[] virtualCameras;
    public enum CameraType
    {
        Dice,
        Player,
        Question,
        Penalty
    }
    void Start()
    {
        for (int i = 1; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].gameObject.SetActive(false);
        }
        if (virtualCameras.Length > 0)
        {
            virtualCameras[0].gameObject.SetActive(true);
        }
    }
    public void switchToCamera(int cameraIndex, PlayerMovement player)
    {
        if (cameraIndex < 0 || cameraIndex >= virtualCameras.Length) return;
        if (player != null)
        {
            virtualCameras[cameraIndex].LookAt = player.transform;
        }
        virtualCameras[currentCameraIndex].gameObject.SetActive(false);
        virtualCameras[cameraIndex].gameObject.SetActive(true);
        currentCameraIndex = cameraIndex;
    }
}
