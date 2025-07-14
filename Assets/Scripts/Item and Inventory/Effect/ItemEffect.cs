using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    public virtual void ExcuteEffect(Transform _enemyPosition)
    {
        Debug.Log("效果生成");//Effect excuted
    }

}
