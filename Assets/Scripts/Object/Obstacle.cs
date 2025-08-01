using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 障碍类型
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
    [Header("图像显示")]
    [Tooltip("精灵渲染器组件")]
    public SpriteRenderer obstacleSpriteRenderer;
    [Tooltip("需要显示的精灵图片")]
    public Sprite obstacleSprite;

    [Header("障碍属性")]
    [Tooltip("障碍类型")]
    public ObstacleType obstacleType;

    [Header("物体互动")]
    [Tooltip("与玩家接触情况")]
    public bool isTrigger;

    [Header("关联物体")]
    [Tooltip("联动物体")]
    public GameObject linkageObject;
    [Tooltip("玩家控制器")]
    public PlayerController playerController;


    private void Start()
    {
        // 若玩家控制器脚本为空
        if (playerController == null)
        {
            // 在场景中搜索玩家并获取其PlayerController组件
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        // 初始化精灵渲染器
        obstacleSpriteRenderer = GetComponent<SpriteRenderer>();

        // 根据障碍类型不同选择不同的显示方式
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
        // 若玩家撞击点与障碍碰撞器接触
        if(isTrigger)
        {
            // 根据障碍类型的不同应用不同的方法
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

    #region < 具体效果 >
    /// <summary>
    /// 脆弱墙效果
    /// </summary>
    private void FragileWall()
    {
        // 玩家进行撞击时销毁墙面
        DestroyObstacle();
    }

    /// <summary>
    /// 联动墙效果
    /// </summary>
    private void LinkageWall()
    {
        // 若玩家移动前的坐标与联动墙的联动物体坐标一致
        if (playerController.originalPosition == linkageObject.transform.position) 
        {
            // 撞击时销毁物体
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

        // 重新加载当前活动场景
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);

        yield return null;
    }

    #endregion

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

    #region < 物品销毁 >

    /// <summary>
    /// 销毁障碍物
    /// </summary>
    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }

    #endregion
}
