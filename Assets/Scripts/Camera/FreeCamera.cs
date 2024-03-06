using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static PlayerInputs;

namespace ColorQuest.FreeCam
{
    public class FreeCamera : MonoBehaviour, IFreeCameraActions
    {
        private PlayerInputs playerInputs;

        //Cam script based on UnityEngine.Rendering.FreeCamera
        const float k_MouseSensitivityMultiplier = 0.01f;

        [Tooltip("Rotation speed when using the mouse")]
        public float m_LookSpeedMouse = 4.0f;

        [Tooltip("Movement speed")]
        public float m_MoveSpeed = 10.0f;

        [Tooltip("Value added to the speed when incrementing")]
        public float m_MoveSpeedIncrement = 2.5f;


        private Vector2 moveInput;
        private Vector2 lookInput;
        float inputRotateAxisX, inputRotateAxisY;
        float inputChangeSpeed;
        float inputVertical, inputHorizontal, inputYAxis;



        void Awake()
        {
            playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            playerInputs.FreeCamera.Enable();
            playerInputs.FreeCamera.AddCallbacks(this);
            playerInputs.FreeCamera.Move.Disable();
            playerInputs.FreeCamera.Look.Disable();
            Cursor.lockState = CursorLockMode.Confined;
            
        }

        private void OnDisable()
        {
            playerInputs.FreeCamera.RemoveCallbacks(this);
            playerInputs.FreeCamera.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        public void OnUnlockCam(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    Cursor.lockState = CursorLockMode.Locked;
                    playerInputs.FreeCamera.Move.Enable();
                    playerInputs.FreeCamera.Look.Enable();
                    break;
                case InputActionPhase.Canceled:
                    Cursor.lockState = CursorLockMode.Confined;
                    playerInputs.FreeCamera.Move.Disable();
                    playerInputs.FreeCamera.Look.Disable();
                    break;
            }
        }

        private void Update()
        {
            HandleCamMovement();
        }

        void HandleCamMovement()
        {
            inputVertical = moveInput.y;
            inputHorizontal = moveInput.x;
            inputRotateAxisX = lookInput.x * m_LookSpeedMouse * k_MouseSensitivityMultiplier;
            inputRotateAxisY = lookInput.y * m_LookSpeedMouse * k_MouseSensitivityMultiplier;

            if (inputChangeSpeed != 0.0f)
            {
                m_MoveSpeed += inputChangeSpeed * m_MoveSpeedIncrement;
                if (m_MoveSpeed < m_MoveSpeedIncrement) m_MoveSpeed = m_MoveSpeedIncrement;
            }

            bool moved = inputRotateAxisX != 0.0f || inputRotateAxisY != 0.0f || inputVertical != 0.0f || inputHorizontal != 0.0f || inputYAxis != 0.0f;
            if (moved)
            {
                float rotationX = transform.localEulerAngles.x;
                float newRotationY = transform.localEulerAngles.y + inputRotateAxisX;

                // Weird clamping code due to weird Euler angle mapping...
                float newRotationX = (rotationX - inputRotateAxisY);
                if (rotationX <= 90.0f && newRotationX >= 0.0f)
                    newRotationX = Mathf.Clamp(newRotationX, 0.0f, 90.0f);
                if (rotationX >= 270.0f)
                    newRotationX = Mathf.Clamp(newRotationX, 270.0f, 360.0f);

                transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, transform.localEulerAngles.z);

                float moveSpeed = Time.deltaTime * m_MoveSpeed;
                transform.position += transform.forward * moveSpeed * inputVertical;
                transform.position += transform.right * moveSpeed * inputHorizontal;
                transform.position += Vector3.up * moveSpeed * inputYAxis;
            }
        }
    }
}

