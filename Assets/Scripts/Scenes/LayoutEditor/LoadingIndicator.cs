using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIndicator : MonoBehaviour
{
    public RectTransform card_Transform;
    public RectTransform loading_Transform;

    public CanvasGroup canvasGroup;

    public bool isLoading = false;

    public float speed = 1f;


    private void Awake()
    {
        SetLoading(isLoading);
    }

    public void SetLoading(bool isLoading)
    {
        this.isLoading = isLoading;

        if (canvasGroup) canvasGroup.interactable = !isLoading;
        card_Transform.gameObject.SetActive(isLoading);
        loading_Transform.gameObject.SetActive(isLoading);
    }

    void Update()
    {
        if (isLoading)
        {
            float angle = loading_Transform.localEulerAngles.z;

            angle += speed * Time.deltaTime;

            angle = angle % 360;

            loading_Transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}
