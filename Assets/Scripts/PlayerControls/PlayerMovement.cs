using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _jumpHeight = 1.5f;

    [Header("Look Settings")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _lookSensitivity = 2f;
    [SerializeField] private float _maxLookAngle = 80f;

    private const float GRAVITY = -9.81f;
    private const float GROUND_STICK_FORCE = -2f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isSprinting = false;
    private float _verticalLookRotation = 0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void Move(Vector2 input)
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = GROUND_STICK_FORCE;
        }

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        float speed = _isSprinting ? _moveSpeed * _sprintMultiplier : _moveSpeed;

        Vector3 finalMove = move * speed * Time.deltaTime;
        _controller.Move(finalMove);

        _velocity.y += GRAVITY * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    public void Look(Vector2 input)
    {
        float mouseX = input.x * _lookSensitivity;
        float mouseY = input.y * _lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _verticalLookRotation -= mouseY;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -_maxLookAngle, _maxLookAngle);
        _cameraTransform.localEulerAngles = new Vector3(_verticalLookRotation, 0f, 0f);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * GRAVITY);
        }
    }

    public void SetSprinting(bool sprinting)
    {
        _isSprinting = sprinting;
    }
}