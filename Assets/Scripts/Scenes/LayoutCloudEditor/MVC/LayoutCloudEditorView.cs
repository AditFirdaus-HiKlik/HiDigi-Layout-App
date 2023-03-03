using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutCloudEditorView : MonoBehaviour
{
    LayoutCloudEditorController controller;

    private void Awake()
    {
        controller = GetComponent<LayoutCloudEditorController>();
    }
}
