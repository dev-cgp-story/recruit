using System;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Entity;
using World.Dto;
using World.Object;

namespace World
{
    internal class UnityWorldViewModel
    {
        static int UvTableLength = 6;
        private List<IObject> objects;
        static Vector2[,] UvTable = new Vector2[,]
        {
            {
                new Vector2(0.5f, 0),
                new Vector2(0, 0),
                new Vector2(0, 0.5f),
                new Vector2(0, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0)
            },
            {
                new Vector2(0.75f, 0),
                new Vector2(0.5f, 0),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.75f, 0.5f),
                new Vector2(0.75f, 0)
            },
            {
                new Vector2(1.0f, 0),
                new Vector2(0.75f, 0),
                new Vector2(0.75f, 0.5f),
                new Vector2(0.75f, 0.5f),
                new Vector2(1.0f, 0.5f),
                new Vector2(1.0f, 0)
            },
        };

        private IRemoteDataSource dataSource;
        private Subject<string> removeSubject = new Subject<string>();

        public UnityWorldViewModel(IRemoteDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public IObservable<string> OnRemoved() => removeSubject;

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
            var dongs = new List<DongInfo>(dongEntities.Length);
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
                int height = Mathf.FloorToInt(roomTypes[0].rooms[0].vertices.Max(x => x.y) / 3);
                dongs.Add(new DongInfo(dong.meta.bdId, dong.meta.dong, height, roomTypes, meshes));
            }

            return dongs.ToArray();
        }

        private Mesh[] CreateMesh(DongInfo.RoomType[] roomTypes)
        {
            List<Mesh> meshes = new List<Mesh>();
            foreach (var roomType in roomTypes)
            {
                foreach (var room in roomType.rooms)
                {
                    int[] triangles = new int[room.vertices.Length];
                    for (int j = 0; room.vertices.Length > j; j++)
                    {
                        triangles[j] = j;
                    }

                    Mesh mesh = new Mesh();
                    mesh.vertices = room.vertices;
                    mesh.triangles = triangles;
                    mesh.RecalculateNormals();
                    mesh.uv = GetUvs(mesh.normals);
                    meshes.Add(mesh);
                }
            }

            return meshes.ToArray();
        }

        private Vector2[] GetUvs(Vector3[] normals)
        {
            Vector2[] uvs = new Vector2[normals.Length];
            for (int i = 0; normals.Length > i; i++)
            {
                Vector3 yAxisNormal = new Vector3(normals[i].x, 0, normals[i].z);
                float angle = Vector3.Dot(yAxisNormal, Vector3.forward);
                if (Mathf.Abs(Mathf.Cos(Vector3.Dot(yAxisNormal, Vector3.right))) == 1)
                {
                    uvs[i] = UvTable[2, i % UvTableLength];
                }
                else if (Mathf.Cos(180) <= angle && angle <= Mathf.Cos(220))
                {
                    uvs[i] = UvTable[0, i % UvTableLength];
                }
                else
                {
                    uvs[i] = UvTable[1, i % UvTableLength];
                }
            }

            return uvs;
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

        public void OnEvent(string eventType, string message)
        {
            if (eventType == "REMOVE")
            {
                removeSubject.OnNext(message);
            }
        }
    }
}