using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleScript : MonoBehaviour {

    public float rifleDamage;
    public float rifleRange;
    public float impactForce;

    public Camera fpsCam;
    

	void Start () {
		
	}
	
	void Update () {
		
	}

    void RifleShoot()
    {
        RaycastHit rifleHitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rifleHitInfo, rifleRange))
        {
            SoldierAI enemy = rifleHitInfo.transform.GetComponent<SoldierAI>();
            if (enemy != null)
            {
                enemy.health = 0;
            }

            if (rifleHitInfo.rigidbody != null)
            {
                rifleHitInfo.rigidbody.AddForce(-rifleHitInfo.normal * impactForce);
            }
        }
    }
}

