using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;

namespace CustomAsset.SceneRole
{
    public partial class SceneRoleAsset : ScriptableObject
    {
        public const string DataFolder = "Assets/Editor/SceneData";
        public const string RoleRoot = "_MONSTERS_";
        public static string Create(string scenePath, GameObject[] objs)
        {

            string savePath = $"{SceneRoleAsset.DataFolder}/{Path.GetFileNameWithoutExtension(scenePath).ToString()}.asset";
            var srAsset = ScriptableObject.CreateInstance<SceneRoleAsset>();
            srAsset.SetScenePath(scenePath);
            foreach (var obj in objs)
            {
                var assetObj = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                if (null == assetObj)
                {
                    Debug.LogWarning($"{obj.name}'s source prefab is null.");
                    continue;
                }
                var assetPath = AssetDatabase.GetAssetPath(assetObj);
                if (string.IsNullOrEmpty(assetPath))
                {
                    Debug.LogWarning($"{assetObj.name} asset path is not find.");
                    continue;
                }

                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                Vector3 worldPos = obj.transform.position;
                Vector3 angle = obj.transform.eulerAngles;
                string name = obj.name;

                SceneRole sr = new SceneRole(name, assetPath, guid, TransformVector3(worldPos), TransformVector3(angle));
                srAsset.Add(sr);
            }
            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            }
            bool exists = File.Exists(savePath);
            if (EditorUtility.DisplayDialog("提示框", exists ? $"保存修改{Path.GetFileNameWithoutExtension(scenePath)}？" : $"创建配置{Path.GetFileNameWithoutExtension(scenePath)}?", "确定", "取消"))
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                AssetDatabase.CreateAsset(srAsset, savePath);
                EditorUtility.SetDirty(srAsset);
            }
            return savePath;
        }
        public static void OpenScene(SceneRoleAsset asset)
        {
            EditorSceneManager.OpenScene(asset.scenePath);
        }
        public static void OpenAndLoadRoleData(SceneRoleAsset asset)
        {
            OpenScene(asset);
            LoadSceneRole(asset);
        }
        public static void Reload(SceneRoleAsset asset)
        {
            LoadSceneRole(asset);
        }
        public static void RemoveSceneRole() {
            GameObject obj = GameObject.Find(SceneRoleAsset.RoleRoot);
            if (null != obj)
            {
                GameObject.DestroyImmediate(obj);
                obj = null;
            }
        }
        public static GameObject TryCreateRoot() {
            var obj = GameObject.Find(SceneRoleAsset.RoleRoot);
            if (null == obj)
            {
                obj = new GameObject(SceneRoleAsset.RoleRoot);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localEulerAngles = Vector3.zero;
            }
            return obj;
        }
        private static void LoadSceneRole(SceneRoleAsset asset)
        {
            SceneRoleAsset.RemoveSceneRole();
            GameObject root = SceneRoleAsset.TryCreateRoot();
            if (null == asset.mSceneRoleData || asset.mSceneRoleData.Count <= 0)
            {
                return;
            }
            try
            {
                int nCount = asset.mSceneRoleData.Count;
                for (int index = 0; index < nCount; index++)
                {
                    var data = asset.mSceneRoleData[index];
                    var assetPath = AssetDatabase.GUIDToAssetPath(data.guid);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    obj.transform.position = data.worldPos;
                    obj.transform.eulerAngles = data.angle;
                    obj.name = data.name;
                    obj.transform.parent = root.transform;
                    EditorUtility.DisplayProgressBar("场景", $"角色初始化中...", index * 1f / nCount);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        static Vector3 TransformVector3(Vector3 src)
        {
            Vector3 tarVector = Vector3.zero;
            tarVector.x = ((int)(src.x * 100)) * 1f / 100f;
            tarVector.y = ((int)(src.y * 100)) * 1f / 100f;
            tarVector.z = ((int)(src.z * 100)) * 1f / 100f;
            return tarVector;
        }
    }
}
