using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    [Header("Mouse Look")]
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float maxLookAngle = 80f;

    private Rigidbody _rb;
    private Camera _camera;
    private float _verticalRotation = 0f;
    private bool _isGrounded = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.mass = 1f;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;

        _camera = GetComponentInChildren<Camera>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);

        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -maxLookAngle, maxLookAngle);
        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  

        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.forward * vertical + transform.right * horizontal).normalized;
        Vector3 targetVelocity = move * moveSpeed;

        targetVelocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = targetVelocity;

    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if(contact.normal.y > 0.5f)
            {
                _isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }

}

// public -> scripts can access it / exposed to unity by default
// private -> scripts can't acess it / not exposed to unity by default
// [HideInInspector] public -> scripts can access it / not exposed in unity
// [SerializeField] private -> scripts can't access it / exposed in unity