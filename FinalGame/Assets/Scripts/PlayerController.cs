using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class PlayerController : MonoBehaviour
{
    public Transform startPoint;

    public TMP_Text scoreText;
    public TMP_Text goalText;
    public TMP_Text backText;

    public float moveForce;
    public float maxMoveSpeed =0.1f;
    public float maxFallSpeed;

    public float waitTimeAfterHit = 1.5f;

    private bool _isFailed;
    private int _score;
    private Vector2 _moveInput;
    private Rigidbody _rb;

    string[] sounds = { "kick", "snare", "hihat","melody","snare2","beep"};




    void Start()
    {

        //************* Instantiate the OSC Handler...
        OSCHandler.Instance.Init();
        //OSCHandler.Instance.SendMessageToClient("pd", "/unity/trigger", "ready");
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/" + sounds[0], 1);
        //*************

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

                //if(_score == 1) {
                //    OSCHandler.Instance.SendMessageToClient("pd", "/unity/snare", 1);
                //    OSCHandler.Instance.SendMessageToClient("pd", "/unity/hihat", 1);
                //}
                //else if(_score >= 3) {
                //    OSCHandler.Instance.SendMessageToClient("pd", "/unity/" + sounds[_score], 1);
                //}


                ////layering instruments
                if (_score < sounds.Length)
                {
                    print("playing: " + sounds[_score]);
                    OSCHandler.Instance.SendMessageToClient("pd", "/unity/" + sounds[_score], 1);
                }

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 1);
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

        //************* Routine for receiving the OSC...
        OSCHandler.Instance.UpdateLogs();
        Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
        servers = OSCHandler.Instance.Servers;
        //*************
    }

    private void BackToStartPoint()
    {
        backText.gameObject.SetActive(false);
        _isFailed = false;
        transform.position = startPoint.position;
        //layering instruments

        for(int i = 0;i< sounds.Length; i++) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/" + sounds[i], 1);
        }
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/kick", 1);
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
