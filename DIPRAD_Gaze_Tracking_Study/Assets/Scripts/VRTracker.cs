using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Photon.Pun;
using UnityEngine.XR;

public class VRTracker : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftController;
    public Transform rightController;

    private PhotonView photonView;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    private GameObject leftControllerPrefab;
    private GameObject rightControllerPrefab;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        OVRCameraRig rig = FindAnyObjectByType<OVRCameraRig>();
        headRig = rig.transform.Find("TrackingSpace/CenterEyeAnchor");
        leftHandRig = rig.transform.Find("TrackingSpace/LeftHandAnchor");
        rightHandRig = rig.transform.Find("TrackingSpace/RightHandAnchor");

        leftControllerPrefab = rig.transform.Find("OVRInteractionComprehensive/OVRControllers/LeftController/OVRControllerVisual/OVRControllerPrefab").gameObject;
        rightControllerPrefab = rig.transform.Find("OVRInteractionComprehensive/OVRControllers/RightController/OVRControllerVisual/OVRControllerPrefab").gameObject;

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
            foreach (var item in GetComponentsInChildren<Canvas>())
            {
                item.enabled = false;
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);
            MapPosition(leftController, leftHandRig);
            MapPosition(rightController, rightHandRig);

            if (leftControllerPrefab.activeSelf || rightControllerPrefab.activeSelf)
            {
                leftController.gameObject.SetActive(true);
                rightController.gameObject.SetActive(true);
                leftHand.gameObject.SetActive(false);
                rightHand.gameObject.SetActive(false);
            }
            else
            {
                leftController.gameObject.SetActive(false);
                rightController.gameObject.SetActive(false);
                leftHand.gameObject.SetActive(true);
                rightHand.gameObject.SetActive(true);
            }
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
