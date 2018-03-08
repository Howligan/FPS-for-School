using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    public float mouseSensitivity;
    private float xAxisClamp = 0.0f;
    public Transform playerBody;

    // Use this for initialization

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RotateCamera(); // Making a method outside of the update and using update to call it is much easier to work with.
        Cursor.lockState = CursorLockMode.Locked; // This prevents the cursor from moving around. It is locked to the center of the screen.
    }

    void RotateCamera() // IMPORTANT NOTE: This code only works for mouse movement as of 3/8/18. Controller will be implemented later.
    {
        float mouseX = Input.GetAxis("Mouse X"); // This gets the mouse position from the mouse's x. It is like the default
        float mouseY = Input.GetAxis("Mouse Y"); // This gets the mouse position from the mouse's y. It is like the default

        float rotAmountX = mouseX * mouseSensitivity; //This multiplies the mouse's default X value by the sensitivity. This is so the player can change the sensitivity
        float rotAmountY = mouseY * mouseSensitivity; //This multiplies the mouse's default y value by the sensitivity. This is also so the player can change the sensitivity

        Vector3 targetRotCam = transform.rotation.eulerAngles; // targetRotCam is used to get the current rotation of the camera. The Vector 3 is converted into Euler.
        Vector3 targetRotBody = playerBody.rotation.eulerAngles; // targetRotBody is used to get the rotation of the body. It is a vec3 converted into euler.

        // It is X > Y, and Y > X, because we are rotating around the Y axis using the mouse X and vice versa.
        targetRotCam.x -= rotAmountY; // VERTICAL looking (up and down). The -= is to invert the rotation.
        targetRotCam.z = 0; // This is to stop the camera from flipping when you look too far down or up.
        targetRotBody.y += rotAmountX; // Syncs the y camera(HORIZONTAL) rotation with the body's rotation.

        xAxisClamp -= rotAmountY;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }

        transform.rotation = Quaternion.Euler(targetRotCam); // Converting the euler into a vector 3
        playerBody.rotation = Quaternion.Euler(targetRotBody); // Converting the euler into vec3
    }
}
