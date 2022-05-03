using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCtrl : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public SoundCtrl soundctrl;
    int BambooCollected = 0;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bamboo"))
        {
            BambooCollected++;
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(soundctrl.Chew, soundctrl.transform.position);
        }

        else if (other.gameObject.CompareTag("Finish"))
        {
            AudioSource.PlayClipAtPoint(soundctrl.Cheer, soundctrl.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = "Score: " + (BambooCollected);
    }
}
