using System;
using UnityEngine;
using UniRx;
using World;

public class MyFramework : MonoBehaviour
{
    [SerializeField]
    private Material roomMaterial;

    private IWorldController controller;

    void Awake()
    {
        this.controller = new UnityWorldController(new MockRemoteDataSource(), new ResourceDataSource(roomMaterial));
    }

    void Update()
    {

    }
}