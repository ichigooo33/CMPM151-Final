using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform startPoint;

    public TMP_Text scoreText;
    public TMP_Text goalText;
    public TMP_Text backText;

    public float moveForce;
    public float maxMoveSpeed;
    public float maxFallSpeed;

    public float waitTimeAfterHit = 1.5f;

    private bool _isFailed;
    private int _score;
    private Vector2 _moveInput;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        goalText.gameObject.SetActive(false);
        backText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isFailed)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            }
            else
            {
                _moveInput = Vector2.zero;
            }

            int tempScore = Mathf.FloorToInt((transform.position.y - 0.5f) / -30);
            if (_score != tempScore)
            {
                _score = tempScore;
                scoreText.text = "Score: " + _score;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                BackToStartPoint();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isFailed)
        {
            if (_moveInput != Vector2.zero)
            {
                _rb.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * moveForce);
            }
        }
        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -maxMoveSpeed, maxMoveSpeed), Mathf.Clamp(_rb.velocity.y, -maxFallSpeed, maxFallSpeed), Mathf.Clamp(_rb.velocity.z, -maxMoveSpeed, maxMoveSpeed));
    }

    private void BackToStartPoint()
    {
        backText.gameObject.SetActive(false);
        _isFailed = false;
        transform.position = startPoint.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            backText.gameObject.SetActive(true);
            _isFailed = true;
            Invoke("BackToStartPoint", waitTimeAfterHit);
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("Goal"))
        {
            goalText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                BackToStartPoint();
                goalText.gameObject.SetActive(false);
            }
        }
    }
}
