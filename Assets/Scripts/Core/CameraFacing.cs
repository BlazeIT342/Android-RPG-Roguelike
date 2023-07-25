using UnityEngine;
using Cinemachine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        //[SerializeField] CinemachineFreeLook playerFramingCamera;

        //private void Start()
        //{
        //    playerFramingCamera = GameObject.FindGameObjectWithTag("PlayerFramingCamera").GetComponent<CinemachineFreeLook>();
        //}

        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
            //transform.LookAt(2 * transform.position - playerFramingCamera.transform.position);
        }
    }
}