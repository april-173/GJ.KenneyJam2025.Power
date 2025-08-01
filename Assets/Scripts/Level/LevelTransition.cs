using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    [Tooltip("玩家控制器")]
    public PlayerController playerController;
    [Tooltip("关卡选择碰撞器")]
    public Collider2D levelSelectcollider;

    [Header("场景选择")]
    [Tooltip("场景名称管理器")]
    [SerializeField] private LevelScenes levelScenes;
    [Tooltip("场景索引")]
    [SerializeField] private int selectLevelScenesIndex;

    [Space]

    [Tooltip("场景名称（按场景索引")]
    [SerializeField] private string selectLevelScenesName;

    private void Start()
    {
        // 初始化碰撞器
        levelSelectcollider = GetComponent<Collider2D>();

        // 使用选定的索引来获取关卡场景名称
        selectLevelScenesName = levelScenes.levelScenesName[selectLevelScenesIndex]; 
    }

    /// <summary>
    ///  玩家与关卡选择碰撞器接触时触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 若接触的碰撞器标签为"Player"
        if(other.gameObject.CompareTag("Player"))
        {
            // 开启关卡切换协程
            StartCoroutine(LevelTransitionCoroutine());
        }
    }

    /// <summary>
    /// 关卡切换协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelTransitionCoroutine()
    {
        // 等待至玩家停止移动后再进行下一步
        while(!playerController.canMove)
        {
            // 等待
            yield return new WaitForSeconds(0.05f);
        }

        // 禁用玩家移动
        playerController.canMove = false;
        // 等待
        yield return new WaitForSeconds(0.1f);
        // 切换关卡
        SceneManager.LoadScene(selectLevelScenesName);

        yield return null;
    }
}
