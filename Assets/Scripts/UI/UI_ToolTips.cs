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
            newxOffset = xOffset;//��꿿����Ļ�Ҳ�ʱ����ʾ������ƫ�ƣ���������ƫ��

        if (mousePosition.y > yLimit)

            newyOffset = -yOffset;
        else
            newyOffset = yOffset;//��꿿����Ļ����ʱ����ʾ������ƫ�ƣ���������ƫ��


        transform.position = new Vector2(mousePosition.x + newxOffset, mousePosition.y + newyOffset);//������ʾ��λ��Ϊ���λ��ƫ�ƺ�ĵ�

    }
}
