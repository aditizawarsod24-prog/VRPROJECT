using UnityEngine;

public class HammerHead : MonoBehaviour
{
    public Rigidbody hammerRigidbody;
    public float minImpactSpeed = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(">>>>> "+ other.name);
        NailController nail = other.GetComponentInParent<NailController>();
        if (nail == null) return;
        //if (hammerRigidbody.linearVelocity.magnitude < minImpactSpeed) return;
        if (!nail.CanBeHit()) return;

        nail.RegisterHammerHit();
    }
}