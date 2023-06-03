using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveForce;
    public float maxMoveSpeed;
    public float maxFallSpeed;

    private Vector2 _moveInput;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        }
        else
        {
            _moveInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (_moveInput != Vector2.zero)
        {
            _rb.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * moveForce);
        }

        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -maxMoveSpeed, maxMoveSpeed), Mathf.Clamp(_rb.velocity.y, -maxFallSpeed, maxFallSpeed), Mathf.Clamp(_rb.velocity.z, -maxMoveSpeed, maxMoveSpeed));
    }
}
