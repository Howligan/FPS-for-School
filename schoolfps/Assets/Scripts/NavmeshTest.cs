using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshTest : MonoBehaviour {

    Transform destination;
    NavMeshAgent nvAgent;

	// Use this for initialization
	void Start () {
        nvAgent = this.GetComponent<NavMeshAgent>();

        if (nvAgent == null)
        {
            Debug.Log("The nav mesh agent component is not attached to" + gameObject.name);
        }
        else
        {
            SetDestination();
        }
	}
	
	// Update is called once per frame
	void SetDestination () {
		if(destination == null)
        {

        }
	}
}
