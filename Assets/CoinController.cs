using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float rotSpeed;
    public float bobSpeed;
    public float bobRange;

    float startY;
    float bobPos;

    void Start()
    {
        startY = transform.position.y;
        bobPos = Random.Range(0, 2 * Mathf.PI);
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            startY + Mathf.Sin(bobPos) * bobRange,
            transform.position.z
        );
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);

        bobPos += bobSpeed * Time.deltaTime;
    }
}
