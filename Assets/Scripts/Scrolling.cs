using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posChange = Input.mouseScrollDelta;
        transform.position += posChange;

        if (Input.GetKeyDown("w")) cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - 5, 5, 30);
        if (Input.GetKeyDown("s")) cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + 5, 5, 30);
    }
}
