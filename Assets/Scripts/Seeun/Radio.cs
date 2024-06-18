using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource audioSource;
    public Light Light;
    public AudioSource offaudioSource;
    public GameObject ui;
    public Collider radioCollider;
    public Collider paperCollider;

    //private bool isInside = false; //�÷��̾ �ݶ��̴� �ȿ� �ִ����� ���θ� ����

    void Start()
    {
        // ���� ���� �� ������� ���
        audioSource.Play();
        //paperCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(true);    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (radioCollider.enabled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                audioSource.Stop();
                offaudioSource.Play();
                Light.enabled = !Light.enabled;
                
                ui.SetActive(false);
                radioCollider.enabled = false;
                //StartCoroutine(RadioColliderDelay(2));
                StartCoroutine(PaperColliderDelay(2));
            }
        }
    }

    IEnumerator PaperColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // ������ �ð�(��)��ŭ ���
        paperCollider.enabled = true;            // ���� �� �ݶ��̴� Ȱ��ȭ
    }
}
