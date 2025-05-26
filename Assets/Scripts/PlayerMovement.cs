using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Rigidbody rb;
    [SerializeField] float weight_max14; //max is double of moveSpeed
    [SerializeField] MemoryData memoryData;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    private float moveX;
    private float moveY;
    private bool isGrounded;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        memoryData.Convert();
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
        transform.position += new Vector3(moveX, 0f, moveY) * (moveSpeed - weight_max14 / 2) * Time.deltaTime;
        weight_max14 = MemoryData.MemoryList.Count;
        //for physics based movement: rb.AddForce(new Vector3(moveX, 0f, moveY) * 2f * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
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
            rb.AddForce(Vector3.up * (jumpForce - weight_max14/4), ForceMode.Impulse);
            isGrounded = false;
        }
    }
}
