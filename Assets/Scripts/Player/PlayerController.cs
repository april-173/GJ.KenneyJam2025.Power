using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;     // 物理刚体组件
    private Collider2D col;     // 碰撞体组件

    [Tooltip("摄像机震动脚本")]
    public CameraShake cameraShake;

    [Header("玩家输入")]
    [Tooltip("玩家输入方向")]
    [SerializeField] private Vector2 moveInput;

    [Header("环境检测")]
    [Tooltip("检测点大小")]
    public Vector2 checkPointSize;
    [Tooltip("检测图层")]
    public LayerMask checkLayer;
    [Tooltip("上侧检测点")]
    public Transform upCheckPoint;
    [Tooltip("下侧检测点")]
    public Transform downCheckPoint;
    [Tooltip("左侧检测点")]
    public Transform leftCheckPoint;
    [Tooltip("右侧检测点")]
    public Transform rightCheckPoint;

    [Space]

    [Tooltip("上侧地块标签")]
    [SerializeField] private string upPlotTag;
    [Tooltip("下侧地块标签")]
    [SerializeField] private string downPlotTag;
    [Tooltip("左侧地块标签")]
    [SerializeField] private string leftPlotTag;
    [Tooltip("上侧地块标签")]
    [SerializeField] private string rightPlotTag;

    [Space]

    [Tooltip("上侧距离")]
    [SerializeField] private Vector3 upDistance;
    [Tooltip("下侧距离")]
    [SerializeField] private Vector3 downDistance;
    [Tooltip("左侧距离")]
    [SerializeField] private Vector3 leftDistance;
    [Tooltip("上侧距离")]
    [SerializeField] private Vector3 rightDistance;

    [Header("障碍检测")]
    [Tooltip("障碍检测点大小")]
    public Vector2 obstacleCheckPointSize;
    [Tooltip("障碍检测图层")]
    public LayerMask obstacleCheckLayer;

    [Space]

    [Tooltip("上侧障碍距离")]
    [SerializeField] private Vector3 upObstacleDistance;
    [Tooltip("下侧障碍距离")]
    [SerializeField] private Vector3 downObstacleDistance;
    [Tooltip("左侧障碍距离")]
    [SerializeField] private Vector3 leftObstacleDistance;
    [Tooltip("上侧障碍距离")]
    [SerializeField] private Vector3 rightObstacleDistance;

    [Header("移动权限")]
    [Tooltip("允许移动")]
    public bool canMove;

    [Space]

    [Tooltip("允许向上移动")]
    [SerializeField] private bool canUpMove;
    [Tooltip("允许向下移动")]
    [SerializeField] private bool canDownMove;
    [Tooltip("允许向左移动")]
    [SerializeField] private bool canLeftMove;
    [Tooltip("允许向右移动")]
    [SerializeField] private bool canRightMove;

    [Header("移动相关")]
    [Tooltip("移动最大速度")]
    public float moveMaxSpeed;
    [Tooltip("移动加速度")]
    public float moveAcceleration;
    [Tooltip("移动方向")]
    public Vector3 moveDirection;

    [Space]

    [Tooltip("原始位置（每次移动进行更新）")]
    public Vector3 originalPosition;
    [Tooltip("目标位置（每次移动进行更新）")]
    [SerializeField] private Vector3 targetPosition;

    [Header("撞击相关")]
    [Tooltip("上撞击点")]
    public GameObject upStrikePoint;
    [Tooltip("下撞击点")]
    public GameObject downStrikePoint;
    [Tooltip("左撞击点")]
    public GameObject leftStrikePoint;
    [Tooltip("右撞击点")]
    public GameObject rightStrikePoint;

    [Header("音效管理")]
    [Tooltip("音效管理器")]
    public AudioManager audioManager;

    [Header("关卡机制")]
    [Tooltip("无敌状态")]
    public bool isInvincible;

    private void Start()
    {
        // 若摄像机震动组件为空
        if(cameraShake ==  null)
        {
            // 在场景中搜索摄像机并获取其CameraShake组件
            cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        }

        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // 撞击点禁用
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

    #region < 获取玩家输入 >

    /// <summary>
    ///获取玩家输入
    /// </summary>
    private void GetInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");    // 获取玩家水平移动输入
        moveInput.y = Input.GetAxisRaw("Vertical");      // 获取玩家垂直移动输入

    }

    #endregion

    #region < 地块检测系统 >

    /// <summary>
    /// 地块检测系统
    /// </summary>
    private void PlotCheckSystem()
    {
        // 获取玩家四个方位接触的碰撞器组件
        Collider2D upTouchCol = PlotColliderAcquisition(upCheckPoint);
        Collider2D downTouchCol = PlotColliderAcquisition(downCheckPoint);
        Collider2D leftTouchCol = PlotColliderAcquisition(leftCheckPoint);
        Collider2D rightTouchCol = PlotColliderAcquisition(rightCheckPoint);

        // 获取玩家四个方位接触的地块碰撞器的标签
        upPlotTag = PlotTagAcquisition(upTouchCol);
        downPlotTag = PlotTagAcquisition(downTouchCol);
        leftPlotTag = PlotTagAcquisition(leftTouchCol);
        rightPlotTag = PlotTagAcquisition(rightTouchCol);

    }

    /// <summary>
    /// 地块碰撞器获取
    /// </summary>
    /// <param name="checkPoint">检测点位置</param>
    /// <returns>检测点接触的碰撞器</returns>
    private Collider2D PlotColliderAcquisition(Transform checkPoint)
    {
        // 获取以 checkPoint.position 为中心， checkPointSize 为范围， 0 为旋转角度，只作用于 checkLayer 图层的接触到的碰撞器组件
        Collider2D TouchCol = Physics2D.OverlapBox(
            checkPoint.position,
            checkPointSize,
            0,
            checkLayer
            );

        return TouchCol;
    }

    /// <summary>
    /// 地块标签获取
    /// </summary>
    /// <param name="Col">地块碰撞器组件</param>
    /// <returns>地块标签</returns>
    private string PlotTagAcquisition(Collider2D plotCol)
    {
        // 获取玩家接触的碰撞器Tag
        if (plotCol != null)
        {
            return plotCol.tag;
        }
        // 玩家未接触碰撞器时返回"UnpassableRoad（不可通行地块）"
        else
        {
            return "null";
        }
    }

    #endregion

    #region < 距离测算系统 >

    /// <summary>
    /// 距离测算系统
    /// </summary>
    private void DistanceMeasurementSystem()
    {
        // 获取上下左右四个方向可移动到的距离
        upDistance = DistanceAcquisition(upCheckPoint, upPlotTag, Vector3.up);
        downDistance = DistanceAcquisition(downCheckPoint, downPlotTag, Vector3.down);
        leftDistance = DistanceAcquisition(leftCheckPoint, leftPlotTag, Vector3.left);
        rightDistance = DistanceAcquisition(rightCheckPoint, rightPlotTag, Vector3.right);
    }

    /// <summary>
    /// 距离获取
    /// </summary>
    /// <param name="checkPoint">检查点（基准点）</param>
    /// <param name="plotTag">检查点所处的地块标签</param>
    /// <param name="direction">检查方向</param>
    /// <returns></returns>
    private Vector3 DistanceAcquisition(Transform checkPoint, string plotTag, Vector3 direction)
    {
        // 获取基准点
        Transform point = checkPoint;
        // 获取基准点地块标签
        string tag = plotTag;

        // 若基准点地块标签为“Roadway”
        if (tag == "Roadway")
        {
            // 循环检测检查方向的地块标签，每一次循环检查的位置前进1
            for (int i = 1; tag == "Roadway"; i++)
            {
                // 每一次检测实际检查点的位置前进1
                // 获取地块标签
                tag = PlotTagAcquisition(PlotColliderAcquisitionByDistance(checkPoint.position + i * direction));

                // 若前进过程中遇到标签不为“Roadway”的地块，返回该地块的前一个地块位置
                if (tag != "Roadway")
                {
                    return checkPoint.position + (i - 1) * direction;
                }
            }
        }
        // 若基准点地块标签不为“Roadway”
        else
        {
            // 返回玩家位置
            return transform.position;
        }

        // 返回玩家位置
        return transform.position;
    }

    /// <summary>
    /// 地块碰撞器获取（按距离）
    /// </summary>
    /// <param name="checkPoint">检测点位置</param>
    /// <returns>检测点接触的碰撞器</returns>
    private Collider2D PlotColliderAcquisitionByDistance(Vector3 checkPoint)
    {
        // 获取以 checkPoint.position + i * direction 为中心， checkPointSize 为范围， 0 为旋转角度，只作用于 checkLayer 图层的接触到的碰撞器组件
        Collider2D touchCol = Physics2D.OverlapBox(
            checkPoint,
            checkPointSize,
            0,
            checkLayer
            );

        return touchCol;
    }


    #endregion

    #region < 障碍检测系统 >

    /// <summary>
    /// 障碍距离测算系统
    /// </summary>
    private void ObstacleDistanceMeasurementSystem()
    {
        // 获取上下左右四个方向最近障碍物的距离
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
    /// 障碍物距离获取
    /// </summary>
    /// <param name="checkPoint">检查点（基准点</param>
    /// <param name="direction">检查方向</param>
    /// <param name="checkDistance">检查距离</param>
    /// <returns></returns>
    private Vector3 ObstacleDistanceAcquisition(Transform checkPoint, Vector3 direction, int checkDistance)
    {
        // 初始化一个碰撞器变量存储获取的碰撞器组件
        Collider2D touchCol = null;

        // 根据检查距离循环检测目标碰撞器（每次循环检测点距离+1）
        for (int i = 0; i <= checkDistance; i++)
        {
            // 获取碰撞器组件
            touchCol = ObstacleColliderAcquisitionByDistance(checkPoint.position + i * direction);

            // 若获取到了目标碰撞器
            if (touchCol != null)
            {
                // 返回该检测到碰撞器的检测点的上一个检测点
                return checkPoint.position + (i - 1) * direction;
            }
        }

        // 若未获取到碰撞器，则障碍物距离为与最近的墙面的距离
        if (direction == Vector3.up)
        { return upDistance; }
        if (direction == Vector3.down)
        { return downDistance; }
        if (direction == Vector3.left)
        { return leftDistance; }
        if (direction == Vector3.right)
        { return rightDistance; }

        // 返回自身位置
        return transform.position;
    }

    /// <summary>
    /// 障碍碰撞器获取（按距离）
    /// </summary>
    /// <param name="checkPoint">检测点位置</param>
    /// <returns>检测点接触的碰撞器</returns>
    private Collider2D ObstacleColliderAcquisitionByDistance(Vector3 checkPoint)
    {
        // 获取以 checkPoint 为中心， obstacleCheckPointSize 为范围， 0 为旋转角度，只作用于 obstacleCheckLayer 图层的接触到的碰撞器组件
        Collider2D touchCol = Physics2D.OverlapBox(
            checkPoint,
            obstacleCheckPointSize,
            0,
            obstacleCheckLayer
            );

        return touchCol;
    }

    #endregion

    #region < 移动权限管理 >

    /// <summary>
    /// 移动权限管理
    /// </summary>
    private void MoveAuthorityManager()
    {
        // 当玩家允许移动时进行检测
        if (canMove)
        {
            // 可移动距离不为0时允许移动
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

    #region < 移动控制系统 >

    /// <summary>
    /// 移动控制系统
    /// </summary>
    private void MoveControllerSystem()
    {
        if (canMove)
        {
            // 获取移动方向
            moveDirection = MoveDirectionAcquisition();
            // 获取目标位置
            targetPosition = TargetPointAcquisition();

            if (moveDirection != Vector3.zero)
            {
                // 启动移动协程
                StartCoroutine(Move());
            }
        }
    }

    /// <summary>
    /// 移动方向获取
    /// </summary>
    /// <returns>移动方向</returns>
    private Vector2 MoveDirectionAcquisition()
    {
        // 获取玩家输入方向
        Vector2 direction = moveInput;

        // 检查移动权限
        // 某方向没有移动权限时无法进行该方向的移动
        if (direction.y > 0 && !canUpMove) direction.y = 0;
        if (direction.y < 0 && !canDownMove) direction.y = 0;
        if (direction.x < 0 && !canLeftMove) direction.x = 0;
        if (direction.x > 0 && !canRightMove) direction.x = 0;

        // 返回实际的移动方向
        return direction;
    }

    /// <summary>
    /// 目标位置获取
    /// </summary>
    /// <returns>目标位置</returns>
    private Vector2 TargetPointAcquisition()
    {
        // 根据移动方向返回对应方向的障碍物距离为目标位置并启用对应方向的撞击点
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
    /// 移动协程
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
            // 计算当前帧最大允许移动距离（防止过冲）
            float remainingDistance = totalDistance - traveledDistance;
            float maxMoveThisFrame = currentSpeed * Time.deltaTime;

            // 应用加速度（但不超过剩余距离所需的速度）
            currentSpeed = Mathf.Min(currentSpeed + moveAcceleration * Time.deltaTime, moveMaxSpeed);

            // 计算实际移动距离（防止超过目标）
            float moveDistance = Mathf.Min(maxMoveThisFrame, remainingDistance);

            // 更新位置和已移动距离
            currentPosition += moveDirection * moveDistance;
            traveledDistance += moveDistance;
            transform.position = currentPosition;

            // 如果已经到达则提前退出
            if (traveledDistance >= totalDistance) break;

            // 等待下一帧
            yield return null;
        }

        //audioManager.PlaySfx(audioManager.strike);
        // 确保精确停在目标位置
        transform.position = targetPosition;
        // 摄像机震动
        cameraShake.TriggerShake();

        // 等待0.05秒
        yield return new WaitForSeconds(0.2f);
        // 禁用撞击点
        ForbiddenStrikePoint();
        // 允许移动
        canMove = true;
        // 解除无敌状态
        isInvincible = false;

        yield return null;
    }

    #endregion

    #region < 玩家撞击系统 >

    private void ForbiddenStrikePoint()
    {
        // 撞击点禁用
        upStrikePoint.SetActive(false);
        downStrikePoint.SetActive(false);
        leftStrikePoint.SetActive(false);
        rightStrikePoint.SetActive(false);
    }


    #endregion
}
