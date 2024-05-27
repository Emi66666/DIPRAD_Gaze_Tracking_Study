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

    private SkinnedMeshRenderer leftHand;
    private SkinnedMeshRenderer rightHand;

    void Start()
    {
        OVRCameraRig rig = FindAnyObjectByType<OVRCameraRig>();
        leftHand = rig.transform.Find("OVRInteractionComprehensive/OVRHands/LeftHandGrabUseSynthetic/OVRLeftHandVisual/OculusHand_L/l_handMeshNode").GetComponent<SkinnedMeshRenderer>();
        rightHand = rig.transform.Find("OVRInteractionComprehensive/OVRHands/RightHandGrabUseSynthetic/OVRRightHandVisual/OculusHand_R/r_handMeshNode").GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if (leftHand.enabled || rightHand.enabled)
        {
            handMenu.position = handPoint.position;
            handMenu.rotation = handPoint.rotation;
            gazeInteractor.position = gazeInteractorHandPoint.position;
            gazeInteractor.rotation = gazeInteractorHandPoint.rotation;
        }
        else
        {
            handMenu.position = controllerPoint.position;
            handMenu.rotation = controllerPoint.rotation;
            gazeInteractor.position = gazeInteractorControllerPoint.position;
            gazeInteractor.rotation = gazeInteractorControllerPoint.rotation;
        }
    }
}
