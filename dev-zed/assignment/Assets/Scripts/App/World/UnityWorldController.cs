using System.Collections.Generic;
using UnityEngine;
using UniRx;
using World.Dto;
using World.Object;
using UnityBridge.Object;

namespace World
{
    public class UnityWorldController : IWorldController
    {
        GameObject World = new GameObject("World");
        private UnityWorldViewModel viewModel;
        private IResourceDataSource resourceDataSource;
        private List<IObject> objectList = new List<IObject>();

        public UnityWorldController(IRemoteDataSource remoteDataSource, IResourceDataSource resourceDataSource)
        {
            this.resourceDataSource = resourceDataSource;
            viewModel = new UnityWorldViewModel(remoteDataSource);

            viewModel.OnRemoved()
                .ObserveOnMainThread()
                .Do(name => Remove(name))
                .Subscribe();

            viewModel.GetDongs()
                .ObserveOnMainThread()
                .Do(dongInfos =>
                {
                    foreach (var dong in dongInfos)
                        objectList.Add(new DongObject(dong, resourceDataSource));
                })
                .Subscribe();
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

        private void Add(IObject obj)
        {
            if (obj == null)
                return;

            objectList.Add(obj);
        }

        private void Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            int index = objectList.FindIndex(x => x.Name == name);
            if (0 > index)
                return;

            var temp = objectList[index];
            objectList.RemoveAt(index);
            temp.OnDestroy();
        }

        public void OnEvent(string eventType, string message)
        {
            viewModel.OnEvent(eventType, message);
        }
    }
}