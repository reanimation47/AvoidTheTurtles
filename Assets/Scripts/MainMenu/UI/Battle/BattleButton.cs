using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : MonoBehaviour
{
    void OnMouseDown()
    {
        startGameAfter(2f);
    }

    IEnumerator startGameAfterIE(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SampleScene");
    }
    void startGameAfter(float delay)
    {
        StartCoroutine(startGameAfterIE(delay));
    }
}
