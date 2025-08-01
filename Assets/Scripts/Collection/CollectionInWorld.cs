using UnityEngine;

public class CollectionInWorld : MonoBehaviour
{
    [Header("收集品信息")]
    [Tooltip("收集品管理器")]
    public CollectionManager collectionManager;
    [Tooltip("收集品索引")]
    public int collectionIndex;

    [Header("物体互动")]
    [Tooltip("与玩家接触情况")]
    public bool isTrigger;

    private void Start()
    {
        gameObject.SetActive(!collectionManager.iscollected[collectionIndex]);
    }

    private void Update()
    {
        if (isTrigger)
        {
            collectionManager.iscollected[collectionIndex] = true;
            collectionManager.collectionQuantity += 1;

            gameObject.SetActive(false);
        }
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
