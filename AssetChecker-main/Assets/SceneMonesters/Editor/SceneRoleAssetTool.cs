using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace CustomAsset.SceneRole
{
    public class SceneRoleAssetTool
    {
        //[MenuItem("Tools/策划/导出当前场景角色数据")]
        public static string CreateSceneRoleDataAsset()
        {
            var scene = SceneManager.GetActiveScene();

            var objs = scene.GetRootGameObjects();

            var objsList = new List<GameObject>();
            foreach (var _GO in objs)
            {
                if (SceneRoleAsset.RoleRoot == _GO.name)
                {
                    var nCount = _GO.transform.childCount;
                    for (int index = 0; index < nCount; index++)
                    {
                        objsList.Add(_GO.transform.GetChild(index).gameObject);
                    }
                }
            }

            List<GameObject> prefabList = new List<GameObject>();
            foreach (var gbo in objsList)
            {
                var status = PrefabUtility.GetPrefabInstanceStatus(gbo);
                if (status == PrefabInstanceStatus.Connected)
                {
                    prefabList.Add(gbo);
                }
                if (status == PrefabInstanceStatus.MissingAsset)
                {
                    Debug.LogWarning($"Prefab [{gbo.name}] Is MissingAsset");
                }
            }
            return SceneRoleAsset.Create(scene.path, prefabList.ToArray());
        }
        //[MenuItem("Tools/策划/加载当前场景角色数据")]
        public static void LoadSceneRoleDataAsset()
        {
            if (Selection.objects.Length <= 0)
            {
                return;
            }
            var asset = Selection.objects[0];
            if (asset.GetType() != typeof(SceneRoleAsset))
            {
                return;
            }
            SceneRoleAsset.OpenAndLoadRoleData(asset as SceneRoleAsset);
        }
    }
}
