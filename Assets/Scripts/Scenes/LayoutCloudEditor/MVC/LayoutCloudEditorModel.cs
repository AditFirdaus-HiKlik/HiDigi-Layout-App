using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class LayoutCloudEditorModel : MonoBehaviour
{
    LayoutCloudEditorController controller;

    private void Awake()
    {
        controller = GetComponent<LayoutCloudEditorController>();
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
