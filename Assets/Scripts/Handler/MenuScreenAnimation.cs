using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenAnimation : MonoBehaviour
{
    // Start Pivot, End Pivot
    // Start Size, End Size
    [Header("Properties")]
    public bool playOnStart = true;
    public float playDelay;

    [Header("References")]
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    [Header("Pivot")]
    public bool playPivot = true;
    public Vector2 startPivot = new Vector2(0.5f, 0.5f);
    public Vector2 endPivot = new Vector2(0.5f, 0.5f);
    public LeanTweenType pivotEaseType = LeanTweenType.easeOutExpo;
    public float pivotDuration = 0.5f;

    [Header("Scale")]
    public bool playScale = true;
    public Vector3 startScale = new Vector3(1, 1, 1);
    public Vector3 endScale = new Vector3(1, 1, 1);
    public LeanTweenType scaleEaseType = LeanTweenType.easeOutExpo;
    public float scaleDuration = 0.5f;

    [Header("Alpha")]
    public bool playAlpha = true;
    public float startAlpha = 0;
    public float endAlpha = 1;
    public LeanTweenType alphaEaseType = LeanTweenType.easeOutExpo;
    public float alphaDuration = 0.5f;

    LTDescr callTween;

    private void Awake()
    {
        SetValueStart();
    }

    public void SetValueStart()
    {
        if (playPivot) rectTransform.pivot = startPivot;
        if (playScale) rectTransform.localScale = startScale;
        if (playAlpha) canvasGroup.alpha = startAlpha;
    }

    private void Start()
    {
        if (playOnStart)
        {
            if (callTween != null) LeanTween.cancel(callTween.id);

            callTween = LeanTween.delayedCall(playDelay, Play);
        }
    }

    public void Play()
    {
        if (playPivot) PlayPivotAnimation();
        if (playScale) PlayScaleAnimation();
        if (playAlpha) PlayAlphaAnimation();
    }

    public void PlayPivotAnimation()
    {
        LeanTween.value(gameObject, startPivot, endPivot, pivotDuration).setEase(pivotEaseType).setOnUpdate((Vector2 value) =>
        {
            rectTransform.pivot = value;
        });
    }

    public void PlayScaleAnimation()
    {
        LeanTween.value(gameObject, startScale, endScale, scaleDuration).setEase(scaleEaseType).setOnUpdate((Vector3 value) =>
        {
            rectTransform.localScale = value;
        });
    }

    public void PlayAlphaAnimation()
    {
        LeanTween.value(gameObject, startAlpha, endAlpha, alphaDuration).setEase(alphaEaseType).setOnUpdate((float value) =>
        {
            canvasGroup.alpha = value;
        });
    }

    private void OnDestroy()
    {
        if (callTween != null) LeanTween.cancel(callTween.id);
    }
}
