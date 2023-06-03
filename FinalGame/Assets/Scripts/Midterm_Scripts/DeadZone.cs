using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class DeadZone : MonoBehaviour
{
    public Text scoreText;

    private Collider _col;
    private CamControl _camControl;
    
    //************* Need to setup this server dictionary...
    Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
    //*************

    void Start()
    {
        _col = GetComponent<MeshCollider>();
        _camControl = Camera.main.GetComponent<CamControl>();
        
        Application.runInBackground = true; //allows unity to update when not in focus

        //************* Instantiate the OSC Handler...
        OSCHandler.Instance.Init ();
        OSCHandler.Instance.SendMessageToClient ("pd", "/unity/trigger", "ready");
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 1);
        //*************
    }

    private void FixedUpdate()
    {
        //************* Routine for receiving the OSC...
        OSCHandler.Instance.UpdateLogs();
        Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
        servers = OSCHandler.Instance.Servers;

        foreach (KeyValuePair<string, ServerLog> item in servers) {
            // If we have received at least one packet,
            // show the last received from the log in the Debug console
            if (item.Value.log.Count > 0) {
                int lastPacketIndex = item.Value.packets.Count - 1;

                //get address and data packet
                scoreText.text = item.Value.packets [lastPacketIndex].Address.ToString ();
                scoreText.text += item.Value.packets [lastPacketIndex].Data [0].ToString ();

            }
        }
        //*************
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Pick Up"))
        {
            _camControl.score++;
            scoreText.text = "Score: " + _camControl.score;
            
            //************* Send the message to the client...
            OSCHandler.Instance.SendMessageToClient ("pd", "/unity/score", _camControl.score);
            OSCHandler.Instance.SendMessageToClient ("pd", "/unity/blue", 0);
            //*************
        }
        else if(collision.transform.CompareTag("Non Pick Up"))
        {
            _camControl.score--;
            scoreText.text = "Score: " + _camControl.score;
            
            //************* Send the message to the client...
            OSCHandler.Instance.SendMessageToClient ("pd", "/unity/score", _camControl.score);
            OSCHandler.Instance.SendMessageToClient ("pd", "/unity/red", 0);
            //*************
        }
        
        // change the tempo of the sequence based on how many obejcts we have picked up.
        if(_camControl.score <= -2 || _camControl.score >= 2)
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 800);
        }
        if (_camControl.score <= -4 || _camControl.score >= 4)
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 700);
        }
        if(_camControl.score >= 6)
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 600);
        }
        if (_camControl.score >= 8)
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 500);
        }
        Destroy(collision.gameObject);
    }

    public void BulletImpact()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/impact", 0);
    }
}
