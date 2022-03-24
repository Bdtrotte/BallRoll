using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public Transform sphere;
    [Range(float.Epsilon, 1)]
    public float lerpSpeed;

    public float mouseSpeed;

    Vector2 viewPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cursor.lockState = Cursor.lockState != CursorLockMode.Locked
                ? CursorLockMode.Locked
                : CursorLockMode.None;
        }

        // Over complicated thing to smooth out camera movement.
        var toLerp = 1 - Mathf.Pow(1 - lerpSpeed, Time.deltaTime);
        var toTarget = sphere.position - transform.position;

        // Moving the camera towards the target by the amount determined above.
        transform.position += toTarget * toLerp;

        viewPos.x += Input.GetAxis("Mouse X");
        viewPos.y -= Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.Euler(viewPos.y, viewPos.x, 0);
    }
}
