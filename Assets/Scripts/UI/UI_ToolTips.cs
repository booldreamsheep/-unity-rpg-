using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTips : MonoBehaviour
{
    

    [SerializeField] private float xLimit = 200;
    [SerializeField] private float yLimit = 100;

    [SerializeField] private float xOffset = 70;
    [SerializeField] private float yOffset = 150;
    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newxOffset = 0;
        float newyOffset = 0;


        if (mousePosition.x > xLimit)
            newxOffset = -xOffset;
        else
            newxOffset = xOffset;//鼠标靠近屏幕右侧时，提示框向左偏移；否则向右偏移

        if (mousePosition.y > yLimit)

            newyOffset = -yOffset;
        else
            newyOffset = yOffset;//鼠标靠近屏幕顶部时，提示框向下偏移；否则向上偏移


        transform.position = new Vector2(mousePosition.x + newxOffset, mousePosition.y + newyOffset);//更新提示框位置为鼠标位置偏移后的点

    }
}
