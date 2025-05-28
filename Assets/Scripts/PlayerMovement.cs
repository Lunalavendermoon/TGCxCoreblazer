using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    [SerializeField] float weight_max14; //max is double of moveSpeed
    [SerializeField] MemoryData memoryData;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float rotationSpeed;
    [SerializeField] DialogueRunner dialogueRunner; //for detecting if dialogue is running

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
        //disable character movement during dialogue
        if(!dialogueRunner.IsDialogueRunning) 
        {
            moveX = inputActions.Player.Move.ReadValue<Vector2>().x; //in Vector3 - (x, 0, 0)
            moveY = inputActions.Player.Move.ReadValue<Vector2>().y; //in Vector 3 - (0, 0, z/y)

            //movement
            weight_max14 = MemoryData.MemoryList.Count;
            transform.position += new Vector3(moveX, 0f, moveY) * (moveSpeed - weight_max14 / 2) * Time.deltaTime;
            //for physics based movement: rb.AddForce(new Vector3(moveX, 0f, moveY) * 2f * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            //rotation
            Vector3 direction = new Vector3(moveX, 0f, moveY).normalized;
            float magnitude = new Vector3(moveX, 0f, moveY).magnitude;
            if (magnitude > 0f)
            {
                Quaternion current = transform.rotation;
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
            }
        }
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

    public void faceNPC(GameObject npcPosition)
    {
        Debug.Log(npcPosition.transform.position);
        Quaternion current = transform.rotation;
        Vector3 targetDirection = npcPosition.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
    }
}
