using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Checker
{
    [Checker( AssetType.Scene,"检查场景中存在Prefab丢失")]
    public class ScenePrefabAssetLose : BaseChecker
    {
        public ScenePrefabAssetLose(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            if (this.CheckPrefabLose(assetPath)) {
                return false;
            }
            return true;
        }

        private bool CheckPrefabLose(string _path)
        {
            bool IsExistsLose = false;
            EditorSceneManager.OpenScene(_path);
            GameObject[] gbos = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            foreach (var gbo in gbos)
            {
                if (PrefabUtility.GetPrefabInstanceStatus(gbo) == PrefabInstanceStatus.MissingAsset)
                {
                    this.AddLog($"missing {gbo.name}");
                    IsExistsLose = true;
                }
            }
            return IsExistsLose;
        }
    }
}
