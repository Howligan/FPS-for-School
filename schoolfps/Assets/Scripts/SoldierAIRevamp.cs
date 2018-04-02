using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAIRevamp : MonoBehaviour {

    public GameObject target;
    public float visionRadius;
    public bool hasTarget { get; set; }
    public bool isDead = false;
    public int howMuchTurn = 100;
    public float bulletForce;
    public int TeamID;
    public bool isReloading = false;
    public bool BoltIsCycling = false;
    public float reloadTime = 2f;
    public int currentAmmo;
    public int maxAmmo;

    private void Start()
    {
        AssignTeamID();
        AssignAmmo();
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isDead == false)
        {
            FindTarget();
        }

        if (currentAmmo <= 0)
        {
           StartCoroutine(Reloading());
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius, ((1<<8) | (1<<9)));
        foreach (Collider hitCol in hits)
        {
            if(hitCol.gameObject.layer != this.gameObject.layer)
            {
                if (hitCol.gameObject.GetComponent<SoldierAIRevamp>().isDead == false)
                {
                    target = hitCol.gameObject;
                    hasTarget = true;
                    AimAtTarget();

                    Debug.Log("I have found the target");
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * howMuchTurn);
        ShootTarget();

        Debug.Log("I am looking at the target");
    }

    void ShootTarget()
    {
        Vector3 shootDir = target.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootDir, out hit))
        {
            if (hit.collider.GetComponent<SoldierAIRevamp>() && hit.collider.GetComponent<SoldierAIRevamp>().isDead == false)
            {
                if (BoltIsCycling == false && isReloading == false && currentAmmo > 0)
                {
                    currentAmmo--;
                    var rbComp = hit.collider.GetComponent<Rigidbody>();
                    var impactForce = (target.transform.position - transform.position).normalized;
                    if (rbComp)
                        rbComp.AddForceAtPosition(impactForce * bulletForce, hit.point, ForceMode.Impulse);
                    target.GetComponent<SoldierAIRevamp>().isDead = true;
                    StartCoroutine(BoltCycle());
                    Debug.Log("I am shooting the target");
                } 
            }
        }
    }

    void AssignTeamID()
    {
        if (this.gameObject.layer == 8)
        {
            TeamID = 1;
        }

        if (this.gameObject.layer == 9)
        {
            TeamID = 2;
        }


    }

    void AssignAmmo()
    {
        if (TeamID == 1)
        {
            maxAmmo = 8;
        }

        if (TeamID == 2)
        {
            maxAmmo = 5;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 lookDir = target.gameObject.transform.position - transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, lookDir);
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    // IENUMERATOR ZONE IENUMERATOR ZONE IENUMERATOR ZONE IENUMERATOR ZONE  IENUMERATOR ZONE  IENUMERATOR ZONE  IENUMERATOR ZONE  IENUMERATOR ZONE  IENUMERATOR ZONE 

    IEnumerator Reloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(2);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator BoltCycle()
    {
        BoltIsCycling = true;
        //animation goes here
        yield return new WaitForSeconds(1);
        BoltIsCycling = false;
    }
}