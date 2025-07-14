using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LadderDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private Vector2 detectionSize = new Vector2(0.6f, 0.2f);
    [SerializeField] private float detectionOffsetY = 0.5f;

    public bool IsTouchingLadder { get; private set; }

    private void Update()
    {
        // 使用盒型检测代替触发器
        Vector2 detectionCenter = (Vector2)transform.position - Vector2.up * detectionOffsetY;
        IsTouchingLadder = Physics2D.OverlapBox(
            detectionCenter,
            detectionSize,
            0,
            ladderLayer
        );

        DebugVisualization(detectionCenter);  // 调试可视化（可选）
    }

    private void DebugVisualization(Vector2 center)
    {
        Color debugColor = IsTouchingLadder ? Color.green : Color.red;
        Debug.DrawRay(center - detectionSize / 2, Vector2.right * detectionSize.x, debugColor);
        Debug.DrawRay(center + detectionSize / 2 * Vector2.up, Vector2.down * detectionSize.y, debugColor);
    }

}
