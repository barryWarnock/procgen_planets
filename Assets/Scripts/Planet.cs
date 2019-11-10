using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int dayLength;
    public float maxHeight;

    public static Vector3 PolarCoord(float angle, float distance) {
        return Quaternion.AngleAxis(angle, Vector3.back) * Vector3.up * distance;
    }

    public Vector3 AlignRotation(Vector3 vector) {
        return transform.rotation * vector;
    }

    void Start() {
        GameObject.Find("Camera").GetComponent<CameraController>().AttachToPlanet(this);
    }

    void FixedUpdate() {
        float rotationPerSec = 365f / dayLength;
        this.transform.rotation = Quaternion.AngleAxis(rotationPerSec * Time.deltaTime, Vector3.back) * this.transform.rotation;
    }
}
