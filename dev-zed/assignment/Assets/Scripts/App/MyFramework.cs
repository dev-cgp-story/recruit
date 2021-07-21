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
    private string removeField;

    void Awake()
    {
        this.controller = new UnityWorldController(new MockRemoteDataSource(), new ResourceDataSource(roomTexture, shader));
    }

    void Update()
    {

    }

    void OnGUI()
    {
        removeField = GUI.TextField(new Rect(50, 50, 100, 50), removeField);
        if (GUI.Button(new Rect(50, 100, 100, 50), "Remove Dong"))
        {
            controller.OnEvent("REMOVE", removeField);
        }
    }
}