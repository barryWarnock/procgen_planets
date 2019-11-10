using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Planet target;
    public float angle;
    public void AttachToPlanet(Planet planet) {
        target = planet;
        this.transform.parent = planet.transform;
    }

// Update is called once per frame
    void Update()
    {
        if (target != null) {
            Vector3 gimble = Quaternion.AngleAxis(angle, Vector3.back) * (target.transform.rotation * Vector3.up * (target.maxHeight + 1));
            gimble.z = -10;
            Vector3 towardsPlanet = target.transform.rotation * Vector3.down;
            towardsPlanet = towardsPlanet.normalized;
            Ray ray = new Ray(gimble, towardsPlanet*10000);
            float distance = Physics2D.Raycast(gimble, towardsPlanet, target.maxHeight).distance;
            this.transform.position = gimble + towardsPlanet * distance;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back)*target.transform.rotation;
        }
    }
}
