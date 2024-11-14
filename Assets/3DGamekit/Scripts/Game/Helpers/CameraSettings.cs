using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Gamekit3D
{
    public class CameraSettings : MonoBehaviour
    {
        public enum InputChoice
        {
            KeyboardAndMouse, Controller,
        }

        public Transform follow;
        public Transform lookAt;
        public CinemachineCamera keyboardAndMouseCamera;
        public CinemachineCamera controllerCamera;
        public InputChoice inputChoice;
        public bool allowRuntimeCameraSettingsChanges;

        public CinemachineCamera Current
        {
            get { return inputChoice == InputChoice.KeyboardAndMouse ? keyboardAndMouseCamera : controllerCamera; }
        }

        void Reset()
        {
            Transform keyboardAndMouseCameraTransform = transform.Find("KeyboardAndMouseFreeLookRig");
            if (keyboardAndMouseCameraTransform != null)
                keyboardAndMouseCamera = keyboardAndMouseCameraTransform.GetComponent<CinemachineCamera>();

            Transform controllerCameraTransform = transform.Find("ControllerFreeLookRig");
            if (controllerCameraTransform != null)
                controllerCamera = controllerCameraTransform.GetComponent<CinemachineCamera>();

            PlayerController playerController = FindAnyObjectByType<PlayerController>();
            if (playerController != null && playerController.name == "Ellen")
            {
                follow = playerController.transform;

                lookAt = follow.Find("HeadTarget");

                if (playerController.cameraSettings == null)
                    playerController.cameraSettings = this;
            }
        }

        void Awake()
        {
            UpdateCameraSettings();
        }

        void Update()
        {
            if (allowRuntimeCameraSettingsChanges)
            {
                UpdateCameraSettings();
            }
        }

        void UpdateCameraSettings()
        {
            keyboardAndMouseCamera.Follow = follow;
            keyboardAndMouseCamera.LookAt = lookAt;

            controllerCamera.Follow = follow;
            controllerCamera.LookAt = lookAt;

            keyboardAndMouseCamera.Priority = inputChoice == InputChoice.KeyboardAndMouse ? 1 : 0;
            controllerCamera.Priority = inputChoice == InputChoice.Controller ? 1 : 0;
        }
    } 
}
