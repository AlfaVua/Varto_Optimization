using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Varto.FPSTests
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private float groundRaycastDistance = 2;
        [SerializeField] private LayerMask groundLayer;
        [Header("Controls")]
        [SerializeField] private Rigidbody playerBody;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private float speed = 2;
        [SerializeField] private float sensitivity = 1;
        [SerializeField] private Vector2 verticalLookConstraints;
        
        private PlayerInputSystem _inputSystem;

        private float _yRotation = 0;
        private float _xRotation = 0;

        private void Awake()
        {
            _inputSystem = new PlayerInputSystem();
        }

        private void Start()
        {
            _inputSystem.Player.Jump.performed += PlayerJump;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void PlayerJump(InputAction.CallbackContext context)
        {
            if (Physics.Raycast(playerBody.transform.position, Vector3.down, groundRaycastDistance, groundLayer))
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void Update()
        {
            var pointerDeltas = _inputSystem.Player.Look.ReadValue<Vector2>();
            var actualDeltas = sensitivity * Time.deltaTime * pointerDeltas;
            
            _yRotation -= actualDeltas.y;
            _yRotation = Mathf.Clamp(_yRotation, verticalLookConstraints.y, verticalLookConstraints.x);
            
            _xRotation += actualDeltas.x;
            
            transform.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0f);
        }

        private void FixedUpdate()
        {
            if (playerBody.velocity.y < -1) playerBody.velocity += Physics.gravity / 20f;

            var moveDirection = _inputSystem.Player.Move.ReadValue<Vector2>();
            
            var globalDirection = new Vector3(moveDirection.x, 0f, moveDirection.y);
            var relativeDirection = transform.TransformDirection(globalDirection);
            var accurateDirection = new Vector3(relativeDirection.x, 0f, relativeDirection.z).normalized * speed;
            playerBody.velocity = new Vector3(accurateDirection.x, playerBody.velocity.y, accurateDirection.z);
        }

        private void OnEnable()
        {
            _inputSystem.Enable();
        }

        private void OnDisable()
        {
            _inputSystem.Disable();
        }
    }
}