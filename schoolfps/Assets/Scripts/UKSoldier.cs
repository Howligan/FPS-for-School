using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UKSoldier : MonoBehaviour
{

    private GameObject target;
    public float visionRadius = 10;
    public bool hasTarget { get; set; }
    public int health = 100;
    public bool isDead;
    public int lookSpeed;
    public float bulletForce;


    void Update()
    {
        FindTarget();
        AimAtTarget();
        iAmDead();
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius);

        foreach (Collider hitCol in hits)
        {
            if (hitCol.gameObject.GetComponent<DESoldier>())
            {
                if (hitCol.gameObject.GetComponent<DESoldier>().isDead == false)
                {
                    target = hitCol.gameObject;
                    hasTarget = true;
                }

            }
        }
    }

    void AimAtTarget()
    {
        if (!hasTarget)
            return;

        Vector3 lookDir = target.gameObject.transform.position - transform.position;
        lookDir.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);

        ShootTarget();
    }

    void iAmDead()
    {
        if (health == 0)
        {
            isDead = true;
        }
    }

    void ShootTarget()
    {
        Vector3 shootDir = target.transform.position - transform.position;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, shootDir, out hit))
        {
            if (hit.collider.GetComponent<DESoldier>())
            {
                var rbComp = hit.collider.GetComponent<Rigidbody>();
                var impactForce = (target.transform.position - transform.position).normalized;

                if (rbComp)
                    rbComp.AddForceAtPosition(impactForce * bulletForce, hit.point, ForceMode.Impulse);

                target.GetComponent<DESoldier>().isDead = true;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 lookDir = target.gameObject.transform.position - transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, lookDir);
    }
}
