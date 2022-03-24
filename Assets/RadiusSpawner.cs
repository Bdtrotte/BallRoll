using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSpawner : MonoBehaviour
{
    public GameObject obj;
    public int n;
    public float yOffset;
    public float radius;

    [Range(0, 360)]
    public float rotXRange;
    [Range(0, 360)]
    public float rotYRange;
    [Range(0, 360)]
    public float rotZRange;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < n; ++i)
        {
            var g = Instantiate(obj);
            var p = Random.insideUnitCircle * radius;
            g.transform.position = new Vector3(p.x, yOffset, p.y);
            g.transform.rotation = Quaternion.Euler(
                Random.Range(0, rotXRange),
                Random.Range(0, rotYRange),
                Random.Range(0, rotZRange)
            );

            g.transform.SetParent(transform);
        }
    }
}
