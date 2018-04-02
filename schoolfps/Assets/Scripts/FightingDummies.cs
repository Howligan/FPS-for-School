using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FightingDummies : MonoBehaviour
{

    [SerializeField]
    private GameObject target;
    public float visionRadius;
    public bool hasTarget { get; set; }
    public int health = 100;
    public bool isDead;
    public int lookSpeed;
    public float bulletForce;
    public Animator animator;

    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;
    private bool boltIsCycling = false;
    private float timeToFire = 0f;
    public ParticleSystem muzzleflash;

    public GameObject myGun;

    void Start()
    {
        if (this.gameObject.layer == 8)
        {
            maxAmmo = 8;
        }

        if (this.gameObject.layer == 9)
        {
            maxAmmo = 5;
        }

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isDead == false)
        {
            FindTarget();
            AimAtTarget();
            iAmDead();
            ShootTarget();
            GunReload();
        }

    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius, ((1 << 8)|(1 << 10)));

        foreach (Collider hitCol in hits)
        {
            if (hitCol.gameObject.layer != this.gameObject.layer)
            {
                if (hitCol.gameObject.GetComponent<testdummy>())
                {
                    target = hitCol.gameObject;
                    hasTarget = true;

                    Debug.Log(target);
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
            if (hit.collider.GetComponent<testdummy>())
            {
                if (isReloading == false && boltIsCycling == false && currentAmmo > 0)
                {
                    muzzleflash.Play();
                    currentAmmo--;

                    UKBoltCycle();

                    int hitChance = Random.Range(0, 100);

                    if (hitChance >= 50)
                    {
                        var rbComp = hit.collider.GetComponent<Rigidbody>();
                        var impactForce = (target.transform.position - transform.position).normalized;

                        if (rbComp)
                            rbComp.AddForceAtPosition(impactForce * bulletForce, hit.point, ForceMode.Impulse);

                        
                    }

                }
            }
        }
    }

    void GunReload()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            if (this.gameObject.layer == 8)
            {
                StartCoroutine(UKReload());
            }

            if (this.gameObject.layer == 9)
            {
                StartCoroutine(DEReload());
            }
        }
    }

    IEnumerator UKReload()
    {
        isReloading = true;
        //animator set bool reloading true (LEBEL ANIMATIONS)
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime - .25f);
        currentAmmo = maxAmmo;
        yield return new WaitForSeconds(.25f);

        //animator set bool reloading to false (no animations lol)
        animator.SetBool("isReloading", false);
        isReloading = false;
    }

    IEnumerator DEReload()
    {
        isReloading = true;
        //animator set bool reloading true (MAUSER ANIMATIONS)
        yield return new WaitForSeconds(reloadTime - .25f);
        currentAmmo = maxAmmo;
        yield return new WaitForSeconds(.25f);

        //animator set bool reloading to false (no animations lol)
        isReloading = false;
    }

    IEnumerator UKBoltCycle()
    {
        boltIsCycling = true;
        animator.SetBool("boltIsCycling", true);
        // play bolt cycle animation (LEBEL ANIMATION)
        yield return new WaitForSeconds(.2f);
        animator.SetBool("boltIsCycling", false);
        boltIsCycling = false;
    }

    IEnumerator DEBoltCycle()
    {
        boltIsCycling = true;
        // play bolt cycle animation (MAUSER ANIMATION)
        yield return new WaitForSeconds(.2f);
        boltIsCycling = false;
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