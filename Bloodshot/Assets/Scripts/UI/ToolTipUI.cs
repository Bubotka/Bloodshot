using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private float _xLimit=960;
    [SerializeField] private float _yLimit=540;

    [SerializeField] private float _xOffset = 150;
    [SerializeField] private float _yOffset = 150;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > _xLimit)
            newXOffset = -_xOffset;
        else
            newXOffset = _xOffset;

        if (mousePosition.y > _yLimit)
            newYOffset = -_yOffset;
        else
            newYOffset = _yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }
}
