using System.Collections;
using UnityEngine;

namespace eneru7i
{
    /// <summary>
    /// �ð� ���� ������ ���
    /// </summary>
    public class ClockPuzzle : MonoBehaviour
    {
        //��ħ
        public GameObject arrow;
        //�־���ϴ� ��ħ
        public GameObject lostarrow;
        //���;��� å
        public GameObject book;
        
        /// <summary>
        /// ������Ʈ ����
        /// </summary>
        void Start()
        {
            //�ð�� å �����
            arrow.SetActive(false);
            book.SetActive(false);
        }

        /// <summary>
        /// �浹 ����
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            //ȭ��ǥ�� ������ ������ �ذ�ǵ��� �ϱ�
            if (collision.gameObject == lostarrow)
            {
                StartCoroutine(Solved());
            }
        }

        /// <summary>
        /// ���� �ذ�
        /// </summary>
        /// <returns></returns>
        IEnumerator Solved()
        {
            Destroy(lostarrow);
            arrow.SetActive(true);
            //�� ġ�� �ð� �ôٸ���
            yield return new WaitForSeconds(2f);

            book.SetActive(true);
        }
    }
}
