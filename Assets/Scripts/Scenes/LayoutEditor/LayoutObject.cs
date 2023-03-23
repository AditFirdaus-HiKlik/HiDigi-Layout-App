using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using TriLibCore;

using Photon.Pun;

public class LayoutObject : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{
    [Header("References")]

    [Header("Properties")]

    [SerializeField]
    public LayoutInstanceData data = new LayoutInstanceData();

    [SerializeField]
    public bool isSelected = false;

    [Header("MVC")]
    public LayoutObjectView view = new LayoutObjectView();
    public LayoutObjectModel model = new LayoutObjectModel();

    public Coroutine loadModelCoroutine => view.loadModelCoroutine;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(view.boxCollider.bounds.center, view.boxCollider.bounds.size);
    }

    private void Awake()
    {
        photonView.ObservedComponents.Add(this);

        model.Init(this);
        view.Init(this);
    }

    private void Start()
    {
        GizmosHandler.OnBoundingBoxToogle.AddListener(OnToogleBoundingBox);
        OnToogleBoundingBox(GizmosHandler.isBoundingBoxActive);
    }

    public void Select()
    {
        GizmosHandler.instance.SetGizmoTarget(this);
    }

    public void Refresh()
    {
        view.Refresh();
    }

    public void OnToogleBoundingBox(bool value)
    {
        view.SetBoundingBox(value);
    }

    public void UpdateTransform()
    {
        transform.position = data.transform.position;
        transform.eulerAngles = data.transform.rotation;
        transform.localScale = data.transform.scale;
    }

    public void TransferOwnership(Photon.Realtime.Player player)
    {
        photonView.TransferOwnership(player);

        DebugApp.Log("TransferOwnership: " + player.ActorNumber);
    }

    public LayoutTransform GetLayoutTransform()
    {
        data.transform.position = transform.position;
        data.transform.rotation = transform.eulerAngles;
        data.transform.scale = transform.localScale;

        return data.transform;
    }

    public LayoutInstanceData GetLayoutInstanceData()
    {
        data.transform = GetLayoutTransform();
        return data;
    }

    public Coroutine Delete()
    {
        return StartCoroutine(_Delete());
    }

    IEnumerator _Delete()
    {
        TransferOwnership(PhotonNetwork.LocalPlayer);
        yield return new WaitUntil(() => photonView.IsMine);
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;

        LayoutInstanceData layoutInstanceData = JsonUtility.FromJson<LayoutInstanceData>((string)instantiationData[0]);

        data = layoutInstanceData;

        transform.position = data.transform.position;
        transform.eulerAngles = data.transform.rotation;
        transform.localScale = data.transform.scale;

        view.LoadModel();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            data.transform = GetLayoutTransform();
            string dataJson = data.ObjectToJson();
            stream.SendNext(dataJson);
        }
        else
        {
            string dataJson = (string)stream.ReceiveNext();
            data = dataJson.JsonToObject<LayoutInstanceData>();
        }
    }
}

[System.Serializable]
public class LayoutObjectView
{
    [HideInInspector] public LayoutObject controller;

    public BoxCollider boxCollider;
    public GameObject modelObjectWrapper;

    public Transform boundingBoxTransform;
    public Renderer[] renderers;
    Bounds bounds;

    public Coroutine loadModelCoroutine;

    public void Init(LayoutObject controller)
    {
        this.controller = controller;
    }

    public void Refresh()
    {

    }

    public Coroutine LoadModel()
    {
        return loadModelCoroutine = ModelCacheHandler.instance.GetModel(controller.data.data.model, OnModelLoaded);
    }

    public void SetBoundingBox(bool value)
    {
        boundingBoxTransform.gameObject.SetActive(value);
    }

    public void OnModelLoaded(GameObject modelObject)
    {
        if (modelObjectWrapper != null) GameObject.Destroy(modelObjectWrapper);

        modelObjectWrapper = modelObject;

        modelObjectWrapper.transform.SetParent(controller.transform);
        modelObjectWrapper.transform.localPosition = Vector3.zero;
        modelObjectWrapper.transform.localRotation = Quaternion.identity;
        modelObjectWrapper.transform.localScale = Vector3.one;

        CreateBounds();

        controller.UpdateTransform();

        loadModelCoroutine = null;
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

            boxCollider.center = transform.InverseTransformPoint(bounds.center);
            boxCollider.size = bounds.size;

            boundingBoxTransform.localPosition = boxCollider.center;
            boundingBoxTransform.localScale = boxCollider.size;

            transform.Translate(Vector3.up * ((bounds.size.y / 2.0f) - (bounds.center.y - transform.position.y)), Space.World);
        }
        else
        {
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
