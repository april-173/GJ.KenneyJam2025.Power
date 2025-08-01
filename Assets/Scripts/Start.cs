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
        // �ȴ�
        yield return new WaitForSeconds(0.1f);
        // �ر���ʾ
        gameObject.SetActive(false);

        yield return null;
    }
}
