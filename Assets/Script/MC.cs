using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class MC : MonoBehaviour
    {
        private static readonly int Velocity = Animator.StringToHash("velocity");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        [Header("Control")] [SerializeField] private float speed = 5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private InputActionAsset inputAction;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float sprintSpeed = 10f;
        [SerializeField] private float adjustedHeight;

        [Header("Input")] private InputAction moveAction;
        private InputAction lookAction;
        private InputAction interactAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private NPC curr;

        private float lookSensitivity = 2f; // Sensitivity for rotation

        private CharacterController characterController;
        [SerializeField]private float gravity = -9.81f;
        private Vector3 velocity;
        private bool isSprinting = false;
        private Animator animator;
        private int NPCLayerMask;
        private Vector3 adjust;
        private InputActionMap playerActionMap;
        private InputActionMap UIActionMap;

        void OnEnable()
        {
            if (moveAction is null)
            {
                var playerMap = inputAction.FindActionMap("Player");
                moveAction = playerMap.FindAction("Move");
                lookAction = playerMap.FindAction("Look");
                interactAction = playerMap.FindAction("Interact");
                jumpAction = playerMap.FindAction("Jump");
                sprintAction = playerMap.FindAction("Sprint");
            }

            moveAction.Enable();
            lookAction.Enable();
            interactAction.Enable();
            jumpAction.Enable();
            sprintAction.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
            lookAction.Disable();
            interactAction.Disable();
            jumpAction.Disable();
            sprintAction.Disable();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            moveAction = inputAction.FindActionMap("Player").FindAction("Move");
            lookAction = inputAction.FindActionMap("Player").FindAction("Look");
            interactAction = inputAction.FindActionMap("Player").FindAction("Interact");
            jumpAction = inputAction.FindActionMap("Player").FindAction("Jump");
            sprintAction = inputAction.FindActionMap("Player").FindAction("Sprint");
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            NPCLayerMask = LayerMask.GetMask("NPC");
            adjust = new Vector3(0, adjustedHeight, 0);
            playerActionMap = inputAction.FindActionMap("Player");
            UIActionMap = inputAction.FindActionMap("UI");
        }

        // Update is called once per frame
        void Update()
        {
            Walk();
            Look();
            Interaction();
            Jump();
            Sprint();
        }

        private void Interaction()
        {
            if (interactAction.triggered)
            {
                if (curr)
                {
                    curr.Interact();
                }
            }
        }

        private void Jump()
        {
            if (jumpAction.ReadValue<float>() > 0 && characterController.isGrounded)
            {
                velocity.y = jumpForce;
            }
        }
        
        private void OntriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & NPCLayerMask) != 0)
            {
                Debug.Log("Enter NPC Trigger");
                curr = other.GetComponent<NPC>();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & NPCLayerMask) != 0)
            {
                Debug.Log("Exit NPC Trigger");
                if (curr && other.GetComponent<NPC>() == curr)
                {
                    curr = null;
                }
            }
        }

        private void Sprint()
        {
            isSprinting = sprintAction.ReadValue<float>() > 0;
        }

        private void Look()
        {
            Vector2 lookInput = lookAction.ReadValue<Vector2>();
            float yaw = lookInput.x * lookSensitivity;
            // Rotate character around Y axis
            transform.Rotate(0, yaw, 0);
        }

        private void Walk()
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            float currentSpeed = isSprinting ? sprintSpeed : speed;
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            characterController.Move(move * (currentSpeed * Time.deltaTime));

            // Animator logic
            if (animator)
            {
                bool isWalking = moveInput.magnitude > 0.1f;
                animator.SetBool(IsWalking, isWalking);
                animator.SetFloat(Velocity, move.magnitude * currentSpeed);
            }

            // Gravity
            if (characterController.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}