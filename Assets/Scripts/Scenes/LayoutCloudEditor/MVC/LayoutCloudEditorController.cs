using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Firebase.Firestore;

public class LayoutCloudEditorController : MonoBehaviour
{
    public string layoutID => layoutCloudEditor.layoutID;
    public LayoutCloudEditor layoutCloudEditor;
    LayoutCloudEditorModel model;
    LayoutCloudEditorView view;

    public UnityEvent<LayoutData> onLayoutUpdate;
    public UnityEvent<LayoutDataProject> onProjectUpdate;

    ListenerRegistration listenerRegistration;

    private void Awake()
    {
        model = GetComponent<LayoutCloudEditorModel>();
        view = GetComponent<LayoutCloudEditorView>();
    }

    private void Start()
    {
        listenerRegistration = model.GetListener();
    }

    public void OnLayoutUpdate(LayoutData layoutData)
    {
        layoutCloudEditor.layoutData = layoutData;

        onLayoutUpdate.Invoke(layoutData);
        onProjectUpdate.Invoke(layoutData.project);
    }

    [ContextMenu("UpdateLayout")]
    public void UpdateLayout()
    {
        FirestoreAPI.UpdateDocument(
            "sessions",
            layoutID,
            layoutCloudEditor.layoutData.ToDictionary()
        );
    }
}
