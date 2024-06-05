using UnityEditor.XR.LegacyInputHelpers;
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
        public float speed = 2f;
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
        private bool left;
        private bool right;
        // �� ��ġ Ʈ������
        public Transform leftHand;
        public Transform rightHand;
        // �տ� ��� �ִ� ������Ʈ
        private GameObject leftHandObject;
        private GameObject rightHandObject;

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
            Move();
            if (jump)
            {
                Jump();
            }
            HandleInteractions();
        }

        #region �÷��̾� ����
        /// <summary>
        /// ���콺�� ȭ�� ����
        /// </summary>
        public void Look()
        {
            // ���콺 �Է� ���� �޾ƿɴϴ�.
            Vector2 mouseDelta = look * mouseSensitivity * Time.deltaTime;

            // ���� ȸ��
            player.transform.Rotate(Vector3.up * mouseDelta.x);

            // ���� ȸ��
            xRotation -= mouseDelta.y;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        /// <summary>
        /// ��ȣ�ۿ� ó��
        /// </summary>
        private void HandleInteractions()
        {
            if (left)
            {
                if (leftHandObject == null)
                {
                    TryPickupObject(ref leftHandObject, leftHand);
                }
                else
                {
                    leftHandObject.transform.position = leftHand.position;
                }
            }
            else if (leftHandObject != null)
            {
                DropObject(ref leftHandObject);
            }

            if (right)
            {
                if (rightHandObject == null)
                {
                    TryPickupObject(ref rightHandObject, rightHand);
                }
                else
                {
                    rightHandObject.transform.position = rightHand.position;
                }
            }
            else if (rightHandObject != null)
            {
                DropObject(ref rightHandObject);
            }
        }

        /// <summary>
        /// ������Ʈ�� �տ� ��� �õ�
        /// </summary>
        /// <param name="handObject">�տ� �� ������Ʈ</param>
        /// <param name="hand">�� ��ġ</param>
        private void TryPickupObject(ref GameObject handObject, Transform hand)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 2.5f))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    handObject = hit.collider.gameObject;
                    handObject.transform.SetParent(hand);
                    handObject.transform.position = hand.position;
                    handObject.transform.localPosition = Vector3.zero;
                    Rigidbody hitRb = handObject.GetComponent<Rigidbody>();
                    if (hitRb != null)
                    {
                        hitRb.isKinematic = true;
                    }
                }
            }
        }

        /// <summary>
        /// ������Ʈ�� �տ��� ����
        /// </summary>
        /// <param name="handObject">�տ��� ���� ������Ʈ</param>
        private void DropObject(ref GameObject handObject)
        {
            // ���� ���콺 ��ġ�� ȭ�� ��ǥ�� ��ȯ�Ͽ� Ray�� ���ϴ�.
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            // Ray�� �浹�� ������ ������ ����
            RaycastHit hit;

            // Ray�� ���� �浹�� ������ �ִٸ�
            if (Physics.Raycast(ray, out hit, 2.5f))
            {
                // �浹 ������ ��ġ�� �������� ���� ��ġ�� �����մϴ�.
                handObject.transform.position = hit.point;
            }
            // �տ��� ��� �ִ� ������Ʈ�� �θ� �����մϴ�.
            handObject.transform.SetParent(null);

            // �տ��� ��� �ִ� ������Ʈ�� Rigidbody�� �����Ѵٸ�
            Rigidbody hitRb = handObject.GetComponent<Rigidbody>();
            if (!hitRb != null)
            {
                // Rigidbody�� Kinematic �Ӽ��� �����մϴ�.
                hitRb.isKinematic = false;
            }

            // �տ��� ��� �ִ� ������Ʈ�� null�� �ʱ�ȭ�մϴ�.
            handObject = null;
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
        public void Running(bool run)
        {
            if (isGround)
            {
                isRunning = run;
            }
        }

        /// <summary>
        /// ���̱�
        /// </summary>
        public void Crouch()
        {
            if (!isCrouch && isGround)
            {
                // �÷��̾ ������ ���� ó��
                animator.SetBool("Crouch", true);
                // �ö��̴��� ���� Ű ó��
                playerCollider.height = crouchHeight;
                playerCollider.center = new Vector3(playerCollider.center.x, crouchHeight / 2f, playerCollider.center.z);
                // ī�޶� ��ġ ����
                mainCamera.transform.localPosition = new Vector3(0f, crouchHeight, 0f);
                // �յ��� ��ġ ����
                leftHand.transform.localPosition = new Vector3(leftHand.transform.localPosition.x, crouchHeight / 2f, leftHand.transform.localPosition.z);
                rightHand.transform.localPosition = new Vector3(rightHand.transform.localPosition.x, crouchHeight / 2f, rightHand.transform.localPosition.z);
                isCrouch = true;
            }
            else if (isGround)
            {
                // �÷��̾ ���� ���¿��� �Ͼ�� ���� ó��
                animator.SetBool("Crouch", false);
                // �ö��̴��� �Ͼ Ű ó��
                playerCollider.height = originalHeight;
                playerCollider.center = new Vector3(playerCollider.center.x, originalHeight / 2f, playerCollider.center.z);
                // ī�޶� ��ġ ����
                mainCamera.transform.localPosition = new Vector3(0f, originalHeight, 0f);
                // �յ��� ��ġ ����ġ
                leftHand.transform.localPosition = new Vector3(leftHand.transform.localPosition.x, originalHeight, leftHand.transform.localPosition.z);
                rightHand.transform.localPosition = new Vector3(rightHand.transform.localPosition.x, originalHeight, rightHand.transform.localPosition.z);
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
            if (context.performed)
            {
                if (isCrouch)
                {
                    Crouch();
                }
                else
                {
                    jump = true;  // ���� ���� ����
                }
            }
        }

        /// <summary>
        /// �޼� ��ȣ�ۿ� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnLeftInteract(InputAction.CallbackContext context)
        {
            left = context.action.ReadValue<float>() > 0.1f;
        }

        /// <summary>
        /// ������ ��ȣ�ۿ� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnRightInteract(InputAction.CallbackContext context)
        {
            right = context.action.ReadValue<float>() > 0.1f;
        }

        /// <summary>
        /// �޸��� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Running(true);  // �޸��� ����
            }
            else if (context.canceled)
            {
                Running(false);  // �޸��� ����
            }
        }

        /// <summary>
        /// ���̱� ����
        /// </summary>
        /// <param name="context"></param>
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Crouch();
            }
        }
        #endregion

        /// <summary>
        /// ��ü �浹 �̺�Ʈ
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject)
            {
                isGround = true;
            }
        }
    }
}