using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * �÷��̾ eŰ�� ������ �������� ���� �ִϸ��̼��� ���.
 * ������ �ִϸ��̼� ��� �� power_indicator Ʈ���� Ȱ��ȭ, eŰ ���� �� ���� (���� Ŵ)
 * ���� ������ ������ ����� (cctv monitor)
 */

//�ǻ��ڵ�
//eŰ ���� -> ������ �Ѳ� ���鼭 ���������� �ִϸ��̼� ��� (�ִϸ��̼��� �ϳ��� �Ұ��� ���ÿ� �����ų����)
//eŰ ������ ���� Ŵ (����Ʈ ����)
//������ �ִϸ��̼��� ����Ǿ��� ������ ���Դٸ� cctv���� ������ ���� (�� 30��)

public class VHSPlayer : MonoBehaviour
{
    //�÷��̾ ����ĳ��Ʈ�� ��. ������ ȹ�� ���ο� ������� �÷��̾ ��ó�� ����
    //�������� �������ּ��� ��� ui�� ��

    //private Animation animation;
    public Animator animator;

    private void Start()
    {
        //animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            animator.SetTrigger("TapeTake");
        }
        Debug.Log("�ǰ� �ִ°�?");
    }



}
