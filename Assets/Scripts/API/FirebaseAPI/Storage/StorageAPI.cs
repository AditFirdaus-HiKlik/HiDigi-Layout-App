using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Storage;

public class StorageAPI
{
    public static FirebaseStorage db => FirebaseStorage.GetInstance(FirebaseAPI.app);

    static GlobalMonoBehaviour monoBehaviour = GlobalMonoBehaviour.instance;

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoBehaviour.StartCoroutine(coroutine);
    }

    // Coroutine
    public static Coroutine DownloadFile(string path, UnityAction<byte[]> callback)
    {
        return StartCoroutine(DownloadFileAsync(path, callback));
    }

    public static Coroutine UploadFile(string path, byte[] data, UnityAction<StorageMetadata> callback)
    {
        return StartCoroutine(UploadFileAsync(path, data, callback));
    }

    public static Coroutine DeleteFile(string path, UnityAction callback)
    {
        return StartCoroutine(DeleteFileAsync(path, callback));
    }

    public static Coroutine GetMetadata(string path, UnityAction<StorageMetadata> callback)
    {
        return StartCoroutine(GetMetadataAsync(path, callback));
    }

    // Async
    static IEnumerator DownloadFileAsync(string path, UnityAction<byte[]> callback)
    {
        StorageReference storageRef = db.GetReference(path);
        Task<byte[]> getBytesTask = storageRef.GetBytesAsync(1024 * 1024 * 10);

        yield return new WaitUntil(() => getBytesTask.IsCompleted);

        if (getBytesTask.Exception != null)
        {
            Debug.LogWarning("Failed to get file: " + getBytesTask.Exception);
        }
        else
        {
            callback(getBytesTask.Result);
        }
    }

    static IEnumerator UploadFileAsync(string path, byte[] data, UnityAction<StorageMetadata> callback)
    {
        StorageReference storageRef = db.GetReference(path);
        Task<StorageMetadata> putBytesTask = storageRef.PutBytesAsync(data);

        yield return new WaitUntil(() => putBytesTask.IsCompleted);

        if (putBytesTask.Exception != null)
        {
            Debug.LogWarning("Failed to upload file: " + putBytesTask.Exception);
        }
        else
        {
            callback(putBytesTask.Result);
        }
    }

    static IEnumerator DeleteFileAsync(string path, UnityAction callback)
    {
        StorageReference storageRef = db.GetReference(path);
        Task deleteTask = storageRef.DeleteAsync();

        yield return new WaitUntil(() => deleteTask.IsCompleted);

        if (deleteTask.Exception != null)
        {
            Debug.LogWarning("Failed to delete file: " + deleteTask.Exception);
        }
        else
        {
            callback();
        }
    }

    static IEnumerator GetMetadataAsync(string path, UnityAction<StorageMetadata> callback)
    {
        StorageReference storageRef = db.GetReference(path);
        Task<StorageMetadata> getMetadataTask = storageRef.GetMetadataAsync();

        yield return new WaitUntil(() => getMetadataTask.IsCompleted);

        if (getMetadataTask.Exception != null)
        {
            Debug.LogWarning("Failed to get metadata: " + getMetadataTask.Exception);
        }
        else
        {
            callback(getMetadataTask.Result);
        }
    }
}
