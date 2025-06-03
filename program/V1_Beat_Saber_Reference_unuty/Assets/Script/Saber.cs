using UnityEngine;

public class Saber : MonoBehaviour
{

    public LayerMask layer;
    public Manager manager;

    Vector3 prevPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1, layer))
        {
            Cube cubeScript = hit.transform.GetComponent<Cube>();
            if (Vector3.Angle(transform.position - prevPos, hit.transform.up) > 130)
            {
                manager = FindObjectOfType<Manager>();
                manager.AddScore(1);
                cubeScript.cubeDid = true;
            }
        }

        prevPos = transform.position;

    }
    
    
}
