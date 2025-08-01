using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;     // ����������
    private Collider2D col;     // ��ײ�����

    [Tooltip("������𶯽ű�")]
    public CameraShake cameraShake;

    [Header("�������")]
    [Tooltip("������뷽��")]
    [SerializeField] private Vector2 moveInput;

    [Header("�������")]
    [Tooltip("�����С")]
    public Vector2 checkPointSize;
    [Tooltip("���ͼ��")]
    public LayerMask checkLayer;
    [Tooltip("�ϲ����")]
    public Transform upCheckPoint;
    [Tooltip("�²����")]
    public Transform downCheckPoint;
    [Tooltip("������")]
    public Transform leftCheckPoint;
    [Tooltip("�Ҳ����")]
    public Transform rightCheckPoint;

    [Space]

    [Tooltip("�ϲ�ؿ��ǩ")]
    [SerializeField] private string upPlotTag;
    [Tooltip("�²�ؿ��ǩ")]
    [SerializeField] private string downPlotTag;
    [Tooltip("���ؿ��ǩ")]
    [SerializeField] private string leftPlotTag;
    [Tooltip("�ϲ�ؿ��ǩ")]
    [SerializeField] private string rightPlotTag;

    [Space]

    [Tooltip("�ϲ����")]
    [SerializeField] private Vector3 upDistance;
    [Tooltip("�²����")]
    [SerializeField] private Vector3 downDistance;
    [Tooltip("������")]
    [SerializeField] private Vector3 leftDistance;
    [Tooltip("�ϲ����")]
    [SerializeField] private Vector3 rightDistance;

    [Header("�ϰ����")]
    [Tooltip("�ϰ������С")]
    public Vector2 obstacleCheckPointSize;
    [Tooltip("�ϰ����ͼ��")]
    public LayerMask obstacleCheckLayer;

    [Space]

    [Tooltip("�ϲ��ϰ�����")]
    [SerializeField] private Vector3 upObstacleDistance;
    [Tooltip("�²��ϰ�����")]
    [SerializeField] private Vector3 downObstacleDistance;
    [Tooltip("����ϰ�����")]
    [SerializeField] private Vector3 leftObstacleDistance;
    [Tooltip("�ϲ��ϰ�����")]
    [SerializeField] private Vector3 rightObstacleDistance;

    [Header("�ƶ�Ȩ��")]
    [Tooltip("�����ƶ�")]
    public bool canMove;

    [Space]

    [Tooltip("���������ƶ�")]
    [SerializeField] private bool canUpMove;
    [Tooltip("���������ƶ�")]
    [SerializeField] private bool canDownMove;
    [Tooltip("���������ƶ�")]
    [SerializeField] private bool canLeftMove;
    [Tooltip("���������ƶ�")]
    [SerializeField] private bool canRightMove;

    [Header("�ƶ����")]
    [Tooltip("�ƶ�����ٶ�")]
    public float moveMaxSpeed;
    [Tooltip("�ƶ����ٶ�")]
    public float moveAcceleration;
    [Tooltip("�ƶ�����")]
    public Vector3 moveDirection;

    [Space]

    [Tooltip("ԭʼλ�ã�ÿ���ƶ����и��£�")]
    public Vector3 originalPosition;
    [Tooltip("Ŀ��λ�ã�ÿ���ƶ����и��£�")]
    [SerializeField] private Vector3 targetPosition;

    [Header("ײ�����")]
    [Tooltip("��ײ����")]
    public GameObject upStrikePoint;
    [Tooltip("��ײ����")]
    public GameObject downStrikePoint;
    [Tooltip("��ײ����")]
    public GameObject leftStrikePoint;
    [Tooltip("��ײ����")]
    public GameObject rightStrikePoint;

    [Header("��Ч����")]
    [Tooltip("��Ч������")]
    public AudioManager audioManager;

    [Header("�ؿ�����")]
    [Tooltip("�޵�״̬")]
    public bool isInvincible;

    private void Start()
    {
        // ������������Ϊ��
        if(cameraShake ==  null)
        {
            // �ڳ������������������ȡ��CameraShake���
            cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        }

        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // ײ�������
        ForbiddenStrikePoint();
    }

    private void Update()
    {
        GetInput();
        PlotCheckSystem();
        DistanceMeasurementSystem();
        ObstacleDistanceMeasurementSystem();
        MoveAuthorityManager();
        MoveControllerSystem();
    }

    #region < ��ȡ������� >

    /// <summary>
    ///��ȡ�������
    /// </summary>
    private void GetInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");    // ��ȡ���ˮƽ�ƶ�����
        moveInput.y = Input.GetAxisRaw("Vertical");      // ��ȡ��Ҵ�ֱ�ƶ�����

    }

    #endregion

    #region < �ؿ���ϵͳ >

    /// <summary>
    /// �ؿ���ϵͳ
    /// </summary>
    private void PlotCheckSystem()
    {
        // ��ȡ����ĸ���λ�Ӵ�����ײ�����
        Collider2D upTouchCol = PlotColliderAcquisition(upCheckPoint);
        Collider2D downTouchCol = PlotColliderAcquisition(downCheckPoint);
        Collider2D leftTouchCol = PlotColliderAcquisition(leftCheckPoint);
        Collider2D rightTouchCol = PlotColliderAcquisition(rightCheckPoint);

        // ��ȡ����ĸ���λ�Ӵ��ĵؿ���ײ���ı�ǩ
        upPlotTag = PlotTagAcquisition(upTouchCol);
        downPlotTag = PlotTagAcquisition(downTouchCol);
        leftPlotTag = PlotTagAcquisition(leftTouchCol);
        rightPlotTag = PlotTagAcquisition(rightTouchCol);

    }

    /// <summary>
    /// �ؿ���ײ����ȡ
    /// </summary>
    /// <param name="checkPoint">����λ��</param>
    /// <returns>����Ӵ�����ײ��</returns>
    private Collider2D PlotColliderAcquisition(Transform checkPoint)
    {
        // ��ȡ�� checkPoint.position Ϊ���ģ� checkPointSize Ϊ��Χ�� 0 Ϊ��ת�Ƕȣ�ֻ������ checkLayer ͼ��ĽӴ�������ײ�����
        Collider2D TouchCol = Physics2D.OverlapBox(
            checkPoint.position,
            checkPointSize,
            0,
            checkLayer
            );

        return TouchCol;
    }

    /// <summary>
    /// �ؿ��ǩ��ȡ
    /// </summary>
    /// <param name="Col">�ؿ���ײ�����</param>
    /// <returns>�ؿ��ǩ</returns>
    private string PlotTagAcquisition(Collider2D plotCol)
    {
        // ��ȡ��ҽӴ�����ײ��Tag
        if (plotCol != null)
        {
            return plotCol.tag;
        }
        // ���δ�Ӵ���ײ��ʱ����"UnpassableRoad������ͨ�еؿ飩"
        else
        {
            return "null";
        }
    }

    #endregion

    #region < �������ϵͳ >

    /// <summary>
    /// �������ϵͳ
    /// </summary>
    private void DistanceMeasurementSystem()
    {
        // ��ȡ���������ĸ�������ƶ����ľ���
        upDistance = DistanceAcquisition(upCheckPoint, upPlotTag, Vector3.up);
        downDistance = DistanceAcquisition(downCheckPoint, downPlotTag, Vector3.down);
        leftDistance = DistanceAcquisition(leftCheckPoint, leftPlotTag, Vector3.left);
        rightDistance = DistanceAcquisition(rightCheckPoint, rightPlotTag, Vector3.right);
    }

    /// <summary>
    /// �����ȡ
    /// </summary>
    /// <param name="checkPoint">���㣨��׼�㣩</param>
    /// <param name="plotTag">���������ĵؿ��ǩ</param>
    /// <param name="direction">��鷽��</param>
    /// <returns></returns>
    private Vector3 DistanceAcquisition(Transform checkPoint, string plotTag, Vector3 direction)
    {
        // ��ȡ��׼��
        Transform point = checkPoint;
        // ��ȡ��׼��ؿ��ǩ
        string tag = plotTag;

        // ����׼��ؿ��ǩΪ��Roadway��
        if (tag == "Roadway")
        {
            // ѭ������鷽��ĵؿ��ǩ��ÿһ��ѭ������λ��ǰ��1
            for (int i = 1; tag == "Roadway"; i++)
            {
                // ÿһ�μ��ʵ�ʼ����λ��ǰ��1
                // ��ȡ�ؿ��ǩ
                tag = PlotTagAcquisition(PlotColliderAcquisitionByDistance(checkPoint.position + i * direction));

                // ��ǰ��������������ǩ��Ϊ��Roadway���ĵؿ飬���ظõؿ��ǰһ���ؿ�λ��
                if (tag != "Roadway")
                {
                    return checkPoint.position + (i - 1) * direction;
                }
            }
        }
        // ����׼��ؿ��ǩ��Ϊ��Roadway��
        else
        {
            // �������λ��
            return transform.position;
        }

        // �������λ��
        return transform.position;
    }

    /// <summary>
    /// �ؿ���ײ����ȡ�������룩
    /// </summary>
    /// <param name="checkPoint">����λ��</param>
    /// <returns>����Ӵ�����ײ��</returns>
    private Collider2D PlotColliderAcquisitionByDistance(Vector3 checkPoint)
    {
        // ��ȡ�� checkPoint.position + i * direction Ϊ���ģ� checkPointSize Ϊ��Χ�� 0 Ϊ��ת�Ƕȣ�ֻ������ checkLayer ͼ��ĽӴ�������ײ�����
        Collider2D touchCol = Physics2D.OverlapBox(
            checkPoint,
            checkPointSize,
            0,
            checkLayer
            );

        return touchCol;
    }


    #endregion

    #region < �ϰ����ϵͳ >

    /// <summary>
    /// �ϰ��������ϵͳ
    /// </summary>
    private void ObstacleDistanceMeasurementSystem()
    {
        // ��ȡ���������ĸ���������ϰ���ľ���
        upObstacleDistance = ObstacleDistanceAcquisition(upCheckPoint, Vector3.up, (int)Mathf.Abs((upDistance.y)-(transform.position.y)));
        downObstacleDistance = ObstacleDistanceAcquisition(downCheckPoint, Vector3.down, (int)Mathf.Abs((transform.position.y)-(downDistance.y)));
        leftObstacleDistance = ObstacleDistanceAcquisition(leftCheckPoint, Vector3.left, (int)Mathf.Abs((transform.position.x)- (leftDistance.x)));
        rightObstacleDistance = ObstacleDistanceAcquisition(rightCheckPoint, Vector3.right, (int)Mathf.Abs((rightDistance.x) - (transform.position.x)));

        //if (upDistance == transform.position) 
        //{
        //    upObstacleDistance = upDistance;
        //}
        //if (downDistance == transform.position) 
        //{
        //    downObstacleDistance = downDistance;
        //}
        //if (leftDistance == transform.position)
        //{
        //    leftObstacleDistance = leftDistance;
        //}
        //if (rightDistance == transform.position)
        //{
        //    rightObstacleDistance = rightDistance;
        //}
    }

    /// <summary>
    /// �ϰ�������ȡ
    /// </summary>
    /// <param name="checkPoint">���㣨��׼��</param>
    /// <param name="direction">��鷽��</param>
    /// <param name="checkDistance">������</param>
    /// <returns></returns>
    private Vector3 ObstacleDistanceAcquisition(Transform checkPoint, Vector3 direction, int checkDistance)
    {
        // ��ʼ��һ����ײ�������洢��ȡ����ײ�����
        Collider2D touchCol = null;

        // ���ݼ�����ѭ�����Ŀ����ײ����ÿ��ѭ���������+1��
        for (int i = 0; i <= checkDistance; i++)
        {
            // ��ȡ��ײ�����
            touchCol = ObstacleColliderAcquisitionByDistance(checkPoint.position + i * direction);

            // ����ȡ����Ŀ����ײ��
            if (touchCol != null)
            {
                // ���ظü�⵽��ײ���ļ������һ������
                return checkPoint.position + (i - 1) * direction;
            }
        }

        // ��δ��ȡ����ײ�������ϰ������Ϊ�������ǽ��ľ���
        if (direction == Vector3.up)
        { return upDistance; }
        if (direction == Vector3.down)
        { return downDistance; }
        if (direction == Vector3.left)
        { return leftDistance; }
        if (direction == Vector3.right)
        { return rightDistance; }

        // ��������λ��
        return transform.position;
    }

    /// <summary>
    /// �ϰ���ײ����ȡ�������룩
    /// </summary>
    /// <param name="checkPoint">����λ��</param>
    /// <returns>����Ӵ�����ײ��</returns>
    private Collider2D ObstacleColliderAcquisitionByDistance(Vector3 checkPoint)
    {
        // ��ȡ�� checkPoint Ϊ���ģ� obstacleCheckPointSize Ϊ��Χ�� 0 Ϊ��ת�Ƕȣ�ֻ������ obstacleCheckLayer ͼ��ĽӴ�������ײ�����
        Collider2D touchCol = Physics2D.OverlapBox(
            checkPoint,
            obstacleCheckPointSize,
            0,
            obstacleCheckLayer
            );

        return touchCol;
    }

    #endregion

    #region < �ƶ�Ȩ�޹��� >

    /// <summary>
    /// �ƶ�Ȩ�޹���
    /// </summary>
    private void MoveAuthorityManager()
    {
        // ����������ƶ�ʱ���м��
        if (canMove)
        {
            // ���ƶ����벻Ϊ0ʱ�����ƶ�
            canUpMove = (upObstacleDistance == transform.position) ? false : true;
            canDownMove = (downObstacleDistance == transform.position) ? false : true;
            canLeftMove = (leftObstacleDistance == transform.position) ? false : true;
            canRightMove = (rightObstacleDistance == transform.position) ? false : true;
        }
        else
        {
            canUpMove = false;
            canDownMove = false;
            canLeftMove = false;
            canRightMove = false;
        }
    }

    #endregion

    #region < �ƶ�����ϵͳ >

    /// <summary>
    /// �ƶ�����ϵͳ
    /// </summary>
    private void MoveControllerSystem()
    {
        if (canMove)
        {
            // ��ȡ�ƶ�����
            moveDirection = MoveDirectionAcquisition();
            // ��ȡĿ��λ��
            targetPosition = TargetPointAcquisition();

            if (moveDirection != Vector3.zero)
            {
                // �����ƶ�Э��
                StartCoroutine(Move());
            }
        }
    }

    /// <summary>
    /// �ƶ������ȡ
    /// </summary>
    /// <returns>�ƶ�����</returns>
    private Vector2 MoveDirectionAcquisition()
    {
        // ��ȡ������뷽��
        Vector2 direction = moveInput;

        // ����ƶ�Ȩ��
        // ĳ����û���ƶ�Ȩ��ʱ�޷����и÷�����ƶ�
        if (direction.y > 0 && !canUpMove) direction.y = 0;
        if (direction.y < 0 && !canDownMove) direction.y = 0;
        if (direction.x < 0 && !canLeftMove) direction.x = 0;
        if (direction.x > 0 && !canRightMove) direction.x = 0;

        // ����ʵ�ʵ��ƶ�����
        return direction;
    }

    /// <summary>
    /// Ŀ��λ�û�ȡ
    /// </summary>
    /// <returns>Ŀ��λ��</returns>
    private Vector2 TargetPointAcquisition()
    {
        // �����ƶ����򷵻ض�Ӧ������ϰ������ΪĿ��λ�ò����ö�Ӧ�����ײ����
        if (moveDirection.y != 0)
        {
            if (moveDirection.y > 0) 
            {
                upStrikePoint.SetActive(true);
                return upObstacleDistance;
            }

            if (moveDirection.y < 0) 
            {
                downStrikePoint.SetActive(true);
                return downObstacleDistance;
            }
        }

        if (moveDirection.x != 0) 
        {
            if (moveDirection.x > 0) 
            {
                rightStrikePoint.SetActive(true);
                return rightObstacleDistance;
            }

            if (moveDirection.x < 0) 
            {
                leftStrikePoint.SetActive(true);
                return leftObstacleDistance;
            }
        }

        return transform.position;
    }    

    /// <summary>
    /// �ƶ�Э��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move()
    {
        if(!canMove) yield break;
        canMove = false;

        //audioManager.PlaySfx(audioManager.slide);

        float currentSpeed = 0f;
        originalPosition = transform.position;
        Vector3 currentPosition = originalPosition;

        float totalDistance = Vector3.Distance(originalPosition, targetPosition);
        float traveledDistance = 0f;

        while (traveledDistance < totalDistance)
        {
            // ���㵱ǰ֡��������ƶ����루��ֹ���壩
            float remainingDistance = totalDistance - traveledDistance;
            float maxMoveThisFrame = currentSpeed * Time.deltaTime;

            // Ӧ�ü��ٶȣ���������ʣ�����������ٶȣ�
            currentSpeed = Mathf.Min(currentSpeed + moveAcceleration * Time.deltaTime, moveMaxSpeed);

            // ����ʵ���ƶ����루��ֹ����Ŀ�꣩
            float moveDistance = Mathf.Min(maxMoveThisFrame, remainingDistance);

            // ����λ�ú����ƶ�����
            currentPosition += moveDirection * moveDistance;
            traveledDistance += moveDistance;
            transform.position = currentPosition;

            // ����Ѿ���������ǰ�˳�
            if (traveledDistance >= totalDistance) break;

            // �ȴ���һ֡
            yield return null;
        }

        //audioManager.PlaySfx(audioManager.strike);
        // ȷ����ȷͣ��Ŀ��λ��
        transform.position = targetPosition;
        // �������
        cameraShake.TriggerShake();

        // �ȴ�0.05��
        yield return new WaitForSeconds(0.2f);
        // ����ײ����
        ForbiddenStrikePoint();
        // �����ƶ�
        canMove = true;
        // ����޵�״̬
        isInvincible = false;

        yield return null;
    }

    #endregion

    #region < ���ײ��ϵͳ >

    private void ForbiddenStrikePoint()
    {
        // ײ�������
        upStrikePoint.SetActive(false);
        downStrikePoint.SetActive(false);
        leftStrikePoint.SetActive(false);
        rightStrikePoint.SetActive(false);
    }


    #endregion
}
