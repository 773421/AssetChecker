using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CustomAsset.SceneRole
{
    public partial class SceneRoleAsset : ScriptableObject
    {
        public string scenePath;
        public List<SceneRole> mSceneRoleData;
        public void SetScenePath(string _scenePath)
        {
            this.scenePath = _scenePath;
        }
        public void Add(SceneRole _role)
        {
            if (null == this.mSceneRoleData)
            {
                this.mSceneRoleData = new List<SceneRole>();
            }
            this.mSceneRoleData.Add(_role);
        }
    }
}
