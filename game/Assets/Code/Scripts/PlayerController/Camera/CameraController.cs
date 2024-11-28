using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityUtils;

namespace AdvancedController {
    
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraController : MonoBehaviour {
        
        [SerializeField] private PlayerInstance player;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private new Camera camera;
        [SerializeField] private Transform camAnchor;

        [SerializeField, Range(0f, 90f)] private float upperVerticalLimit = 35f;
        [SerializeField, Range(0f, 90f)] private float lowerVerticalLimit = 35f;
        [SerializeField, Range(1f, 50f)] private float cameraSmoothingFactor = 25f;

        [SerializeField] private float minCameraSpeed = 20f;
        [SerializeField] private float maxCameraSpeed = 100f;
        [SerializeField] private bool smoothCameraRotation;

        private float CameraSpeed => Mathf.Lerp(minCameraSpeed, maxCameraSpeed, _sensitivity);
        private InputReader Input => player.InputReader;
        public Camera Camera => camera;
        public CinemachineCamera CinemachineCamera => cinemachineCamera;
        public Vector3 GetUpDirection() => camAnchor.up;
        public Vector3 GetFacingDirection () => camAnchor.forward;
        
        private float _currentXAngle;
        private float _currentYAngle;
        
        private bool _invertY = false;
        private bool _invertX = false;
        private float _sensitivity = .5f;

        void Awake() {
            _currentXAngle = camAnchor.localRotation.eulerAngles.x;
            _currentYAngle = camAnchor.localRotation.eulerAngles.y;

            GameSettings.OnInvertXChanged += OnInvertX;
            GameSettings.OnInvertYChanged += OnInvertY;
            GameSettings.OnSensitivityChanged += OnSensitivity;
        }

        private void OnDestroy() {
            GameSettings.OnInvertXChanged -= OnInvertX;
            GameSettings.OnInvertYChanged -= OnInvertY;
            GameSettings.OnSensitivityChanged -= OnSensitivity;
        }

        private void Start()
        {
            _invertY = Mathf.Approximately(SaveSystem.GetFloat(SaveSystem.SaveVariable.InvertY), 1f);
            _invertX = Mathf.Approximately(SaveSystem.GetFloat(SaveSystem.SaveVariable.InvertX), 1f);
            _sensitivity = SaveSystem.GetFloat(SaveSystem.SaveVariable.Sensitivity);
        }

        void FixedUpdate() {
            RotateCamera(Input.LookDirection.x, -Input.LookDirection.y);
        }

        void RotateCamera(float horizontalInput, float verticalInput) {
            if (smoothCameraRotation) {
                horizontalInput = Mathf.Lerp(0, horizontalInput, Time.fixedDeltaTime * cameraSmoothingFactor);
                verticalInput = Mathf.Lerp(0, verticalInput, Time.fixedDeltaTime * cameraSmoothingFactor);
            }
            
            _currentXAngle += verticalInput * CameraSpeed * Time.fixedDeltaTime * (_invertY ? -1 : 1);
            _currentYAngle += horizontalInput * CameraSpeed * Time.fixedDeltaTime * (_invertX ? -1 : 1);
            
            _currentXAngle = Mathf.Clamp(_currentXAngle, -upperVerticalLimit, lowerVerticalLimit);
            
            camAnchor.localRotation = Quaternion.Euler(_currentXAngle, _currentYAngle, 0);
        }

        private void OnInvertX(bool value) => _invertX = value;
        private void OnInvertY(bool value) => _invertY = value;
        private void OnSensitivity(float value) => _sensitivity = value;
    }
}