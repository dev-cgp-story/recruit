using System.Collections.Generic;
using UnityEngine;

public class ResourceDataSource : IResourceDataSource
{
    private Dictionary<int, Material> materialTable = new Dictionary<int, Material>();
    private Texture dongTexture;
    private Shader shader;

    public ResourceDataSource(Texture dongTexture, Shader shader)
    {
        this.dongTexture = dongTexture;
        this.shader = shader;
    }

    public Material GetDongMaterial(int height)
    {
        if (materialTable.ContainsKey(height))
            return materialTable[height];

        var mat = new Material(shader);
        mat.mainTexture = dongTexture;
        mat.SetTextureScale("_BaseMap", new Vector2(1, height));
        materialTable.Add(height, mat);
        return mat;
    }
}