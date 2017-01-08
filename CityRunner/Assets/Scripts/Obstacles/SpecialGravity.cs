using UnityEngine;
using System.Collections;

public class SpecialGravity : MonoBehaviour {

    public float speed = 0.01f;
    public float yMin = 0f;
    public Vector3 direction;

    // Update is called once per frame
    void Update () {

        if(transform.position.y > yMin)
            transform.Translate(direction * speed);


    }
}
