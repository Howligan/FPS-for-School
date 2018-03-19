using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingai : MonoBehaviour {

    public Transform target;
    public float visionRadius = 60;
    public bool hasTarget { get; set; }

    public testingai aiBrain;

    public int health = 100;

    public bool isDead;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        FindTarget();
        AimAtTarget();
        iAmDead();

        Debug.Log(isDead);
	}

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, visionRadius);

        foreach(Collider hitCol in hits)
        {
            if(hitCol.tag == "DE Soldier")
            {
                target = hitCol.transform;
                aiBrain = hitCol.GetComponent<testingai>();
                hasTarget = true;
            }
        }
    }

    void AimAtTarget()
    {
        if (!target)
            return;

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
    }

    void iAmDead()
    {
       /* if ()
        {
            isDead = true;
        }*/
    }
}
