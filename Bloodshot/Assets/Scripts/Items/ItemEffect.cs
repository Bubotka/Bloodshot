using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string EffectDescription;

    public virtual void ExecuteEffect(Transform enemyPosition)
    {
        Debug.Log("Effect executed");
    }
}
 