using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Assets.Checker
{
    [Checker(AssetType.Prefab, "内嵌预制物体丢失检查")]
    public class PrefabMissingChecker : BaseChecker
    {
        public PrefabMissingChecker(string pattern) : base(pattern)
        {

        }
        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            try
            {
                if (OpenPrefab(assetPath))
                {

                    if (this.ExistChildPrefabMissing())
                    {
                        return false;
                    }
                }
            }
            catch (Exception e) {
                this.AddLog(e.ToString());
            }
            finally
            {
                this.CloseStage();
            }
            return true;
        }
        bool OpenPrefab(string assetPath)
        {
            try
            {
                System.Type type = typeof(PrefabStageUtility);
                MemberInfo info = type.GetMethod("OpenPrefab");
                object obj = type.InvokeMember("OpenPrefab",
                System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.NonPublic, null, null,
                new object[] { assetPath });
                return true;
            }
            catch (System.Exception e)
            {
                this.AddLog(e.Message);
            }
            return false;
        }
        void CloseStage()
        {
            try
            {
                PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                stage.ClearDirtiness();
                System.Type type = typeof(PrefabStage);
                MethodInfo mi = type.GetMethod("CloseStage");
                mi?.Invoke(stage, null);
            }
            catch (System.Exception e)
            {
                this.AddLog($"PrefabAssetLose.CloseStage {e.Message}");
            }
        }
        bool ExistChildPrefabMissing()
        {
            GameObject obj = Selection.activeObject as GameObject;
            if (null != obj)
            {
                Transform objTran = obj.transform;
                bool bExists = false;
                Queue<Transform> queue = new Queue<Transform>();
                queue.Enqueue(objTran);
                while (queue.Count > 0)
                {
                    objTran = queue.Dequeue();

                    if (PrefabUtility.GetPrefabAssetType(objTran.gameObject) == PrefabAssetType.MissingAsset)
                    {
                        this.AddLog($"{objTran.name} 丢失");
                        bExists = true;
                        break;
                    }
                    else
                    {
                        int nChild = objTran.childCount;
                        for (int index = 0; index < nChild; index++)
                        {
                            queue.Enqueue(objTran.GetChild(index));
                        }
                    }
                }
                queue.Clear();
                return bExists;
            }
            else
            {
                this.AddLog("IsExistChildPrefabMissing Obj Is null");
            }
            return false;
        }
    }
}
