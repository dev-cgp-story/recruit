using System.Collections;
using UnityEngine;
using UniRx;
using World.Dto;

namespace World
{
    public class UnityWorldController : IWorldController
    {
        GameObject World = new GameObject("World");
        private UnityWorldViewModel viewModel;
        private IResourceDataSource resourceDataSource;
        public UnityWorldController(IRemoteDataSource remoteDataSource, IResourceDataSource resourceDataSource)
        {
            this.resourceDataSource = resourceDataSource;
            viewModel = new UnityWorldViewModel(remoteDataSource);
            viewModel.GetDongs()
                .ObserveOnMainThread()
                .Do(dongInfos => DrawDongs(dongInfos))
                .Subscribe();
        }

        void DrawDongs(DongInfo[] dongInfos)
        {
            foreach (var dongInfo in dongInfos)
            {
                GameObject dong = new GameObject(dongInfo.dong);
                dong.transform.parent = World.transform;
                foreach (var mesh in dongInfo.meshes)
                {
                    GameObject model = new GameObject("Mesh");
                    model.transform.parent = dong.transform;
                    var meshFilter = model.AddComponent<MeshFilter>();
                    meshFilter.mesh = mesh;
                    var meshRenderer = model.AddComponent<MeshRenderer>();
                    meshRenderer.material = resourceDataSource.GetRoomMaterial();

                    for (int j = 0; mesh.vertices.Length > j; j++)
                    {
                        var vert = mesh.vertices[j];
                        var vertObject = new GameObject($"Vert[{j}]");
                        vertObject.transform.parent = model.transform;
                        vertObject.transform.localPosition = vert;
                        vertObject.AddComponent<VertGizmoDrawer>();
                    }
                }
            }
        }

        void DrawRoomType(GameObject roomTypeObject, DongInfo.RoomType roomType)
        {
            for (int i = 0; roomType.rooms.Length > i; i++)
            {
                var room = roomType.rooms[i];
                var roomObeject = new GameObject($"Room[{i}]");
                roomObeject.transform.parent = roomTypeObject.transform;

                for (int j = 0; room.vertices.Length > j; j++)
                {
                    var vert = room.vertices[j];
                    var vertObject = new GameObject($"Vert[{j}]");
                    vertObject.transform.parent = roomObeject.transform;
                    vertObject.transform.localPosition = vert;
                    vertObject.AddComponent<VertGizmoDrawer>();
                }
            }
        }

        public void OnEvent(string eventType)
        {
            viewModel.OnEvent(eventType);
        }
    }
}