using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LayoutEditorInspector : MonoBehaviour
{
    public static LayoutEditorInspector instance;

    public LayoutObject selectedObject;

    public UnityEvent<LayoutObject> OnObjectSelected = new UnityEvent<LayoutObject>();

    private void Awake()
    {
        instance = this;
        GizmosManager.OnGizmoTargetSet.AddListener(OnGizmoTargetSet);
    }

    private void Start()
    {
        OnGizmoTargetSet(null);
    }

    private void OnGizmoTargetSet(LayoutObject layoutObject)
    {
        if (layoutObject != null)
        {
            selectedObject = layoutObject;
            SelectObject(layoutObject);
        }
        else
        {
            selectedObject = null;
            OnObjectSelected.Invoke(null);
        }
    }

    public void SelectObject(LayoutObject layoutObject)
    {
        selectedObject = layoutObject;
        OnObjectSelected.Invoke(selectedObject);
    }
}
