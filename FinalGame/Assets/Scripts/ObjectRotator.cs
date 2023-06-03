using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float quickRotateInterval;
    
    public float normalRotateAngle;
    public float quickRotateAngle;
    public float quickRotateTime;
    
    private bool _isQuickRotate;
    private float _quickRotateIntervalTimer;
    private float _quickRotateStopTimer;
    private void Update()
    {
        if (!_isQuickRotate)
        {
            _quickRotateIntervalTimer += Time.deltaTime;
            
            if (_quickRotateIntervalTimer >= quickRotateInterval)
            {
                _isQuickRotate = true;
                _quickRotateIntervalTimer = 0;
            }
        }
        
        if (_isQuickRotate)
        {
            StartQuickRotate();
        }
        
        transform.Rotate(Vector3.up, normalRotateAngle * Time.deltaTime);
    }

    private void StartQuickRotate()
    {
        _quickRotateStopTimer += Time.deltaTime;
        transform.Rotate(Vector3.up, quickRotateAngle * Time.deltaTime);
        
        if (_quickRotateStopTimer >= quickRotateTime)
        {
            _isQuickRotate = false;
            _quickRotateStopTimer = 0;
        }
    }
}
