using UnityEngine;
using System.Collections;

public class GunAK47 : MonoBehaviour {

    public float akDamage = 10f;
    public float akRange = 100f;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    private float timeToFire = 0f;

    public Animator animator;

    // Use this for initialization

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update () {

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadAK());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1f / fireRate;
            akShoot();
        }
	}

    void akShoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit akHitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out akHitInfo, akRange))
        {
            Debug.Log(akHitInfo.transform.name);

            SoldierAI enemy = akHitInfo.transform.GetComponent<SoldierAI>();
            if (enemy != null)
            {
                enemy.health = 0;
            }

            if(akHitInfo.rigidbody != null)
            {
                akHitInfo.rigidbody.AddForce(-akHitInfo.normal * impactForce);
            }

            GameObject impactPS = Instantiate(impactEffect, akHitInfo.point, Quaternion.LookRotation(akHitInfo.normal));
            Destroy(impactPS, 1f);
        }
    }

    IEnumerator ReloadAK()
    {
        isReloading = true;

        Debug.Log("Reloading");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        currentAmmo = maxAmmo;
        yield return new WaitForSeconds(.25f);

        animator.SetBool("Reloading", false);
        isReloading = false;
    }
}
