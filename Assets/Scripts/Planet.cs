using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int dayLength;
    public float maxHeight;

    void Start() {
        GameObject.Find("Camera").GetComponent<CameraController>().AttachToPlanet(this);
    }

    void FixedUpdate() {
        float rotationPerSec = 365f / dayLength;
        this.transform.rotation = Quaternion.AngleAxis(rotationPerSec * Time.deltaTime, Vector3.back) * this.transform.rotation;
    }
}
