using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Home : MonoBehaviour
{
    [Header("����ѡ��")]
    [Tooltip("�������ƹ�����")]
    [SerializeField] private LevelScenes levelScenes;

    [Space]

    [Tooltip("�������ƣ�����������")]
    [SerializeField] private string selectLevelScenesName;

    private void Start()
    {
        // ʹ��ѡ������������ȡ�ؿ���������
        selectLevelScenesName = levelScenes.levelScenesName[0];
    }

    public void LevelTransition()
    {
        // �л��ؿ�
        SceneManager.LoadScene(0);
    }
}
