using UnityEngine;

public class CollectionInWorld : MonoBehaviour
{
    [Header("�ռ�Ʒ��Ϣ")]
    [Tooltip("�ռ�Ʒ������")]
    public CollectionManager collectionManager;
    [Tooltip("�ռ�Ʒ����")]
    public int collectionIndex;

    [Header("���廥��")]
    [Tooltip("����ҽӴ����")]
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

    #region< ��ײ��� >

    // ��������ײ��������ײ����Χ��ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ������ײ��Ϊ�����ײ��
        if (other.CompareTag("Player"))
        {
            // ����ҽӴ�
            isTrigger = true;

        }
    }

    // ��������ײ���뿪��ײ����Χ��ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        // ������ײ��Ϊ�����ײ��
        if (other.CompareTag("Player"))
        {
            // �����δ�Ӵ�
            isTrigger = false;

        }
    }

    #endregion
}
