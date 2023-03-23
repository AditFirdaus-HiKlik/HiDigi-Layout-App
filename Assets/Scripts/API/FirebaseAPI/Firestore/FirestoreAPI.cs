using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Firestore;

public class FirestoreAPI
{
    public static FirebaseFirestore db => FirebaseFirestore.GetInstance(FirebaseAPI.app);

    static GlobalMonoBehaviour monoBehaviour = GlobalMonoBehaviour.instance;

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoBehaviour.StartCoroutine(coroutine);
    }

    // Coroutine
    public static Coroutine GetDocument(string collection, string document, UnityAction<DocumentSnapshot> callback = null)
    {
        return StartCoroutine(GetDocumentAsync(collection, document, callback));
    }
    public static Coroutine GetCollection(string collection, UnityAction<QuerySnapshot> callback = null)
    {
        return StartCoroutine(GetCollectionAsync(collection, callback));
    }
    public static Coroutine SetDocument(string collection, string document, Dictionary<string, object> data, UnityAction callback = null)
    {
        return StartCoroutine(SetDocumentAsync(collection, document, data, callback));
    }
    public static Coroutine UpdateDocument(string collection, string document, Dictionary<string, object> data, UnityAction callback = null)
    {
        return StartCoroutine(UpdateDocumentAsync(collection, document, data, callback = null));
    }
    public static Coroutine DeleteDocument(string collection, string document, UnityAction callback = null)
    {
        return StartCoroutine(DeleteDocumentAsync(collection, document, callback = null));
    }
    public static Coroutine AddDocument(string collection, Dictionary<string, object> data, UnityAction<DocumentReference> callback = null)
    {
        return StartCoroutine(AddDocumentAsync(collection, data, callback = null));
    }

    public static ListenerRegistration ListenDocument(string collection, string document, UnityAction<DocumentSnapshot> callback = null)
    {
        DocumentReference docRef = db.Collection(collection).Document(document);
        return docRef.Listen(snapshot =>
        {
            callback?.Invoke(snapshot);
        });
    }

    // Async
    public static IEnumerator GetDocumentAsync(string collection, string document, UnityAction<DocumentSnapshot> callback = null)
    {
        DocumentReference docRef = db.Collection(collection).Document(document);
        Task<DocumentSnapshot> getDocTask = docRef.GetSnapshotAsync();

        yield return new WaitUntil(() => getDocTask.IsCompleted);

        if (getDocTask.Exception != null)
        {
            DebugApp.Log("Failed to get document: " + getDocTask.Exception);
        }
        else
        {
            callback?.Invoke(getDocTask.Result);
        }
    }
    public static IEnumerator GetCollectionAsync(string collection, UnityAction<QuerySnapshot> callback = null)
    {
        Query query = db.Collection(collection);
        Task<QuerySnapshot> getCollectionTask = query.GetSnapshotAsync();

        yield return new WaitUntil(() => getCollectionTask.IsCompleted);

        if (getCollectionTask.Exception != null)
        {
            DebugApp.Log("Failed to get collection: " + getCollectionTask.Exception);
        }
        else
        {
            callback?.Invoke(getCollectionTask.Result);
        }
    }
    public static IEnumerator SetDocumentAsync(string collection, string document, Dictionary<string, object> data, UnityAction callback = null)
    {
        DocumentReference docRef = db.Collection(collection).Document(document);
        Task setDocTask = docRef.SetAsync(data);

        yield return new WaitUntil(() => setDocTask.IsCompleted);

        if (setDocTask.Exception != null)
        {
            DebugApp.Log("Failed to set document: " + setDocTask.Exception);
        }
        else
        {
            callback?.Invoke();
        }
    }
    public static IEnumerator UpdateDocumentAsync(string collection, string document, Dictionary<string, object> data, UnityAction callback = null)
    {
        DocumentReference docRef = db.Collection(collection).Document(document);
        Task updateDocTask = docRef.UpdateAsync(data);

        yield return new WaitUntil(() => updateDocTask.IsCompleted);

        if (updateDocTask.Exception != null)
        {
            DebugApp.Log("Failed to update document: " + updateDocTask.Exception);
        }
        else
        {
            callback?.Invoke();
        }
    }
    public static IEnumerator DeleteDocumentAsync(string collection, string document, UnityAction callback = null)
    {
        DocumentReference docRef = db.Collection(collection).Document(document);
        Task deleteDocTask = docRef.DeleteAsync();

        yield return new WaitUntil(() => deleteDocTask.IsCompleted);

        if (deleteDocTask.Exception != null)
        {
            DebugApp.Log("Failed to delete document: " + deleteDocTask.Exception);
        }
        else
        {
            callback?.Invoke();
        }
    }
    public static IEnumerator AddDocumentAsync(string collection, Dictionary<string, object> data, UnityAction<DocumentReference> callback = null)
    {
        CollectionReference collectionRef = db.Collection(collection);
        Task<DocumentReference> addDocTask = collectionRef.AddAsync(data);

        yield return new WaitUntil(() => addDocTask.IsCompleted);

        if (addDocTask.Exception != null)
        {
            DebugApp.Log("Failed to add document: " + addDocTask.Exception);
        }
        else
        {
            callback?.Invoke(addDocTask.Result);
        }
    }
}
