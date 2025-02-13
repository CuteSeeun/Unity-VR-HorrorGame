using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenAnimation : MonoBehaviour
{
    //public Animator animator;
    public GameObject ui;
    public Animator animator;
    public Collider selfCollider;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(BoxUiStart());

            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("isOpen");
                selfCollider.enabled = false;

            }
        }
    }

    IEnumerator BoxUiStart()
    {
        ui.SetActive(true);
        yield return new WaitForSeconds(2);
        ui.SetActive(false);
    }
}
