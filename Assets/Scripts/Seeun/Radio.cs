using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    //1. ���� ���� �� ���� ����� �Ҹ� ��� , ui ���
    //2. �÷��̾ eŰ�� ������ ���� �Ҹ��� ���� �ִϸ��̼��� ����. �׸��� ������ ������ �ִϸ��̼��� ���.

    public AudioSource audioSource; // Unity �ν����Ϳ��� ������ �� �ֵ��� public���� ����
    public Animator animator;       // Unity �ν����Ϳ��� ������ �� �ֵ��� public���� ����
    public GameObject uiElement;    // ���� UI ���

    void Start()
    {
        // ���� ���� �� ������� ����ϰ� UI�� Ȱ��ȭ
        audioSource.Play();
        uiElement.SetActive(true);
    }

    public void TurnOffRadio()
    {
        // ����� ���� �� �ִϸ��̼� ����
        audioSource.Stop();
        animator.SetTrigger("turnOff");
    }
}
