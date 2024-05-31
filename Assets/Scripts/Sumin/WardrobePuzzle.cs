using UnityEngine;

namespace eneru7i
{
    /// <summary>
    /// ������� ����
    /// </summary>
    public class WardrobePuzzle : MonoBehaviour
    {
        //å
        public GameObject book;
        //�Ҿ���� å
        public GameObject lostbook;
        //���;��� ������
        public GameObject videotape;

        public Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
            book.SetActive(false);
        }


        /// <summary>
        /// �浹 ����
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            //ȭ��ǥ�� ������ ������ �ذ�ǵ��� �ϱ�
            if (collision.gameObject == lostbook)
            {
                Solved();
            }
        }

        /// <summary>
        /// ���� �ذ�
        /// </summary>
        /// <returns></returns>
        void Solved()
        {
            Destroy(lostbook);
            book.SetActive(true);

            animator.SetBool("Book", true);
        }
    }
}
