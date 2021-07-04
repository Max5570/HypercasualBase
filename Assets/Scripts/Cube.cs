using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public MeshRenderer MeshRenderer;
    
    private Action<Cube> _onHitObstacle;
    private Action<Cube> _onHitCube;
    private Action _onFinish;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void Init(Action<Cube> onHitCube, Action<Cube> onHitObstacle, Action onFinish)
    {
        _onHitCube = onHitCube;
        _onHitObstacle = onHitObstacle;
        _onFinish = onFinish;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Obstacle")
            && transform.position.y - other.transform.position.y < other.transform.localScale.y / 2)
        {
            _onHitObstacle?.Invoke(this);
        }
        else if (other.transform.CompareTag("Cube"))
        {
            _onHitCube.Invoke(other.transform.GetComponent<Cube>());
        }
        else if (other.transform.CompareTag("Finish"))
        {
            _onFinish.Invoke();
        }
    }
}
