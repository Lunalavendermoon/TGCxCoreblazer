using System.Collections;
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
    [SerializeField] RespawnScript respawnScript;
    [SerializeField] float max_distance_from_ground;

    private float moveX;
    private float moveY;
    private bool isGrounded;
    private bool jumpSound;
    private bool jumpCooldownFinished;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        memoryData.Convert();
        jumpCooldownFinished = true;
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

            Debug.DrawRay(transform.position, -transform.up * 10f, Color.red);
            //ground check
            if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hits, max_distance_from_ground))
            {
                isGrounded = true; //if not far from ground
                jumpSound = true;
            }
            else
            {
                isGrounded = false;
                jumpSound = false;
            }

                //rotation
                Vector3 direction = new Vector3(moveX, 0f, moveY).normalized;
            float magnitude = new Vector3(moveX, 0f, moveY).magnitude;
            if (magnitude > 0f)
            {
                Quaternion current = transform.rotation;
                //Quaternion rotation = Quaternion.LookRotation(npcPosition.transform.position - transform.position);
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            isGrounded = false;
            jumpSound = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        respawnScript.updateCheckpoint(collision.gameObject.name);

        //LayerMask checkPointModifier = LayerMask.GetMask("CheckpointModifier"); //gets bitwise representation of that layer
        /*
         * & - is BITWISE and operator - checks if there is overlap between bits
         * 1 << gameObject.layer - converts layer to bitwise representation
         */
        //if ((1 << collision.gameObject.layer & checkPointModifier) != 0)
        //{
        //    //Debug.Log("touched checkpoint modifier");
        //    respawnScript.updateCheckpoint(collision.gameObject.name);
        //}
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("GroundSurface"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isGrounded = false;
    //    jumpSound = true;
    //}

    private void OnJump()
    {
        if (isGrounded && jumpCooldownFinished)
        {
            StartCoroutine(runJumpCooldown());
            isGrounded = false;
            rb.AddForce(Vector3.up * (jumpForce - weight_max14/4), ForceMode.Impulse);
            if (jumpSound)
            {
                AudioManager.Instance.PlaySFX("tap");
                jumpSound = false;
            }
        }
    }

public void faceNPC(Transform npcPosition)
    {
        //transform.LookAt(npcPosition.transform); //snappy rotate to face if u like that better :>
        StartCoroutine(rotateOverTime(npcPosition.position));
    }

    public IEnumerator rotateOverTime(Vector3 npcPosition)
    {
        float startTime = Time.time;
        float duration = 2.0f;
        while(Time.time - startTime < duration)
        {
            Quaternion current = transform.rotation;
            Vector3 npcPosLevelWithPlayer = new Vector3(npcPosition.x, transform.position.y, npcPosition.z);
            //so that player doesn't start looking into the sky or into the ground LMFAO
            Quaternion rotation = Quaternion.LookRotation(npcPosLevelWithPlayer - transform.position);
            transform.rotation = Quaternion.Slerp(current, rotation, (Time.time - startTime)/duration); //gives fraction of rotation complete
            yield return null;
        }
        yield break;
    }

    private IEnumerator runJumpCooldown()
    {
        jumpCooldownFinished = false;
        yield return new WaitForSeconds(0.1f);
        jumpCooldownFinished = true;
    }
}