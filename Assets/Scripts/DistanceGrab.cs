using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DistanceGrab : XRBaseInputInteractor
{

    List<XRBaseInteractable> m_ValidTargets = new List<XRBaseInteractable>();
    XRBaseInteractable m_CurrentNearestObject;

    public float m_GrabbingThreshold = 0f;
    public GameObject m_Cursor;
    public Transform m_FwdVector;

    private List<XRBaseInteractable> m_GrabbableItems;
    private SphereCollider m_Coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
        if(!m_Coll)
        {
            m_Coll = gameObject.AddComponent<SphereCollider>();
        }
        m_Coll.radius = .1f;
        m_Coll.isTrigger = true;

        m_Cursor = Instantiate(m_Cursor);
        m_Cursor.SetActive(false);

        m_GrabbableItems = FindObjectsByType<XRBaseInteractable>(FindObjectsSortMode.None).ToList();
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        GetValidTargets(m_ValidTargets);
    }
    public new void GetValidTargets(List<XRBaseInteractable> validTargets)
    {
        validTargets.Clear();

        float bestGuess = 0;
        XRBaseInteractable selectable = null;
        foreach ( XRBaseInteractable obj in m_GrabbableItems)
        {
            Vector3 dir = (obj.transform.position - m_FwdVector.position).normalized;
            float currentGuess = Vector3.Dot(m_FwdVector.forward, dir);

            if (currentGuess > m_GrabbingThreshold && currentGuess > bestGuess)
            {
                bestGuess = currentGuess;
                selectable = obj;
                m_CurrentNearestObject = selectable;
                m_Coll.center = transform.InverseTransformPoint(selectable.transform.position);
                validTargets.Add(selectable);
            }
        }

        if (selectable)
        {
            m_Cursor.SetActive(true);
            m_Cursor.transform.position = selectable.transform.position;
        }
        else
        {
            m_Coll.center = Vector3.zero;
            m_Cursor.SetActive(false);
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        bool selectActivated = m_CurrentNearestObject == interactable || base.CanSelect(interactable);
        return selectActivated && (interactablesSelected == null || interactablesSelected == interactable);
    }
}
