using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace World.Dto
{
    public struct DongInfo
    {
        public struct RoomType
        {
            public int id;
            public Room[] rooms;

            public RoomType(int id, Room[] rooms)
            {
                this.id = id;
                this.rooms = rooms;
            }
        }

        public struct Room
        {
            public Vector3[] vertices;

            public Room(Vector3[] vertices)
            {
                this.vertices = vertices;
            }
        }

        public int bdId;
        public string dong;
        public int height;
        public RoomType[] roomTypes;
        public Mesh[] meshes;

        public DongInfo(int bdId, string dong, int height, RoomType[] roomTypes, Mesh[] meshes)
        {
            this.bdId = bdId;
            this.dong = dong;
            this.height = height;
            this.roomTypes = roomTypes;
            this.meshes = meshes;
        }
    }

}