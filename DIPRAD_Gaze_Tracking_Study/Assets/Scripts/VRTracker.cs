using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Photon.Pun;
using UnityEngine.XR;

public class VRTracker : MonoBehaviour
{
    public Transform head;

    private PhotonView photonView;

    private Transform headRig;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        OVRCameraRig rig = FindAnyObjectByType<OVRCameraRig>();
        headRig = rig.transform.Find("TrackingSpace/CenterEyeAnchor");

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
