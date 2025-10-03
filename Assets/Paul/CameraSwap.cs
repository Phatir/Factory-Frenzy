using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("R�f�rence des cam�ras virtuelles")]
    public CinemachineCamera cam1;
    public CinemachineCamera cam2;

    private bool isCam1Active = true;

    void Start()
    {
        // Initialisation : cam1 active, cam2 inactive
        cam1.Priority.Value = 10;
        cam2.Priority.Value = 0;
    }

    /// <summary>
    /// Inverse la cam�ra active (�change les priorit�s)
    /// </summary>
    public void SwitchCamera()
    {
        if (isCam1Active)
        {
            cam1.Priority.Value = 0;
            cam2.Priority.Value = 10;
        }
        else
        {
            cam1.Priority.Value = 10;
            cam2.Priority.Value = 0;
        }

        isCam1Active = !isCam1Active;
    }
}
