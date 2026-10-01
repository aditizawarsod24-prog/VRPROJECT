using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Rigidbody))]
public class NailController : MonoBehaviour
{
    public Transform tip;
    public int maxHits = 3;
    public float initialEmbedDepth = 0.01f;
    public float plungeStep = 0.015f;
    public LayerMask woodLayer;
    public float raycastDistance = 0.5f;
    public float hitCooldown = 0.3f;

    private int hitCount = 0;
    private bool isPlunged = false;
    private float lastHitTime = -10f;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    public bool CanBeHit()
    {
        return hitCount < maxHits && Time.time - lastHitTime > hitCooldown;
    }

    public void RegisterHammerHit()
    {
        if (!CanBeHit()) return;
        lastHitTime = Time.time;

        if (!isPlunged) TryFirstPlunge();
        else AdvancePlunge();
    }

    private void TryFirstPlunge()
    {
        Vector3 origin = tip.position - transform.forward * 0.05f;
        if (!Physics.Raycast(origin, transform.forward, out RaycastHit hit, raycastDistance, woodLayer))
            return;

        if (grabInteractable.isSelected)
        {
            List<IXRSelectInteractor> interactors = new List<IXRSelectInteractor>(grabInteractable.interactorsSelecting);
            foreach (IXRSelectInteractor interactor in interactors)
                grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);
        }
        grabInteractable.enabled = false;

        rb.isKinematic = true;

        Vector3 delta = hit.point - tip.position;
        transform.position += delta;
        transform.position += transform.forward * initialEmbedDepth;

        //transform.SetParent(hit.collider.transform, true);

        isPlunged = true;
        hitCount = 1;
    }

    private void AdvancePlunge()
    {
        transform.position += transform.forward * plungeStep;
        hitCount++;
    }
}