using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransparentFadeIn : MonoBehaviour
{
    private TextMeshProUGUI tmpComponent;
    private Color baseColor;
 
    void Start()
    {
        tmpComponent = GetComponent<TextMeshProUGUI>();
        baseColor = tmpComponent.color;
    }

  
    void Update()
    {
        if (baseColor.a >= .7f) baseColor.a -= .1f * Time.deltaTime;
       // else if(baseColor.a  1f) baseColor.a -= .1f * Time.deltaTime;

    }
}
