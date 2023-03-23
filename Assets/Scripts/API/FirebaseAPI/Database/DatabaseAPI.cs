using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Database;

public class DatabaseAPI
{
    public static FirebaseDatabase db => FirebaseDatabase.GetInstance(FirebaseAPI.app);

    static GlobalMonoBehaviour monoBehaviour = GlobalMonoBehaviour.instance;

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoBehaviour.StartCoroutine(coroutine);
    }

    // Coroutine
    public static Coroutine GetDocument(string collection, string document)
    {
        return StartCoroutine(GetDocumentAsync(collection, document));
    }

    public static Coroutine SetDocument(string collection, string document, string data)
    {
        return StartCoroutine(SetDocumentAsync(collection, document, data));
    }
    public static Coroutine UpdateDocument(string collection, string document, string data)
    {
        return StartCoroutine(UpdateDocumentAsync(collection, document, data));
    }
    public static Coroutine DeleteDocument(string collection, string document)
    {
        return StartCoroutine(DeleteDocumentAsync(collection, document));
    }
    public static Coroutine AddDocument(string collection, string document, string data)
    {
        return StartCoroutine(AddDocumentAsync(collection, document, data));
    }

    // Async
    public static IEnumerator GetDocumentAsync(string collection, string document)
    {
        DatabaseReference docRef = db.GetReference(collection).Child(document);
        Task<DataSnapshot> getDocTask = docRef.GetValueAsync();

        yield return new WaitUntil(() => getDocTask.IsCompleted);

        if (getDocTask.Exception != null)
        {
            DebugApp.Log("Failed to get document: " + getDocTask.Exception);
        }
        else
        {
            DebugApp.Log("Got document: " + getDocTask.Result.GetRawJsonValue());
        }
    }
    public static IEnumerator GetCollectionAsync(string collection)
    {
        DatabaseReference docRef = db.GetReference(collection);
        Task<DataSnapshot> getDocTask = docRef.GetValueAsync();

        yield return new WaitUntil(() => getDocTask.IsCompleted);

        if (getDocTask.Exception != null)
        {
            DebugApp.Log("Failed to get collection: " + getDocTask.Exception);
        }
        else
        {
            DebugApp.Log("Got collection: " + getDocTask.Result.GetRawJsonValue());
        }
    }
    public static IEnumerator SetDocumentAsync(string collection, string document, string data)
    {
        DatabaseReference docRef = db.GetReference(collection).Child(document);
        Task setDocTask = docRef.SetRawJsonValueAsync(data);

        yield return new WaitUntil(() => setDocTask.IsCompleted);

        if (setDocTask.Exception != null)
        {
            DebugApp.Log("Failed to set document: " + setDocTask.Exception);
        }
    }
    public static IEnumerator UpdateDocumentAsync(string collection, string document, string data)
    {
        DatabaseReference docRef = db.GetReference(collection).Child(document);
        Task updateDocTask = docRef.SetRawJsonValueAsync(data);

        yield return new WaitUntil(() => updateDocTask.IsCompleted);

        if (updateDocTask.Exception != null)
        {
            DebugApp.Log("Failed to update document: " + updateDocTask.Exception);
        }
    }
    public static IEnumerator DeleteDocumentAsync(string collection, string document)
    {
        Task deleteDocTask = db.GetReference(collection).Child(document).RemoveValueAsync();

        yield return new WaitUntil(() => deleteDocTask.IsCompleted);

        if (deleteDocTask.Exception != null)
        {
            DebugApp.Log("Failed to delete document: " + deleteDocTask.Exception);
        }
    }
    public static IEnumerator AddDocumentAsync(string collection, string document, string data)
    {
        Task addDocTask = db.GetReference(collection).Child(document).SetRawJsonValueAsync(data);

        yield return new WaitUntil(() => addDocTask.IsCompleted);

        if (addDocTask.Exception != null)
        {
            DebugApp.Log("Failed to add document: " + addDocTask.Exception);
        }
    }
}
