
using UnityEngine;

namespace Skoggy.LD45.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        public Transform Target;

        public float MouseSensitivity = 10f;
        public float ScrollSpeed = 1f;
        public float ZoomSpeed = 15f;
        public float RotationSpeed = 15f;
        public Vector2 Distance = new Vector2(5f, 30f);
        
        private Camera _cam;
        private float _x, _y;
        private float _distance = 15f;
        private float _targetDistance = 15f;
        private Quaternion _targetRotation, _currentRotation;

        void Start()
        {
            _cam = GetComponent<Camera>();
            _targetDistance = _distance;
            _targetRotation = transform.rotation;
            _currentRotation = transform.rotation;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if(Target == null) return;

            var mouseDelta = new Vector3(
                Input.GetAxis("Mouse Y"),
                Input.GetAxis("Mouse X"),
                0
            );

            var scroll = -Input.mouseScrollDelta.y * ScrollSpeed;

            _targetDistance += scroll * Time.deltaTime;

            _targetDistance = Mathf.Clamp(_targetDistance, Distance.x, Distance.y); 
            _distance = Mathf.Lerp(_distance, _targetDistance, ZoomSpeed * Time.deltaTime);

            _x += mouseDelta.x * MouseSensitivity * Time.deltaTime;
            _y += mouseDelta.y * MouseSensitivity * Time.deltaTime;

            _x = Mathf.Clamp(_x, -70f, -20f);

            _targetRotation = Quaternion.Euler(_x, _y, 0f);

            var rotation = Quaternion.Slerp(_currentRotation, _targetRotation, RotationSpeed * Time.deltaTime);
            var targetPosition = Target.position + (rotation * (Vector3.forward * _distance));
            
            _currentRotation = rotation;

            transform.position = targetPosition;
            transform.LookAt(Target);
        }
    }
}