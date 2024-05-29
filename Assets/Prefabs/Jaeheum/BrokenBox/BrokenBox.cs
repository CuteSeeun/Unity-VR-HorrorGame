using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBox : MonoBehaviour
{
    public Collider[] colliders;
    public float Mass = 1;
    public float Drag = 2; 

    private void Awake()
    {
        colliders  = gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider item in colliders)
        {
           Rigidbody rb = item.GetComponent<Rigidbody>();
            if ( rb != null )
            {
                rb.constraints = RigidbodyConstraints.FreezeAll; // ���� ���� ����
                rb.mass = Mass;
                rb.drag = Drag;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon") {  // Weapon �±׷� ������ ������Ʈ�� �ǵ帮��
            colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach(Collider item in colliders)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if( rb != null )
                {
                    rb.constraints = RigidbodyConstraints.None; // rigidbody.constraints üũ ���� 
                    Destroy(gameObject, 30); // �ı��� ������Ʈ�� ��������� �ϰ��� 
                }
            }

            Debug.Log("���˵�");
        }
    }
}
