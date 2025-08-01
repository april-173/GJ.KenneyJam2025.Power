using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCollectionManager", menuName = "Collection Manager")]
public class CollectionManager : ScriptableObject
{
    [Header("ȫ���ռ�Ʒ���ɹ���")]
    [Tooltip("�ռ�Ʒ�ռ��������")]
    public List<bool> iscollected = new List<bool>();

    [Tooltip("�ռ�Ʒ���ռ���Ŀ")]
    public int collectionQuantity;
}
