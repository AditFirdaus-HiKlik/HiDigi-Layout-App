using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutAction : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        LayoutManager.OnLayoutDataSyncing.AddListener((value) =>
        {
            canvasGroup.interactable = !value;
        });
    }

    public void Save()
    {
        LayoutManager.SaveInstance();
    }

    public void Load()
    {
        LayoutManager.LoadInstance();
    }
}
