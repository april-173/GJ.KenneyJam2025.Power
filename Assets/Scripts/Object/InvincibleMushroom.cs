using UnityEngine;
using System.Collections;

public class InvincibleMushroom : MonoBehaviour
{
    [Header("���廥��")]
    [Tooltip("����ҽӴ����")]
    public bool isTrigger;

    [Header("��������")]
    [Tooltip("��ҿ�����")]
    public PlayerController playerController;
    [Tooltip("��Ҿ�����Ⱦ��")]
    public SpriteRenderer playerSpriteRenderer;



    private void Start()
    {
        // ����ҿ������ű�Ϊ��
        if (playerController == null)
        {
            // �ڳ�����������Ҳ���ȡ��PlayerController���
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        if (isTrigger)
        {
            if(!playerController.isInvincible)
            {
                playerController.isInvincible = true;

                StartCoroutine(FadeColorRoutine());
            }
        }
    }

    // ��ɫ����Э��
    private IEnumerator FadeColorRoutine()
    {
        while(!playerController.canMove)
        {
            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0.75f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0.5f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0.25f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0.25f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0.5f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 0.75f);

            yield return null;

            playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, 1f);
        }

        yield return null;
    }

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
}
