using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using TriLibCore;

using Photon.Pun;

public class LayoutObject : MonoBehaviourPun
{
    [Header("References")]

    [Header("Properties")]

    [SerializeField]
    public LayoutInstanceData data = new LayoutInstanceData();

    [Header("MVC")]
    public LayoutObjectView view = new LayoutObjectView();
    public LayoutObjectModel model = new LayoutObjectModel();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(view.boxCollider.bounds.center, view.boxCollider.bounds.size);
    }

    private void Awake()
    {
        model.Init(this);
        view.Init(this);
    }

    public void Select()
    {
        GizmosManager.instance.SetGizmoTarget(this);
    }

    private void Start()
    {
        Refresh();

        view.LoadModel();
    }

    public void Refresh()
    {
        transform.position = data.transform.position;
        transform.eulerAngles = data.transform.rotation;
        transform.localScale = data.transform.scale;

        view.Refresh();
    }

    [PunRPC]
    public void SetTransform(string transformJson)
    {
        LayoutTransform instanceTransform = transformJson.JsonToObject<LayoutTransform>();

        data.transform = instanceTransform;

        transform.position = instanceTransform.position;
        transform.eulerAngles = instanceTransform.rotation;
        transform.localScale = instanceTransform.scale;
    }

    public void SetTransformAcrossNetwork(LayoutTransform instanceTransform)
    {
        data.transform = instanceTransform;
        photonView.RPC("SetTransform", RpcTarget.OthersBuffered, instanceTransform.ObjectToJson());
    }

    public void SetInstanceData(InstanceData instanceData)
    {
        data.data = instanceData;
        view.LoadModel();
    }

    [PunRPC]
    public void SetInstanceDataJson(string instanceData)
    {
        SetInstanceData(instanceData.JsonToObject<InstanceData>());
    }

    public void SetInstanceDataAcrossNetwork(InstanceData instanceData)
    {
        data.data = instanceData;
        photonView.RPC("SetInstanceDataJson", RpcTarget.OthersBuffered, instanceData.ObjectToJson());
    }

    public void SetLayoutInstanceData(LayoutInstanceData layoutInstanceData)
    {
        data = layoutInstanceData;
        SetInstanceDataAcrossNetwork(layoutInstanceData.data);
        SetTransformAcrossNetwork(layoutInstanceData.transform);
        Refresh();
    }

    [PunRPC]
    public void SetLayoutInstanceDataJson(string layoutInstanceData)
    {
        SetLayoutInstanceData(layoutInstanceData.JsonToObject<LayoutInstanceData>());
    }

    public void SetLayoutInstanceDataAcrossNetwork(LayoutInstanceData layoutInstanceData)
    {
        data = layoutInstanceData;
        string layoutInstanceDataJson = layoutInstanceData.ObjectToJson();
        Debug.Log("SetLayoutInstanceDataAcrossNetwork: " + layoutInstanceDataJson);
        photonView.RPC("SetLayoutInstanceDataJson", RpcTarget.AllBuffered, layoutInstanceDataJson);
    }

    public void Delete()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}

[System.Serializable]
public class LayoutObjectView
{
    [HideInInspector] public LayoutObject controller;

    public BoxCollider boxCollider;
    public GameObject modelObjectWrapper;

    public Renderer[] renderers;
    Bounds bounds;

    Coroutine loadModelCoroutine;

    public void Init(LayoutObject controller)
    {
        this.controller = controller;
    }

    public void Refresh()
    {

    }

    public void LoadModel()
    {
        if (loadModelCoroutine != null) controller.StopCoroutine(loadModelCoroutine);
        loadModelCoroutine = controller.StartCoroutine(LoadModelAsync());
    }

    public IEnumerator LoadModelAsync()
    {
        if (modelObjectWrapper != null) GameObject.Destroy(modelObjectWrapper);

        if (controller.data.data.model.url.Length == 0) yield break;

        InstanceData data = controller.data.data;
        InstanceDataModel model = data.model;

        Debug.Log("Loading model: " + model.fileName);

        string path = Path.Combine(Application.persistentDataPath, "Temp", "Assets", "Models", data.id + data.model.fileExtension);

        var www = new UnityWebRequest(model.url);

        Debug.Log("Model path: " + path);

        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        if (!File.Exists(path)) www.downloadHandler = new DownloadHandlerFile(path);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            modelObjectWrapper = new GameObject("Wrapper");

            modelObjectWrapper.transform.SetParent(controller.transform);
            modelObjectWrapper.transform.localPosition = Vector3.zero;
            modelObjectWrapper.transform.localRotation = Quaternion.identity;
            modelObjectWrapper.transform.localScale = Vector3.one;

            GameObject rootGameObject = null;
            bool isLoaded = false;

            var context = AssetLoader.LoadModelFromFile(
                path,
                wrapperGameObject: modelObjectWrapper,
                onMaterialsLoad: (context) =>
                {
                    rootGameObject = context.RootGameObject;
                    isLoaded = true;
                }
            );

            var task = context.Task;

            yield return new WaitUntil(() => isLoaded);

            CreateBounds();

            Debug.Log("Model loaded: " + data.id);
        }
    }

    public void CreateBounds()
    {
        Transform transform = controller.transform;

        bounds = new Bounds(transform.position, Vector3.zero);
        renderers = controller.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            Debug.Log("renderers found");
            boxCollider.center = transform.InverseTransformPoint(bounds.center);
            boxCollider.size = bounds.size;

            transform.Translate(Vector3.up * ((bounds.size.y / 2.0f) - (bounds.center.y - transform.position.y)), Space.World);
        }
        else
        {
            Debug.Log("No renderers found");

            boxCollider.size = Vector3.zero;
        }
    }
}

[System.Serializable]
public class LayoutObjectModel
{
    [HideInInspector] public LayoutObject controller;

    public void Init(LayoutObject controller)
    {
        this.controller = controller;
    }
}
