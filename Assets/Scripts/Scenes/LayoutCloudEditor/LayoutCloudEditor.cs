using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Firebase.Firestore;

public class LayoutCloudEditor : MonoBehaviour
{
    public string layoutID;
    public LayoutData layoutData;
    public LayoutCloudEditorModel model;
    public LayoutCloudEditorView view;

    public UnityEvent<LayoutData> onLayoutUpdate;
    public UnityEvent<LayoutDataProject> onProjectUpdate;

    ListenerRegistration listenerRegistration;

    private void Awake()
    {
        model.Awake(this);
        view.Awake(this);
    }

    private void Start()
    {
        listenerRegistration = model.GetListener();
    }

    public void OnLayoutUpdate(LayoutData layoutData)
    {
        this.layoutData = layoutData;

        onLayoutUpdate.Invoke(layoutData);
        onProjectUpdate.Invoke(layoutData.project);
    }

    [ContextMenu("UpdateLayout")]
    public void UpdateLayout()
    {
        FirestoreAPI.UpdateDocument(
            "sessions",
            layoutID,
            layoutData.ToDictionary()
        );
    }
}

[System.Serializable]
public class LayoutCloudEditorView
{
    LayoutCloudEditor controller;

    public void Awake(LayoutCloudEditor controller)
    {
        this.controller = controller;
    }
}

[System.Serializable]
public class LayoutCloudEditorModel
{
    LayoutCloudEditor controller;

    public void Awake(LayoutCloudEditor controller)
    {
        this.controller = controller;
    }

    public ListenerRegistration GetListener()
    {
        return FirestoreAPI.ListenDocument(
            "sessions",
            controller.layoutID,
            ValidateListener
        );
    }

    public void ValidateListener(DocumentSnapshot snapshot)
    {
        if (snapshot.Exists)
        {
            Debug.Log("Document data for " + snapshot.Id + " document:");
            Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

            LayoutData layoutData = LayoutData.FromDictionary(documentDictionary);

            controller.OnLayoutUpdate(layoutData);
        }
        else
        {
            Debug.Log("Document " + snapshot.Id + " does not exist!");
        }
    }
}
