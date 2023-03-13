using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

using TriLibCore;

// This handler is used to cache the model
// It manage the download and cache of the model
public class ModelCacheHandler : MonoBehaviour
{
    // Get model, if it exist in scene then copy it, if don't then load it from local, if don't then download it from server
    public static string cachePath => Path.Combine(Application.persistentDataPath, "Temp", "Assets", "Models");
    public static ModelCacheHandler instance;
    public Dictionary<string, GameObject> modelCache = new Dictionary<string, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void ClearCache()
    {
        foreach (var item in modelCache)
        {
            Destroy(item.Value);
        }

        modelCache.Clear();
    }

    public Coroutine GetModel(InstanceDataModel model, UnityAction<GameObject> callback)
    {
        return StartCoroutine(_GetModel(model, callback));
    }

    IEnumerator _GetModel(InstanceDataModel model, UnityAction<GameObject> callback)
    {
        if (modelCache.ContainsKey(model.guid))
        {
            CopyFromCache(model.guid, callback);
            yield break;
        }

        string path = Path.Combine(cachePath, model.guid + model.fileExtension);
        string directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log($"[MODEL LOADER] [{model.guid}] Directory doesn't exist, creating {directory}");
        }

        if (!File.Exists(path))
        {
            Debug.Log($"[MODEL LOADER] [{model.guid}] File doesn't exist on {path}, downloading {model.url}");

            var www = new UnityWebRequest(model.url);

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            www.downloadHandler = new DownloadHandlerFile(path);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[MODEL LOADER] [{model.guid}] Error downloading {model.url} {www.error}");
                callback?.Invoke(null);
                yield break;
            }
            else
            {
                Debug.Log($"[MODEL LOADER] [{model.guid}] Downloaded {model.url}");
            }
        }

        GameObject modelObjectWrapper = new GameObject(model.guid)
        {
            transform = {
                parent = transform,
                localPosition = Vector3.zero,
                localRotation = Quaternion.identity,
                localScale = Vector3.one
            }
        };

        GameObject rootGameObject = null;
        bool isLoaded = false;
        bool isError = false;

        Debug.Log($"[MODEL LOADER] [{model.guid}] Loading model: {path}");

        var context = AssetLoader.LoadModelFromFile(
            path,
            wrapperGameObject: modelObjectWrapper,
            onMaterialsLoad: (context) =>
            {
                rootGameObject = context.RootGameObject;
                modelObjectWrapper.SetActive(false);
                isLoaded = true;
            },
            onProgress: (context, progress) =>
            {
                Debug.Log($"[MODEL LOADER] [{model.guid}] Loading... {progress * 100}%");
            },
            onError: (context) =>
            {
                isError = true;
            }
        );

        yield return new WaitUntil(() => isLoaded);

        if (isError)
        {
            Debug.Log($"[MODEL LOADER] [{model.guid}] Error loading model: {path}");
            callback?.Invoke(null);
            yield break;
        }

        Debug.Log($"[MODEL LOADER] [{model.guid}] Loaded model: {path}");

        if (!modelCache.ContainsKey(model.guid)) modelCache.Add(model.guid, modelObjectWrapper);

        CopyFromCache(model.guid, callback);
    }

    public void CopyFromCache(string guid, UnityAction<GameObject> callback)
    {
        if (modelCache.ContainsKey(guid))
        {
            GameObject go = Instantiate(modelCache[guid]);
            go.SetActive(true);
            callback(go);

            Debug.Log($"[MODEL LOADER] [{guid}] Callback called with copy of {guid}");
        }
        else
        {
            Debug.Log($"[MODEL LOADER] [{guid}] is already in cache");
        }
    }
}
