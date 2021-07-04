using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public List<Cube> Cubes = new List<Cube>();
    
    [SerializeField] private Material _startCubeMaterial;
    [SerializeField] private Material _inactiveMaterial;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _cameraViewChange = 10;

    private Transform _camera;
    private bool _firstJump = true;

    private void Start()
    {
        _camera = Camera.main.transform;
        SetupCubes();
    }

    private void SetupCubes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Cubes.Add(transform.GetChild(i).GetComponent<Cube>());
        }

        foreach (var cube in Cubes)
        {
            cube.Init(JoinCube, RemoveCube, () => { LevelManager.Instance.Finish();} );
        }
    }

    public void JoinCube(Cube cube)
    {
        cube.transform.parent = transform;
        var y = Cubes.Count * Cubes[0].transform.localScale.y + .1f;
        cube.transform.localPosition = new Vector3(0, y, 0);
        cube.transform.tag = "Untagged";
        var rotation = new Vector3(_camera.transform.eulerAngles.x - _cameraViewChange, _camera.transform.eulerAngles.y);
        _camera.transform.DORotate(rotation, .7f).SetEase(Ease.InOutSine);
        cube.MeshRenderer.material = _startCubeMaterial;
        cube.Init(JoinCube, RemoveCube, () => { LevelManager.Instance.Finish();});
        Cubes.Add(cube);
    }
    
    public void RemoveCube(Cube cube)
    {
        cube.MeshRenderer.material = _inactiveMaterial;
        cube.transform.parent = null;
        cube.Rigidbody.velocity = Vector3.zero;
        var rotation = new Vector3(_camera.transform.eulerAngles.x + _cameraViewChange, _camera.transform.eulerAngles.y);
        _camera.transform.DORotate(rotation, .7f).SetEase(Ease.InOutSine);
        Cubes.Remove(cube);
        if (Cubes.Count == 0)
        {
            LevelManager.Instance.GameOver();
        }
    }

    private void FixedUpdate()
    {
        foreach (var cube in Cubes)
        {
            cube.transform.localPosition = new Vector3(0,cube.transform.localPosition.y, 0);
        }
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
        _camera.position += transform.forward * _speed * Time.deltaTime;

        HandleJump();
    }

    private void HandleJump()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_firstJump || Cubes.Count == 0)
            {
                _firstJump = false;
                return;
            }

            if (IsGrounded())
            {
                foreach (var cube in Cubes)
                {
                    cube.Rigidbody.velocity = Vector3.zero;
                    cube.Rigidbody.AddForce(Vector3.up * _jumpForce * cube.Rigidbody.mass);
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) && Vector3.Angle(Vector3.down, GetLowerCube().Rigidbody.velocity) > 5)
        {
            foreach (var cube in Cubes)
            {
                cube.Rigidbody.velocity = Vector3.zero;
            }
        }
    }

    private bool IsGrounded()
    {
        var lowerCube = GetLowerCube();
        if (Physics.Raycast(lowerCube.transform.position, Vector3.down, lowerCube.transform.localScale.y / 2 + .3f))
        {
            return true;
        }

        return false;
    }

    private Cube GetLowerCube()
    {
        var lowerPosition = 100;
        Cube lower = null;
        foreach (var cube in Cubes)
        {
            if (cube.transform.position.y < lowerPosition)
            {
                lower = cube;
            }
        }

        return lower;
    }
}
