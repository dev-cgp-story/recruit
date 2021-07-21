using UnityEngine;
using World.Object;
using World.Dto;

namespace UnityBridge.Object
{
    public class DongObject : IUnityObjectWrapper, IObject
    {
        private DongInfo dong;
        private GameObject gameObject;
        private Transform cachedTransform;

        public string Name => dong.dong;

        public DongObject(DongInfo dong, IResourceDataSource resources)
        {
            this.dong = dong;
            this.gameObject = new GameObject(dong.dong);
            this.cachedTransform = gameObject.transform;

            foreach (var mesh in dong.meshes)
            {
                GameObject model = new GameObject("Mesh");
                model.transform.parent = cachedTransform;
                var meshFilter = model.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                var meshRenderer = model.AddComponent<MeshRenderer>();
                meshRenderer.material = resources.GetDongMaterial(dong.height);

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



        public void Attach(IObject parent)
        {
            if (parent is IUnityObjectWrapper wrapper)
            {
                wrapper.Transform();
            }
        }

        public void OnDestroy()
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(gameObject);
#else
            GameObject.Destroy(gameObject);
#endif
        }

        public Transform Transform()
        {
            return cachedTransform;
        }
    }
}