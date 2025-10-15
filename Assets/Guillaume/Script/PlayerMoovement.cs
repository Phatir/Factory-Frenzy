using UnityEngine;


public class PlayerMoovement : MonoBehaviour
{

    //pour le moovement du joueur
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float gravity = -9.81f;

    //pour la hauteur du saut
    [SerializeField] private float jumpHeight = 2f;

    //pour le crouch
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchTransitionSpeed = 5f;

    //pour le saut 
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [SerializeField] private AudioSource JumpSound;
     private AudioClip Jump;


    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
            

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(jumpKey) && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            JumpSound.Play();
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        HandleCrouch();

        // Empêche le joueur de s'incliner
        Vector3 rot = transform.eulerAngles;
        rot.x = 0f;
        rot.z = 0f;
        transform.eulerAngles = rot;
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(crouchKey))
            isCrouching = !isCrouching;

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
    }





}

