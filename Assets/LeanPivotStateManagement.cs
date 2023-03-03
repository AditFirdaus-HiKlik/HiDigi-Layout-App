using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanPivotStateManagement : MonoBehaviour
{
    public bool isShowing = true;

    public Vector2 showPivot = new Vector2(0.5f, 0.5f);
    public Vector2 hidePivot = new Vector2(0.5f, 0.5f);
    public float duration = 0.5f;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetStateInstant(isShowing);
    }

    public void ToogleState()
    {
        SetState(!isShowing);
    }

    public void SetState(bool isShowing)
    {
        this.isShowing = isShowing;

        if (isShowing) LeanPivot(showPivot, duration, LeanTweenType.easeOutExpo);
        else LeanPivot(hidePivot, duration, LeanTweenType.easeOutExpo);
    }

    public void SetStateInstant(bool isShowing)
    {
        this.isShowing = isShowing;

        if (isShowing) LeanPivot(showPivot, 0, LeanTweenType.easeOutExpo);
        else LeanPivot(hidePivot, 0, LeanTweenType.easeOutExpo);
    }

    public LTDescr LeanPivot(Vector2 pivot, float duration, LeanTweenType easeType)
    {
        return LeanTween.value(gameObject, rectTransform.pivot, pivot, duration).setEase(easeType).setOnUpdate((Vector2 value) =>
        {
            rectTransform.pivot = value;
        });
    }
}
