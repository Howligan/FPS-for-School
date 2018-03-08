using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private CharacterController charControl;
    public float speed;
   


	// Use this for initialization
	void Awake () {
        charControl = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
         MovePlayer();
        //myControls();
	}

    void MovePlayer()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 moveDirSide = transform.right * hor * speed;
        Vector3 moveDirForward = transform.forward * ver * speed;

        charControl.SimpleMove(moveDirSide);
        charControl.SimpleMove(moveDirForward);
    }

    void myControls()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            Debug.Log("a");
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            Debug.Log("d");
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            Debug.Log("w");
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            Debug.Log("s");
        }
    }
}
