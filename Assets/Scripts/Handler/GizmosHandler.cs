using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

using RuntimeGizmos;

public class GizmosHandler : MonoBehaviour
{
    public static GizmosHandler instance;

    public TransformGizmo transformGizmo;
    public LayoutObject selectedObject;
    public Transform selectedTransform;

    public static UnityEvent<LayoutObject> OnAddObject = new UnityEvent<LayoutObject>();
    public static UnityEvent<bool> OnBoundingBoxToogle = new UnityEvent<bool>();

    public static bool isBoundingBoxActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        transformGizmo.onAddTarget.AddListener(OnAddTarget);
    }

    private void Update()
    {
        if (selectedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                selectedObject.Delete();
            }
        }
    }

    private void OnAddTarget(Transform target)
    {
        selectedTransform = target;
        selectedObject = target.GetComponent<LayoutObject>();

        selectedObject.TransferOwnership(PhotonNetwork.LocalPlayer);

        OnAddObject.Invoke(selectedObject);
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

    public static void ToggleBoundingBox()
    {
        isBoundingBoxActive = !isBoundingBoxActive;
        OnBoundingBoxToogle.Invoke(isBoundingBoxActive);
    }
}
