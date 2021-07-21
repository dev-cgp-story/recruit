using System;
using UnityEngine;
using UniRx;
using World;

public class MyFramework : MonoBehaviour
{
    [SerializeField]
    private Texture roomTexture;

    [SerializeField]
    private Shader shader;

    private IWorldController controller;

    void Awake()
    {
        this.controller = new UnityWorldController(new MockRemoteDataSource(), new ResourceDataSource(roomTexture, shader));
    }

    void Update()
    {

    }
}