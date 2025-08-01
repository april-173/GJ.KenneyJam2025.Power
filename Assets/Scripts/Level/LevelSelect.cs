using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
    [Header("����ѡ��")]
    [Tooltip("�������ƹ�����")]
    [SerializeField] private LevelScenes levelScenes;
    [Tooltip("��������")]
    [SerializeField] private int selectLevelScenesIndex;

    [Space]

    [Tooltip("�������ƣ�����������")]
    [SerializeField] private string selectLevelScenesName;

    private void Start()
    {
        // ʹ��ѡ������������ȡ�ؿ���������
        selectLevelScenesName = levelScenes.levelScenesName[selectLevelScenesIndex];
    }

    public void LevelTransition()
    {
        // �л��ؿ�
        SceneManager.LoadScene(selectLevelScenesName);
    }
}
