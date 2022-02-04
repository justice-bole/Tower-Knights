using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]

public class TransparentFadeIn : MonoBehaviour
{
    [SerializeField] float transparencyFloor = 0.6f;
    [SerializeField] float transparencyCeiling = 1.0f;
    [SerializeField] float fadeRate = .25f;
    [SerializeField] float scaleRate = .025f;

    private bool alphaIsDecreasing = true;
    private TextMeshProUGUI tmpComponent;
    private RectTransform rectTransform;

    private void Start()
    {
        tmpComponent = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (tmpComponent.alpha > transparencyFloor && alphaIsDecreasing) 
        {
            FadeTextOut(tmpComponent, fadeRate);
            ScaleRectText(rectTransform, scaleRate, false);
        }
        else if (tmpComponent.alpha <= transparencyFloor && alphaIsDecreasing)
        {
            alphaIsDecreasing = false;
        }

        if(!alphaIsDecreasing && tmpComponent.alpha < transparencyCeiling)
        {
            FadeTextIn(tmpComponent, fadeRate);
            ScaleRectText(rectTransform, scaleRate, true);
        }
        else if(!alphaIsDecreasing && tmpComponent.alpha >= transparencyCeiling)
        {
            alphaIsDecreasing = true;
        }
    }

    private void FadeTextOut(TextMeshProUGUI textToFade, float fadeOutRate)
    {
        textToFade.alpha -= fadeOutRate * Time.deltaTime;
    }

    private void FadeTextIn(TextMeshProUGUI textToFadeIn, float fadeInRate)
    {
        textToFadeIn.alpha += fadeInRate * Time.deltaTime;
    }

    private void ScaleRectText(RectTransform rectToScale, float scaleRate, bool scaleUp)
    {
        float xScale = rectToScale.localScale.x;
        float yScale = rectToScale.localScale.y;
        if (scaleUp)
        {
            xScale += scaleRate * Time.deltaTime;
            yScale += scaleRate * Time.deltaTime;
        }
        else
        {
            xScale -= scaleRate * Time.deltaTime;
            yScale -= scaleRate * Time.deltaTime;
        }
        rectTransform.localScale = new Vector2(xScale, yScale);
    }
}
