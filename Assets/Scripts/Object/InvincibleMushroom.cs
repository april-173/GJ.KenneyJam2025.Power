using UnityEngine;
using System.Collections;

public class InvincibleMushroom : MonoBehaviour
{
    [Header("物体互动")]
    [Tooltip("与玩家接触情况")]
    public bool isTrigger;

    [Header("关联物体")]
    [Tooltip("玩家控制器")]
    public PlayerController playerController;
    [Tooltip("玩家精灵渲染器")]
    public SpriteRenderer playerSpriteRenderer;



    private void Start()
    {
        // 若玩家控制器脚本为空
        if (playerController == null)
        {
            // 在场景中搜索玩家并获取其PlayerController组件
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

    // 颜色渐变协程
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

    #region< 碰撞相关 >

    // 当其它碰撞器进入碰撞器范围内时触发
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 若该碰撞器为玩家碰撞器
        if (other.CompareTag("Player"))
        {
            // 与玩家接触
            isTrigger = true;

        }
    }

    // 当其它碰撞器离开碰撞器范围内时触发
    private void OnTriggerExit2D(Collider2D other)
    {
        // 若该碰撞器为玩家碰撞器
        if (other.CompareTag("Player"))
        {
            // 与玩家未接触
            isTrigger = false;

        }
    }

    #endregion
}
