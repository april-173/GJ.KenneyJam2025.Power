using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Start : MonoBehaviour
{
    
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        // µ»¥˝
        yield return new WaitForSeconds(0.1f);
        // πÿ±’œ‘ æ
        gameObject.SetActive(false);

        yield return null;
    }
}
