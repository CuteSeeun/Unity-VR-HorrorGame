
using UnityEngine;
using UnityEngine.SceneManagement;

namespace eneru7i
{
    /// <summary>
    /// ���� �Ŵ��� ��ũ��Ʈ
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        public void NewGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        /// <summary>
        /// ���� �̾��ϱ�
        /// </summary>
        public void ContinueGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        /// <summary>
        /// �ɼ� ���� â
        /// </summary>
        public void Options()
        {

        }


    }
}
