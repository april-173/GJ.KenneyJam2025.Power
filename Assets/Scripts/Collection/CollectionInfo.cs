using TMPro;
using UnityEngine;

public class CollectionInfo : MonoBehaviour
{
    [Header("收集品信息")]
    [Tooltip("收集品管理器")]
    public CollectionManager collectionManager;

    [Header("信息显示文本")]
    [Tooltip("信息显示文本")]
    public TextMeshProUGUI collectionText;

    private void Update()
    {
        collectionText.text = new string($"X {string.Format("{0:D2}", collectionManager.collectionQuantity)}");
    }

    // 重置收集品管理器
    public void ResetCollectionManager()
    {
        for (int i = 0; i < collectionManager.iscollected.Count; i++)
        {
            collectionManager.iscollected[i] = false;
        }

        collectionManager.collectionQuantity = 0;
    }    
}
