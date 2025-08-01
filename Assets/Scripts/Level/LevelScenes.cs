using UnityEngine;


[CreateAssetMenu(fileName = "NewLevelScenes", menuName = "Level Scenes")]
public class LevelScenes : ScriptableObject
{
    [Header("场景名称管理器")]
    [Tooltip("场景名称数组")]
    public string[] levelScenesName;
}
