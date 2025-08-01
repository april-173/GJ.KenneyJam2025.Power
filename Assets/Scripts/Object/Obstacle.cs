using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// �ϰ�����
/// </summary>
public enum ObstacleType
{
    None,
    SolidWall,
    FragileWall,
    LinkageWall,
    RestrictedRZone
}

public class Obstacle : MonoBehaviour
{
    [Header("ͼ����ʾ")]
    [Tooltip("������Ⱦ�����")]
    public SpriteRenderer obstacleSpriteRenderer;
    [Tooltip("��Ҫ��ʾ�ľ���ͼƬ")]
    public Sprite obstacleSprite;

    [Header("�ϰ�����")]
    [Tooltip("�ϰ�����")]
    public ObstacleType obstacleType;

    [Header("���廥��")]
    [Tooltip("����ҽӴ����")]
    public bool isTrigger;

    [Header("��������")]
    [Tooltip("��������")]
    public GameObject linkageObject;
    [Tooltip("��ҿ�����")]
    public PlayerController playerController;


    private void Start()
    {
        // ����ҿ������ű�Ϊ��
        if (playerController == null)
        {
            // �ڳ�����������Ҳ���ȡ��PlayerController���
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        // ��ʼ��������Ⱦ��
        obstacleSpriteRenderer = GetComponent<SpriteRenderer>();

        // �����ϰ����Ͳ�ͬѡ��ͬ����ʾ��ʽ
        switch(obstacleType)
        {
            case ObstacleType.None:
                DestroyObstacle();
                break;
            case ObstacleType.SolidWall:
                obstacleSpriteRenderer.sprite = obstacleSprite;
                break;
            case ObstacleType.FragileWall:
                obstacleSpriteRenderer.sprite = obstacleSprite;
                break;
            case ObstacleType.LinkageWall:
                obstacleSpriteRenderer.sprite = obstacleSprite;
                break;
            case ObstacleType.RestrictedRZone:
                obstacleSpriteRenderer.sprite = obstacleSprite;
                break;
            default:
                DestroyObstacle();
                break;
        }

        if(linkageObject == null)
        {
            linkageObject = this.gameObject;
        }
    }

    private void Update()
    {
        // �����ײ�������ϰ���ײ���Ӵ�
        if(isTrigger)
        {
            // �����ϰ����͵Ĳ�ͬӦ�ò�ͬ�ķ���
            switch(obstacleType)
            {
                case ObstacleType.None:
                    break;
                case ObstacleType.SolidWall:
                    break;
                case ObstacleType.FragileWall:
                    FragileWall();
                    break;
                case ObstacleType.LinkageWall:
                    LinkageWall();
                    break;
                case ObstacleType.RestrictedRZone:
                    RestrictedRZone();
                    break;
                default:
                    break;
            }
        }
    }

    #region < ����Ч�� >
    /// <summary>
    /// ����ǽЧ��
    /// </summary>
    private void FragileWall()
    {
        // ��ҽ���ײ��ʱ����ǽ��
        DestroyObstacle();
    }

    /// <summary>
    /// ����ǽЧ��
    /// </summary>
    private void LinkageWall()
    {
        // ������ƶ�ǰ������������ǽ��������������һ��
        if (playerController.originalPosition == linkageObject.transform.position) 
        {
            // ײ��ʱ��������
            DestroyObstacle();
        }
    }

    private void RestrictedRZone()
    {
        if (playerController.isInvincible)
        {
            return;
        }
        else
        {
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.02f);

        // ���¼��ص�ǰ�����
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);

        yield return null;
    }

    #endregion

    #region< ��ײ��� >

    // ��������ײ��������ײ����Χ��ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ������ײ��Ϊ�����ײ��
        if (other.CompareTag("Player"))
        {
            // ����ҽӴ�
            isTrigger = true;

        }
    }

    // ��������ײ���뿪��ײ����Χ��ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        // ������ײ��Ϊ�����ײ��
        if (other.CompareTag("Player"))
        {
            // �����δ�Ӵ�
            isTrigger = false;

        }
    }

    #endregion

    #region < ��Ʒ���� >

    /// <summary>
    /// �����ϰ���
    /// </summary>
    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }

    #endregion
}
