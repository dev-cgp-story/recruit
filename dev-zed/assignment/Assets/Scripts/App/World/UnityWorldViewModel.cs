using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Entity;
using World.Dto;

namespace World
{
    internal class UnityWorldViewModel
    {
        private IRemoteDataSource dataSource;

        public UnityWorldViewModel(IRemoteDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public IObservable<DongInfo[]> GetDongs()
        {
            return dataSource.GetDongs()
                .ObserveOnMainThread()
                .Select(response =>
                {
                    if (!response.success || response.code != 200)
                    {
                        // 필요한 경우 이곳에서 예외 처리
                        return null;
                    }

                    return SelectDogs(response.data);
                });
        }

        private DongInfo[] SelectDogs(Dong[] dongEntities)
        {
            var dongs = new DongInfo[dongEntities.Length];
            for (int i = 0; dongEntities.Length > i; i++)
            {
                var dong = dongEntities[i];
                var roomTypes = new DongInfo.RoomType[dong.roomTypes.Length];
                for (int j = 0; roomTypes.Length > j; j++)
                {
                    var roomType = dong.roomTypes[j];
                    var romms = SelectRooms(roomType);
                    roomTypes[j] = new DongInfo.RoomType(roomType.meta.roomTypeId, romms);
                }
                var meshes = CreateMesh(roomTypes);
                dongs[i] = new DongInfo(dong.meta.bdId, dong.meta.dong, dong.meta.height, roomTypes, meshes);
            }

            return dongs;
        }

        private static Mesh[] CreateMesh(DongInfo.RoomType[] roomTypes)
        {
            List<Mesh> meshes = new List<Mesh>();

            foreach (var roomType in roomTypes)
            {
                foreach (var room in roomType.rooms)
                {
                    int[] triangles = new int[room.vertices.Length];
                    for (int j = 0; room.vertices.Length > j; j++)
                        triangles[j] = j;

                    Mesh mesh = new Mesh();
                    mesh.vertices = room.vertices;
                    mesh.triangles = triangles;
                    meshes.Add(mesh);
                }
            }

            return meshes.ToArray();
        }

        private static DongInfo.Room[] SelectRooms(RoomType roomType)
        {
            var rooms = new DongInfo.Room[roomType.coordinates.Length];
            for (int i = 0; roomType.coordinates.Length > i; i++)
            {
                var coordinate = roomType.coordinates[i];
                List<Vector3> vertices = new List<Vector3>(coordinate.values.Length);
                for (int j = 0; coordinate.values.Length > j; j += 3)
                {
                    vertices.Add(new Vector3(coordinate.values[j], coordinate.values[j + 2], coordinate.values[j + 1]));
                }
                rooms[i] = new DongInfo.Room(vertices.ToArray());
            }
            return rooms;
        }

        public void OnEvent(string eventType)
        {
            if (eventType == "generate")
            {

            }
        }
    }
}