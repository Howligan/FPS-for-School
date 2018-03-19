using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DESoldier : MonoBehaviour {

    private GameObject target;
    public float visionRadius = 10;
    public bool hasTarget { get; set;}
    public int health = 100;
    public bool isDead;
    public int lookSpeed;

    void Update ()
    {
        FindTarget();
        AimAtTarget();
        //ShootTarget();
        iAmDead();
	}

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius);

        foreach(Collider hitCol in hits)
        {
            if (hitCol.gameObject.GetComponent<UKSoldier>())
            {
                if (hitCol.gameObject.GetComponent<UKSoldier>().isDead == false)
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

        //ShootTarget();
    }

    void iAmDead()
    {
        if(health == 0)
        {
            isDead = true;
        }
    }

    void ShootTarget()
    {
        //target.GetComponent<UKSoldier>().health = 0;
    }
}
