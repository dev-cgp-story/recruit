using UnityEngine;

public class ResourceDataSource : IResourceDataSource
{
    private Material roomMaterial;

    public ResourceDataSource(Material roomMaterial)
    {
        this.roomMaterial = roomMaterial;
    }

    public Material GetRoomMaterial()
    {
        return roomMaterial;
    }
}