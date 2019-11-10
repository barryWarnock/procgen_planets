using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Planet target;
    public float angle;
    public float zoomScale = 1.5f;
    public float speed = 1;
    public float minSize = 100;
    public float maxScale = 1.5f;
    protected Camera cameraGC;
    void Start() {
        cameraGC = this.GetComponent<Camera>();
    }
    public void AttachToPlanet(Planet planet) {
        target = planet;
        this.transform.parent = planet.transform;
    }

// Update is called once per frame
    void Update()
    {
        if (target != null) {
            if (cameraGC.orthographicSize < target.maxHeight) {
                Vector3 gimble = transform.rotation * Vector3.up * (target.maxHeight+1);
                gimble.z = -10;
                Vector3 towardsPlanet = target.transform.position - gimble;
                Debug.DrawLine(target.transform.position, gimble, Color.red, 1);
                towardsPlanet = towardsPlanet.normalized;
                Ray ray = new Ray(gimble, towardsPlanet*target.maxHeight);
                float distance = Physics2D.Raycast(gimble, towardsPlanet, target.maxHeight).distance;
                this.transform.position = gimble + towardsPlanet * distance;
                this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back)*target.transform.rotation;
            } else {
                this.transform.position = target.transform.position+Vector3.back*10;
            }
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back)*target.transform.rotation;
        }
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.RightArrow)) {
            this.angle += speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            this.angle -= speed * Time.deltaTime;
        }

            Debug.Log("here");
        if(Input.GetKey(KeyCode.DownArrow)) {
            cameraGC.orthographicSize *= zoomScale;
        } else if(Input.GetKey(KeyCode.UpArrow)) {
            cameraGC.orthographicSize /= zoomScale;
        }
        cameraGC.orthographicSize = Mathf.Clamp(cameraGC.orthographicSize, minSize, target.maxHeight * maxScale);
    }
}
