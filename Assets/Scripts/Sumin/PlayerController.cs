using UnityEngine;
using UnityEngine.InputSystem;

namespace eneru7i
{
    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ�
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        //�÷��̾�
        public GameObject player;
        //ī�޶�
        public Camera mainCamera;
        //���콺 ����
        public float mouseSensitivity = 100f;
        //�̵��ӵ�
        float speed = 3f;
        //ī�޶� ���� ����
        float xRotation = 0f;
        //������ٵ�
        Rigidbody rb;
        //�ִϸ�����
        Animator animator;
        //���� ����� ����
        bool isGround = true;
        //�޸����� ����
        bool isRunning = false;
        //�ɾư����� ����
        bool isCrouch = false;
        //���� Ű
        public float originalHeight;
        //���� Ű
        public float crouchHeight;
        //�ö��̴�
        CapsuleCollider playerCollider;
        //��ǲ�ý��� 
        private Vector2 moving;
        private Vector2 look;
        private bool jump;
        private bool interact;

        void Start()
        {
            if (player == null)
            {
                player = this.gameObject;
            }

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            rb = player.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = player.AddComponent<Rigidbody>();
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            playerCollider = player.GetComponent<CapsuleCollider>();
            if (playerCollider == null)
            {
                playerCollider = player.AddComponent<CapsuleCollider>();
            }
            originalHeight = playerCollider.height;

            animator = player.GetComponent<Animator>();
        }

        /// <summary>
        /// �÷��̾� ������ ����
        /// </summary>
        void Update()
        {
            Look();
            Interact();
            Move();
            if (jump)
            {
                Jump();
            }
        }

        #region �÷��̾� ����
        /// <summary>
        /// ���콺�� ȭ�� ����
        /// </summary>
        public void Look()
        {
            // ���콺 �Է� ���� �޾ƿɴϴ�.
            float mouseX = look.x * mouseSensitivity * Time.deltaTime;
            float mouseY = look.y * mouseSensitivity * Time.deltaTime;

            // �÷��̾ ���� �������� ȸ���մϴ�.
            player.transform.Rotate(Vector3.up * mouseX);

            // ī�޶� ���� �������� ȸ���մϴ�.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        /// <summary>
        /// Ŭ���� ��ȣ�ۿ�
        /// </summary>
        public void Interact()
        {
            if (interact)
            {
                Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 5f))
                {
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        Debug.Log("interacting");
                    }
                }
            }
        }

        /// <summary>
        /// �÷��̾� �̵�
        /// </summary>
        public void Move()
        {
            float moveX = moving.x;
            float moveZ = moving.y;
            //�޸��� ���ο� ���� �̵��ӵ� ����
            float speedGain = isRunning ? 2 : 1;
            //���̱� ���ο� ���� �̵��ӵ� ����
            float currentSpeed = isCrouch ? speed * 0.5f : speed;
            //�̵��ӵ� ���
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            transform.position += move * currentSpeed * speedGain * Time.deltaTime;
            //�̵� �ִϸ��̼� ���
            animator.SetFloat("MoveX", moveX * speedGain);
            animator.SetFloat("MoveY", moveZ * speedGain);
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Jump()
        {
            if (isGround && !isCrouch)
            {
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                isGround = false;
            }
            jump = false;
        }

        /// <summary>
        /// �޸���
        /// </summary>
        public void Running()
        {
            if (!isRunning && isGround)
            {
                isRunning = true;
            }
            else if (isGround)
            {
                isRunning = false;
            }
        }

        /// <summary>
        /// ���̱�
        /// </summary>
        public void Crouch()
        {
            ///���̱�
            if (!isCrouch && isGround)
            {
                animator.SetBool("Crouch",true);
                //���� ����� Ű
                playerCollider.height = crouchHeight;
                //���ϰ�� ����
                playerCollider.center = new Vector3(playerCollider.center.x, crouchHeight / 2f, playerCollider.center.z);
                isCrouch = true;
            }
            ///�Ͼ��
            else if (isGround)
            {
                animator.SetBool("Crouch", false);
                //�Ͼ ����� Ű
                playerCollider.height = originalHeight;
                //�Ͼ ��� ����
                playerCollider.center = new Vector3(playerCollider.center.x, originalHeight / 2f, playerCollider.center.z);
                isCrouch = false;
            }
        }
        #endregion

        // Unity Events �޼���
        #region unity event

        /// <summary>
        /// �̵� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            moving = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// �ü���ȯ ����
        /// </summary>
        /// <param name="context"></param>
        public void OnLook(InputAction.CallbackContext context)
        {
            look = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && !isCrouch)
            {
                jump = true;
            }
            else if (context.performed && isCrouch)
            {
                Crouch();
            }
        }

        /// <summary>
        /// �޸��� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // �޸��� ���¸� �����մϴ�.
                Running();
            }
            else if (context.canceled)
            {
                // �޸��� ���¸� �����մϴ�.
                isRunning = false;
            }
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Crouch();
            }
        }

        /// <summary>
        /// ��ȣ�ۿ� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                interact = true;
            }
            else if (context.canceled)
            {
                interact = false;
            }
        }
        #endregion

        /// <summary>
        /// ��ü �浹 �̺�Ʈ
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Untagged"))
            {
                isGround = true;              
            }
        }
    }
}
