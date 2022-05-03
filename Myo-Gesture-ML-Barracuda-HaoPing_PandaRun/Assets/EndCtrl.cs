using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCtrl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeAfter1SecondsCoroutine());
        }
    }

    IEnumerator ChangeAfter1SecondsCoroutine()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }

}
