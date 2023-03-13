using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;

using Firebase.Firestore;

public class LayoutManager : MonoBehaviour
{
    public static LayoutManager instance;

    public static Photon.Realtime.Player masterClient => PhotonNetwork.MasterClient;

    public static string layoutID = "layout_default";
    public static LayoutData layoutData = new LayoutData();

    public static UnityEvent<bool> OnLayoutDataSyncing = new UnityEvent<bool>();

    public LayoutData _layoutData; // For debugging
    public bool isDataSyncing = false;

    public GameObject layoutObjectPrefab;

    [Header("Events")]
    public UnityEvent<LayoutData> OnLayoutDataLoaded = new UnityEvent<LayoutData>();
    public UnityEvent<LayoutDataProject> OnLayoutDataProjectLoaded = new UnityEvent<LayoutDataProject>();

    ListenerRegistration listenerRegistration;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        InitListener();
    }

    public static void LoadInstance()
    {
        instance.StartCoroutine(LoadInstanceAsync());
    }

    static IEnumerator LoadInstanceAsync()
    {
        yield return LoadLayoutData();

        LayoutObject[] layoutObjects = GameObject.FindObjectsOfType<LayoutObject>();

        foreach (var layoutObject in layoutObjects)
        {
            yield return DestroyLayoutObject(layoutObject);
        }

        Debug.Log("Layout instance cleared");

        if (layoutData == null)
        {
            layoutData = new LayoutData();

            layoutData.id = layoutID;
        }

        foreach (var instance in layoutData.instances)
        {
            yield return InstantiateLayoutObject(instance);
        }

        Debug.Log("Layout instance loaded");
    }

    public static void SaveInstance()
    {
        List<LayoutInstanceData> instances = new List<LayoutInstanceData>();

        LayoutObject[] layoutObjects = GameObject.FindObjectsOfType<LayoutObject>();

        foreach (var layoutObject in layoutObjects)
        {
            instances.Add(layoutObject.GetLayoutInstanceData());
        }

        layoutData.instances = instances;

        Debug.Log("Layout instance saved");

        SaveLayoutData();
    }

    [ContextMenu("Init Listener")]
    public void InitListener()
    {
        listenerRegistration = FirestoreAPI.ListenDocument("sessions", layoutID, (snapshot) =>
        {
            if (snapshot.Exists)
            {
                Dictionary<string, object> layoutDictionary = snapshot.ToDictionary();

                layoutData = LayoutData.FromDictionary(layoutDictionary);

                _layoutData = layoutData;

                OnLayoutDataLoaded.Invoke(layoutData);

                if (layoutData.project != null)
                {
                    OnLayoutDataProjectLoaded.Invoke(layoutData.project);
                }

                Debug.Log("Layout data loaded: " + layoutData.ObjectToJson());
            }
        });

        Debug.Log("Listener initialized");
    }

    public static Coroutine InstantiateLayoutObject(LayoutInstanceData layoutInstanceData)
    {
        return instance.StartCoroutine(InstantiateLayoutObjectAsync(layoutInstanceData));
    }

    public static Coroutine DestroyLayoutObject(LayoutObject layoutObject)
    {
        if (layoutObject == null)
        {
            return null;
        }

        return layoutObject.Delete();
    }

    static IEnumerator InstantiateLayoutObjectAsync(LayoutInstanceData layoutInstanceData)
    {
        OnLayoutDataSyncing.Invoke(true);

        GameObject layoutObject = null;
        LayoutObject layoutObjectComponent = null;

        // 3 Times to try
        for (int i = 0; i < 3; i++)
        {
            layoutObject = PhotonNetwork.Instantiate("LayoutObject", Vector3.zero, Quaternion.identity);

            if (layoutObject != null)
            {
                Debug.Log("LayoutObject is not null, break the loop");
                layoutObjectComponent = layoutObject.GetComponent<LayoutObject>();
                break;
            }
            Debug.Log("LayoutObject is null, try again");
        }

        layoutObjectComponent.SetLayoutInstanceData(layoutInstanceData);

        yield return layoutObjectComponent.view.LoadModel();

        layoutObjectComponent.view.CreateBounds();

        layoutObjectComponent.UpdateTransform();

        layoutObjectComponent.Select();

        OnLayoutDataSyncing.Invoke(false);

        yield return null;
    }

    [ContextMenu("Save Layout")]
    public static Coroutine SaveLayoutData()
    {
        return instance.StartCoroutine(SaveLayoutDataAsync());
    }

    static IEnumerator SaveLayoutDataAsync()
    {
        OnLayoutDataSyncing.Invoke(true);

        try
        {
            yield return new WaitForSeconds(0.5f);

            yield return FirestoreAPI.SetDocument("sessions", layoutID, layoutData.ToDictionary());

            Debug.Log("Layout data saved");
        }
        finally
        {
            OnLayoutDataSyncing.Invoke(false);
        }

        OnLayoutDataSyncing.Invoke(false);

        Debug.Log("Layout data saved");
    }

    [ContextMenu("Load Layout")]
    public static Coroutine LoadLayoutData()
    {
        return instance.StartCoroutine(LoadLayoutDataAsync());
    }

    static IEnumerator LoadLayoutDataAsync()
    {
        OnLayoutDataSyncing.Invoke(true);

        try
        {
            yield return new WaitForSeconds(0.5f);

            DocumentSnapshot snapshot = null;

            yield return FirestoreAPI.GetDocument("sessions", layoutID, (documentSnapshot) =>
            {
                snapshot = documentSnapshot;
            });

            if (snapshot.Exists)
            {
                Dictionary<string, object> layoutDictionary = snapshot.ToDictionary();

                layoutData = LayoutData.FromDictionary(layoutDictionary);

                instance._layoutData = layoutData;

                instance.OnLayoutDataLoaded.Invoke(layoutData);

                if (layoutData.project != null)
                {
                    instance.OnLayoutDataProjectLoaded.Invoke(layoutData.project);
                }

                Debug.Log("Layout data loaded: " + layoutData.ObjectToJson());

                yield return null;
            }
        }
        finally
        {
            OnLayoutDataSyncing.Invoke(false);
        }

        OnLayoutDataSyncing.Invoke(false);
    }

}