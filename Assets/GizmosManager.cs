using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using RuntimeGizmos;

public class GizmosManager : MonoBehaviour
{
    public static GizmosManager instance;

    public TransformGizmo transformGizmo;
    public LayoutObject selectedObject;
    public Transform selectedTransform;

    public static UnityEvent<LayoutObject> OnGizmoTargetSet = new UnityEvent<LayoutObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (transformGizmo.mainTargetRoot != null)
        {
            selectedObject = transformGizmo.mainTargetRoot.GetComponent<LayoutObject>();

            if (selectedObject != null)
            {
                if (transformGizmo.isTransforming)
                {
                    LayoutTransform layoutTransform = new LayoutTransform();

                    layoutTransform.position = selectedObject.transform.position;
                    layoutTransform.rotation = selectedObject.transform.eulerAngles;
                    layoutTransform.scale = selectedObject.transform.localScale;

                    selectedObject.SetTransformAcrossNetwork(layoutTransform);
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    selectedObject.Delete();
                }
            }
        }

        if (transformGizmo.mainTargetRoot != selectedTransform)
        {
            selectedTransform = transformGizmo.mainTargetRoot;
            OnGizmoTargetSet.Invoke(selectedObject);
        }
    }

    public void SetGizmoTarget(LayoutObject target)
    {
        ClearGizmosTarget();
        transformGizmo.AddTarget(target.transform);
    }

    public void ClearGizmosTarget()
    {
        transformGizmo.ClearTargets();
    }

    public void SetGizmoMode(TransformType mode)
    {
        transformGizmo.transformType = mode;
    }

    public void SetGizmoSpace(TransformSpace space)
    {
        transformGizmo.space = space;
    }
}
