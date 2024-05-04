using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttached : XRGrabInteractable
{
    public Transform leftAttachedTransform;
    public Transform rightAttachedTransform;

    //protected override void OnSelectEntered(SelectEnterEventArgs args)
    //{
    //    if (args.interactorObject.transform.CompareTag("Left Hand"))
    //    {
    //        attachTransform = leftAttachedTransform;
    //    }
    //    else if (args.interactorObject.transform.CompareTag("Right Hand"))
    //    {
    //        attachTransform = rightAttachedTransform;
    //    }

    //    base.OnSelectEntered(args);
    //}

    public override Transform GetAttachTransform(IXRInteractor interactor)
    {
        Transform i_attachTransform = null;

        if (interactor.transform.CompareTag("Left Hand"))
        {
            i_attachTransform = leftAttachedTransform;
        }
        else if (interactor.transform.CompareTag("Right Hand"))
        {
            i_attachTransform = rightAttachedTransform;
        }

        return i_attachTransform != null ? i_attachTransform : base.GetAttachTransform(interactor);
    }
}
