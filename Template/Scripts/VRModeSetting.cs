using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.UI;
using Crengine.Util.KeyboardSystem;
using UnityEngine.InputSystem.UI;

public abstract class VRModeSetting : MonoBehaviour
{
    [SerializeField] protected GameObject eventSystem;
    [SerializeField] protected bool enableAutomate;
    [SerializeField] protected GameObject VRComponent;

    [Header("Camera")]
    [SerializeField] protected Camera pcCamera;
    [SerializeField] protected Camera vrCamera;

    [Header("Canvas Location")]
    [SerializeField] protected Canvas mainCanvas;
    [SerializeField] protected float canvasZValue;


    protected void Start()
    {
        if (XRSettings.enabled)
        {
            VRSetting();

            if (enableAutomate)
            {
                AutoWork();
            }
        }
        else
        {
            VRComponent.gameObject.SetActive(false);
        }
    }


    protected void VRSetting()
    {
        VRComponent.gameObject.SetActive(true);
        pcCamera.gameObject.SetActive(false);

        mainCanvas.renderMode = RenderMode.WorldSpace;
        mainCanvas.worldCamera = vrCamera;
        SetCanvasTransform(mainCanvas.GetComponent<RectTransform>());
        mainCanvas.gameObject.AddComponent<TrackedDeviceGraphicRaycaster>();

        if (eventSystem.GetComponent<XRUIInputModule>() == null)
            eventSystem.AddComponent<XRUIInputModule>();

        if (eventSystem.GetComponent<InputSystemUIInputModule>() != null)
            Destroy(eventSystem.GetComponent<InputSystemUIInputModule>());
    }

    protected void SetCanvasTransform(RectTransform rectTransform)
    {
        rectTransform.localScale = Vector3.one * 0.01f;
        rectTransform.position = Vector3.zero + Vector3.forward * canvasZValue;
    }

    protected abstract void AutoWork();
}
