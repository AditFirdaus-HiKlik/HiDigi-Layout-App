using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class InspectorMetadata : MonoBehaviour
{
    public LayoutObject target => LayoutEditorInspector.instance.selectedObject;

    public string metadataName
    {
        get
        {
            if (target == null) return "";
            return target.data.metadata.title;
        }
        set
        {
            if (target == null) return;
            target.data.metadata.title = value;
        }
    }

    public string metadataDescription
    {
        get
        {
            if (target == null) return "";
            return target.data.metadata.description;
        }
        set
        {
            if (target == null) return;
            target.data.metadata.description = value;
        }
    }

    public TMP_InputField inputFieldTitle;
    public TMP_InputField inputFieldDescription;

    public void Awake()
    {
        LayoutEditorInspector.instance.OnObjectSelected.AddListener(OnObjectSelected);
    }

    public void OnObjectSelected(LayoutObject layoutObject)
    {
        Refresh();
    }

    private void Refresh()
    {
        inputFieldTitle.text = metadataName;
        inputFieldDescription.text = metadataDescription;
    }

    public void SetTitle(string title)
    {
        metadataName = title;
    }

    public void SetDescription(string description)
    {
        metadataDescription = description;
    }
}
