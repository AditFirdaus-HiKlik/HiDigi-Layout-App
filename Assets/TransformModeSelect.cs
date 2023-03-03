using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RuntimeGizmos;

public class TransformModeSelect : MonoBehaviour
{
    public TransformModeSelectView view;
    public TransformModeSelectModel model;

    void Start()
    {
        view.Awake(this);
        model.Awake(this);
    }

    public void SetType(int mode)
    {
        TransformType transformType = (TransformType)mode;

        model.transformType = transformType;
        GizmosManager.instance.SetGizmoMode(transformType);
    }

    public void SetSpace(int mode)
    {
        TransformSpace transformSpace = (TransformSpace)mode;

        model.transformSpace = transformSpace;
        GizmosManager.instance.SetGizmoSpace(transformSpace);
    }
}

[System.Serializable]
public class TransformModeSelectView
{
    public TransformModeSelect controller;
    public ToggleGroup typeGroup;
    public ToggleGroup spaceGroup;

    public void Awake(TransformModeSelect controller)
    {
        this.controller = controller;
    }
}

[System.Serializable]
public class TransformModeSelectModel
{
    public TransformModeSelect controller;

    public TransformType transformType;
    public TransformSpace transformSpace;

    public void Awake(TransformModeSelect controller)
    {
        this.controller = controller;
    }
}
