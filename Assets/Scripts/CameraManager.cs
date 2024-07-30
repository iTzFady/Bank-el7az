using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private int currentCameraIndex = 0;
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
        switchToCamera((int)CameraType.Dice, null);
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
