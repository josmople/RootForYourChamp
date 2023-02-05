using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizingUI : MonoBehaviour
{

    public Canvas canvas;
    public RectTransform minimized;
    public RectTransform maximized;
    public RectTransform rectangle;

    public float interpolation;

    public void Start()
    {
        if (this.rectangle == null)
            this.rectangle = GetComponent<RectTransform>();
        if (this.canvas == null)
            this.canvas = FindObjectOfType<Canvas>();
    }

    public void Update()
    {
        //this.rectangle.anchorMin = Vector2.Lerp(minimized.anchorMin, maximized.anchorMin, interpolation);
        //this.rectangle.anchorMax = Vector2.Lerp(minimized.anchorMax, maximized.anchorMax, interpolation);
        //this.rectangle.sizeDelta = Vector2.Lerp(minimized.sizeDelta, maximized.sizeDelta, interpolation);
        //this.rectangle.anchoredPosition = Vector2.Lerp(minimized.anchoredPosition, maximized.anchoredPosition, interpolation);

        //this.rectangle.pivot = Vector2.Lerp(minimized.pivot, maximized.pivot, interpolation);
        this.rectangle.position = Vector2.Lerp(minimized.position, maximized.position, interpolation);
        //this.rectangle.localScale = Vector2.Lerp(minimized.localScale, maximized.localScale, interpolation);

        var target_width = Mathf.Lerp(minimized.rect.width, maximized.rect.width, interpolation);
        var target_height = Mathf.Lerp(minimized.rect.height, maximized.rect.height, interpolation);

        this.rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target_width);
        this.rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, target_height);
    }

}
