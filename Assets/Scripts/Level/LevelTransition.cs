using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    [Tooltip("��ҿ�����")]
    public PlayerController playerController;
    [Tooltip("�ؿ�ѡ����ײ��")]
    public Collider2D levelSelectcollider;

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
        // ��ʼ����ײ��
        levelSelectcollider = GetComponent<Collider2D>();

        // ʹ��ѡ������������ȡ�ؿ���������
        selectLevelScenesName = levelScenes.levelScenesName[selectLevelScenesIndex]; 
    }

    /// <summary>
    ///  �����ؿ�ѡ����ײ���Ӵ�ʱ����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���Ӵ�����ײ����ǩΪ"Player"
        if(other.gameObject.CompareTag("Player"))
        {
            // �����ؿ��л�Э��
            StartCoroutine(LevelTransitionCoroutine());
        }
    }

    /// <summary>
    /// �ؿ��л�Э��
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelTransitionCoroutine()
    {
        // �ȴ������ֹͣ�ƶ����ٽ�����һ��
        while(!playerController.canMove)
        {
            // �ȴ�
            yield return new WaitForSeconds(0.05f);
        }

        // ��������ƶ�
        playerController.canMove = false;
        // �ȴ�
        yield return new WaitForSeconds(0.1f);
        // �л��ؿ�
        SceneManager.LoadScene(selectLevelScenesName);

        yield return null;
    }
}
