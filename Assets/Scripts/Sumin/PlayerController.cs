using UnityEngine;

namespace eneru7i
{
    /// <summary>
    /// �÷��̾ ��Ʈ���ϴ� �ڵ�
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public GameObject player;
        public Camera mainCamera;
        // ���콺 ����
        public float mouseSensitivity = 100f;
        //�̵��ӵ�
        float speed = 3f;
        // ī�޶� ȸ�� ���� ����
        float xRotation = 0f;
        //Rigidbody ������Ʈ
        Rigidbody rb;
        //���� ��ҳ� ����
        bool isGround = true;
        //���� ��°��� ����
        bool isCrouch = false;
        // �÷��̾� ���� ����
        float originalHeight;
        // �÷��̾� �ɾ��� �� ����
        public float crouchHeight = 0.8f;
        // �÷��̾��� �ݶ��̴�
        CapsuleCollider playerCollider;

        void Start()
        {
            // �÷��̾�� ī�޶� ����
            if (player == null)
            {
                player = this.gameObject;
            }

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            // Rigidbody ������Ʈ �Ҵ�
            rb = player.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = player.AddComponent<Rigidbody>();
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            // �ݶ��̴� �Ҵ� �� ���� ����
            playerCollider = player.GetComponent<CapsuleCollider>();
            if (playerCollider == null)
            {
                playerCollider = player.AddComponent<CapsuleCollider>();
            }
            originalHeight = playerCollider.height;
        }

        /// <summary>
        /// �÷��̾� ������ ����
        /// </summary>
        void Update()
        {
            Rotate();
            Move();
            Jump();
            Crouch();
            Interact();
        }

        #region ���콺 ���
        /// <summary>
        /// �÷��̾� ȸ���ϴ� �Լ�
        /// </summary>
        void Rotate()
        {
            // ���콺 �Է� �ޱ�
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // ��ü �¿� ȸ��
            player.transform.Rotate(Vector3.up * mouseX);

            // ī�޶� ���� ȸ��
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

    /// <summary>
    /// Ŭ���� ��ȣ�ۿ�
    /// </summary>
    void Interact()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                //5f �Ÿ� �� ��ȣ�ۿ��� ������ ��ü�� ���� ���
                if (Physics.Raycast(ray, out RaycastHit hit, 5f))
                {
                    int hitLayer = hit.collider.gameObject.layer;
                    if (hit.collider.CompareTag("Object"))
                    {
                        Debug.Log("interacting");
                    }
                }
            }
        }
        #endregion

        #region Ű���� ���
        /// <summary>
        /// �÷��̾ �����̴� �Լ�
        /// </summary>
        void Move()
        {
            {
                // �¿� �Է¿� ���� �̵�
                float moveX = Input.GetAxis("Horizontal");
                float moveZ = Input.GetAxis("Vertical");
                //����Ʈ ������ �̵��ӵ� 2��
                float speedGain = Input.GetKey(KeyCode.LeftShift)? 2 : 1;
                // ������ �̵��ӵ� ����
                float currentSpeed = isCrouch ? speed * 0.5f : speed;

                // �̵� ���͸� ���� ��ǥ��� ��ȯ
                Vector3 move = transform.right * moveX + transform.forward * moveZ;

                // �̵� �ӵ��� ���� ��ȭ             
                transform.position += move * currentSpeed * speedGain * Time.deltaTime;
            }
        }

        /// <summary>
        /// �÷��̾� �����ϴ� ��ũ��Ʈ
        /// </summary>
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround && !isCrouch)
            {
                // �÷��̾ ���� �������� ���� ���� ����
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                isGround = false;
            }
        }

        /// <summary>
        /// ���� ��� ��ũ��Ʈ
        /// </summary>
        void Crouch()
        {
            //���� �ȱ���
            if (!isCrouch)              
            {
                if (Input.GetKeyDown(KeyCode.C) && isGround)
                {
                    // �ɴ� ���·� ����
                    playerCollider.height = crouchHeight;
                    isCrouch = true;
                }              
            }
            //���� �� ���
            else
            {
                if ((Input.GetKeyDown(KeyCode.C)|| Input.GetKeyDown(KeyCode.Space)) && isGround)
                {
                    // ���� �ʴ� ���·� ����
                    playerCollider.height = originalHeight;
                    isCrouch = false;
                }
            }
        }
        #endregion

        /// <summary>
        /// �浹 ���� ��ũ��Ʈ��
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            // �浹�� ��ü�� ���̸� isGround�� true�� ����
            if (collision.gameObject.CompareTag("Untagged"))
            {
                isGround = true;
            }
        }
    }
}
