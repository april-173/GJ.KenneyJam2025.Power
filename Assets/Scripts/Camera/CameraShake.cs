using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CameraShake : MonoBehaviour
{

    [Header("相机震动")]
    [Tooltip("震动持续时间")]
    public float shakeDuration;
    [Tooltip("震动幅度")]
    public float shakeMagnitude;
    [Tooltip("震动衰减速度")]
    public float shakeDampingSpeed;
    [Tooltip("震动曲线控制")]
    public AnimationCurve shakeCurve;

    private Vector3 initialPosition;         // 初始位置
    private bool isShaking = false;          // 震动状态

    void Awake()
    {
        // 保存初始位置
        initialPosition = transform.localPosition;

        // 设置默认震动曲线（如果没有在Inspector中设置）
        if (shakeCurve == null || shakeCurve.length == 0)
        {
            shakeCurve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(0.5f, -1f),
                new Keyframe(1f, 0f)
            );
            shakeCurve.preWrapMode = WrapMode.Clamp;
            shakeCurve.postWrapMode = WrapMode.Clamp;
        }
    }

    // 触发相机震动
    public void TriggerShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    // 震动协程
    private IEnumerator Shake()
    {
        isShaking = true;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // 计算当前震动进度 (0-1)
            float progress = elapsedTime / shakeDuration;

            // 根据曲线获取当前震动幅度
            float curveValue = shakeCurve.Evaluate(progress);

            // 计算偏移量
            Vector2 offset = Random.insideUnitCircle * shakeMagnitude * curveValue;

            // 应用偏移
            transform.localPosition = initialPosition + (Vector3)offset;

            // 更新时间
            elapsedTime += Time.deltaTime * shakeDampingSpeed;

            yield return null;
        }

        // 恢复初始位置
        transform.localPosition = initialPosition;
        isShaking = false;
    }
}

