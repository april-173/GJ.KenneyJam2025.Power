using TMPro;
using UnityEngine;

public class CollectionInfo : MonoBehaviour
{
    [Header("�ռ�Ʒ��Ϣ")]
    [Tooltip("�ռ�Ʒ������")]
    public CollectionManager collectionManager;

    [Header("��Ϣ��ʾ�ı�")]
    [Tooltip("��Ϣ��ʾ�ı�")]
    public TextMeshProUGUI collectionText;

    private void Update()
    {
        collectionText.text = new string($"X {string.Format("{0:D2}", collectionManager.collectionQuantity)}");
    }

    // �����ռ�Ʒ������
    public void ResetCollectionManager()
    {
        for (int i = 0; i < collectionManager.iscollected.Count; i++)
        {
            collectionManager.iscollected[i] = false;
        }

        collectionManager.collectionQuantity = 0;
    }    
}
