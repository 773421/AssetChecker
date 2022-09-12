
using UnityEditor;
using UnityEngine;

namespace CustomAsset.SceneRole
{
    [System.Serializable]
    public class SceneRole
    {
        public string name;
        public string path;
        public string guid;
        public Vector3 worldPos;
        public Vector3 angle;

        public SceneRole(string name, string path, string guid, Vector3 worldPos, Vector3 angle)
        {
            this.name = name;
            this.path = path;
            this.guid = guid;
            this.worldPos = worldPos;
            this.angle = angle;
        }
    }
}
