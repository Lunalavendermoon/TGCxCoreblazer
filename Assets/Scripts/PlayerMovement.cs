using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    private float moveX;
    private float moveY;
    private bool isGrounded;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable(); //start listening for input
    }

    private void OnDisable()
    {
        inputActions.Player.Disable(); //stop listening
    }

    private void Update()
    {
        moveX = inputActions.Player.Move.ReadValue<Vector2>().x;
        moveY = inputActions.Player.Move.ReadValue<Vector2>().y;
        transform.position += new Vector3(moveX, 0f, moveY) * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("GroundSurface"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
}
