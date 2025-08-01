using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCollectionManager", menuName = "Collection Manager")]
public class CollectionManager : ScriptableObject
{
    [Header("全局收集品集成管理")]
    [Tooltip("收集品收集情况链表")]
    public List<bool> iscollected = new List<bool>();

    [Tooltip("收集品已收集数目")]
    public int collectionQuantity;
}
