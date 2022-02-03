using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    public AnimationCurve myCurve;

    void Update()
    {
        transform.position = new Vector2(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)));
    }
}
