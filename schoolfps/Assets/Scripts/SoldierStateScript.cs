using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStateScript : MonoBehaviour {

    public enum SoldierAIState { Searching, Aiming, Shooting, Reloading, Dead }

    public SoldierAIState currentSoldierState;

    GameObject currentTarget;
    public int visionRadius;
    public int aimSpeed;

    public int maxAmmo;
    public int currentAmmo;
    public bool BoltisCycling;
    public bool isReloading;
    public float timeToReload;
    public float reloadTime;

    // Use this for initialization
    void Start()
    {
        if (this.gameObject.layer == 8)
        {
            maxAmmo = 8;
            reloadTime = 9;
        }

        if (this.gameObject.layer == 9)
        {
            maxAmmo = 5;
            reloadTime = 4;
        }

        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentSoldierState)
        {
            case SoldierAIState.Searching:
                Searching();
                break;
            case SoldierAIState.Aiming:
                Aiming();
                break;
            case SoldierAIState.Shooting:
                Shooting();
                break;
            case SoldierAIState.Reloading:
                Reloading();
                break;
            case SoldierAIState.Dead:
                Dead();
                break;
        }
    }

    void Searching()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius, ((1 << 8) | (1 << 9)));
        foreach (Collider hitCol in hits)
        {
            if (hitCol.gameObject.layer != this.gameObject.layer)
            {
                if (hitCol.gameObject.GetComponent<SoldierStateScript>().currentSoldierState != SoldierAIState.Dead)
                {
                    currentTarget = hitCol.gameObject;
                }
            }
        }

        if (currentTarget != null)
        {
            currentSoldierState = SoldierAIState.Aiming;
        }
    }

    void Engaging()
    {

    }

    void Aiming()
    {
        Vector3 aimDir = (currentTarget.gameObject.transform.position - transform.position).normalized;
        aimDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(aimDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * aimSpeed);
        if ((transform.rotation.eulerAngles - rotation.eulerAngles).magnitude < 0.1f)
            currentSoldierState = SoldierAIState.Shooting;

        if (currentTarget.GetComponent<SoldierStateScript>().currentSoldierState == SoldierAIState.Dead)
        {
            currentTarget = null;
            currentSoldierState = SoldierAIState.Searching;
        }
    }

    void Shooting()
    {
        Vector3 shootDir = currentTarget.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootDir, out hit))
        {
            if (hit.collider.GetComponent<SoldierStateScript>().currentSoldierState != SoldierAIState.Dead)
            {
                if (BoltisCycling == false && isReloading == false && currentAmmo >= 0)
                {
                    currentAmmo--;
                    var rbComp = hit.collider.GetComponent<Rigidbody>();
                    var impactForce = (currentTarget.transform.position - transform.position).normalized;
                    if (rbComp)
                        rbComp.AddForceAtPosition(impactForce * 2, hit.point, ForceMode.Impulse);
                    currentTarget.GetComponent<SoldierStateScript>().currentSoldierState = SoldierAIState.Dead;

                    if(currentAmmo <= 0)
                    {
                        currentSoldierState = SoldierAIState.Reloading;
                    }

                    StartCoroutine(CyclingTheBolt());
                    
                }
            }
        }
    }

    void Reloading()
    {
        StartCoroutine(StartReloading());
    }

    void Dead()
    {
        currentSoldierState = SoldierAIState.Dead;
        //enable ragdoll
    }

    IEnumerator CyclingTheBolt()
    {
        if (isReloading == false)
        {
            BoltisCycling = true;
            yield return new WaitForSeconds(2);
            BoltisCycling = false;
            currentSoldierState = SoldierAIState.Searching;
        }
    }

    IEnumerator StartReloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
