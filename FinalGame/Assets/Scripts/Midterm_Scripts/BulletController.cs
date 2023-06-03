using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private DeadZone _deadZoneScript;

    private void Start()
    {
        _deadZoneScript = GameObject.FindWithTag("DeadZone").GetComponent<DeadZone>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("DeadZone"))
        {
            _deadZoneScript.BulletImpact();
        }
    }
}
