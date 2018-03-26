using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour {


    private GameObject target;
    public float visionRadius;
    public bool hasTarget { get; set; }
    public int health = 100;
    public bool isDead;
    public int lookSpeed;
    public float bulletForce;

    void Update()
    {
        if (isDead == false)
        {
            FindTarget();
            AimAtTarget();
            iAmDead();
            ShootTarget();
        }
        
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius,((1<<8) | (1<<9)));

        foreach (Collider hitCol in hits)
        {
            if (hitCol.gameObject.layer != this.gameObject.layer)
            {
                if (hitCol.gameObject.GetComponent<SoldierAI>().isDead == false)
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

        if (Physics.Raycast(transform.position, shootDir, out hit))
        {
            if (hit.collider.GetComponent<SoldierAI>() && hit.collider.GetComponent<SoldierAI>().isDead == false)
            {
                var rbComp = hit.collider.GetComponent<Rigidbody>();
                var impactForce = (target.transform.position - transform.position).normalized;

                if (rbComp)
                    rbComp.AddForceAtPosition(impactForce * bulletForce, hit.point, ForceMode.Impulse);

                target.GetComponent<SoldierAI>().isDead = true;
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
// Need to add rate of fire, reload time, shoot time, 
// move towards target, maybe miss chance %
// make them move towards enemy (need nav mesh?)