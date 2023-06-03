using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public float rotateSpeed;       //camera rotate speed
    public float score;             //player's score
    public float fireCd;            //cool down time
    public float bulletSpeed;       //bullet's speed

    public GameObject bullet;
    
    private Transform _tr;
    private Vector3 _centerPos;

    private float _fireCounter;

    void Start()
    {
        _tr = GetComponent<Transform>();
        _centerPos = Vector3.zero;
        _centerPos.y = _tr.position.y;
    }


    void Update()
    {
        _fireCounter += Time.deltaTime;
        
        //press "A" or "D" to rotate the camera
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _tr.RotateAround(_centerPos, Vector3.up, -Input.GetAxis("Horizontal") * rotateSpeed);
        }

        //press left mouse button to fire
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        
        if (_fireCounter >= fireCd)
        {
            GameObject tempBullet = Transform.Instantiate(bullet);
            tempBullet.transform.position = _tr.position;
            tempBullet.GetComponent<Rigidbody>().velocity = ray.direction * bulletSpeed;
            _fireCounter = 0;
        }
    }
}
