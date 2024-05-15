using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuController : MonoBehaviour
{
    public RectTransform handPoint;
    public RectTransform controllerPoint;
    public Transform gazeInteractorHandPoint;
    public Transform gazeInteractorControllerPoint;

    public RectTransform handMenu;
    public Transform gazeInteractor;

    private GameObject leftControllerPrefab;
    private GameObject rightControllerPrefab;

    void Start()
    {
        OVRCameraRig rig = FindAnyObjectByType<OVRCameraRig>();
        leftControllerPrefab = rig.transform.Find("OVRInteractionComprehensive/OVRControllers/LeftController/OVRControllerVisual/OVRControllerPrefab").gameObject;
        rightControllerPrefab = rig.transform.Find("OVRInteractionComprehensive/OVRControllers/RightController/OVRControllerVisual/OVRControllerPrefab").gameObject;
    }

    void Update()
    {
        if (leftControllerPrefab.activeSelf || rightControllerPrefab.activeSelf)
        {
            handMenu.position = controllerPoint.position;
            handMenu.rotation = controllerPoint.rotation;
            gazeInteractor.position = gazeInteractorControllerPoint.position;
            gazeInteractor.rotation = gazeInteractorControllerPoint.rotation;
        }
        else
        {
            handMenu.position = handPoint.position;
            handMenu.rotation = handPoint.rotation;
            gazeInteractor.position = gazeInteractorHandPoint.position;
            gazeInteractor.rotation = gazeInteractorHandPoint.rotation;
        }
    }
}
