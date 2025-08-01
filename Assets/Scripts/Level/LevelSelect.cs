using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
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
        // 使用选定的索引来获取关卡场景名称
        selectLevelScenesName = levelScenes.levelScenesName[selectLevelScenesIndex];
    }

    public void LevelTransition()
    {
        // 切换关卡
        SceneManager.LoadScene(selectLevelScenesName);
    }
}
