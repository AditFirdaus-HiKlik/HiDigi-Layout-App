using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.Events;

public class InspectorTransform : MonoBehaviour
{
    public static InspectorTransform instance;

    public LayoutObject target => LayoutEditorInspector.instance.selectedObject;
    public Transform targetTransform => target ? target.transform : null;

    public Vector3Field position;
    public Vector3Field rotation;
    public Vector3Field scale;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // Register the event listeners.
        position.onValueChanged.AddListener(SetPosition);
        rotation.onValueChanged.AddListener(SetRotation);
        scale.onValueChanged.AddListener(SetScale);

        // Start the input fields.
        position.Start();
        rotation.Start();
        scale.Start();
    }

    public void Update()
    {
        ValidateTransform();
    }

    public void ValidateTransform()
    {
        if (targetTransform == null) return;

        if (targetTransform.position != position.value) position.SetValue(targetTransform.position);
        if (targetTransform.eulerAngles != rotation.value) rotation.SetValue(targetTransform.eulerAngles);
        if (targetTransform.localScale != scale.value) scale.SetValue(targetTransform.localScale);
    }

    public void SetPosition(Vector3 position) => targetTransform.position = position;
    public void SetRotation(Vector3 rotation) => targetTransform.eulerAngles = rotation;
    public void SetScale(Vector3 scale) => targetTransform.localScale = scale;

    [System.Serializable]
    public class Vector3Field
    {
        public Vector3 value;

        public TMP_InputField inputFieldX;
        public TMP_InputField inputFieldY;
        public TMP_InputField inputFieldZ;

        [HideInInspector] public UnityEvent<Vector3> onValueChanged;

        public void Start()
        {
            // Register the input fields to the event listeners.
            inputFieldX.onValueChanged.AddListener(SetX);
            inputFieldY.onValueChanged.AddListener(SetY);
            inputFieldZ.onValueChanged.AddListener(SetZ);
        }

        // Input
        public void SetValue(Vector3 value)
        {
            this.value = value;
            UpdateInputFields();
        }

        // Output
        public void SetX(string v)
        {
            if (v == "") v = "0";

            this.value.x = float.Parse(v);
            onValueChanged.Invoke(this.value);
        }

        public void SetY(string v)
        {
            if (v == "") v = "0";

            value.y = float.Parse(v);
            onValueChanged.Invoke(value);
        }

        public void SetZ(string v)
        {
            if (v == "") v = "0";

            value.z = float.Parse(v);
            onValueChanged.Invoke(value);
        }

        // To update the input field, we need to convert the value to a string.
        public void UpdateInputFields()
        {
            inputFieldX.text = value.x.ToString("0.00");
            inputFieldY.text = value.y.ToString("0.00");
            inputFieldZ.text = value.z.ToString("0.00");
        }
    }
}
