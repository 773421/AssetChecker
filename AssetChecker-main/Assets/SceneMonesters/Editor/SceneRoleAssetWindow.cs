using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace CustomAsset.SceneRole
{
    public class DataCfg
    {
        public string assetPath { get; private set; }
        public SceneRoleAsset srAsset { get; private set; }
        public List<SceneRole> mSceneRoleData
        {
            get
            {
                return srAsset.mSceneRoleData;
            }
        }
        public string name { get; private set; }
        public DataCfg(string assetPath, SceneRoleAsset srAsset)
        {
            this.assetPath = assetPath;
            this.srAsset = srAsset;
            this.name = this.srAsset.name;
        }
    }
    public class SceneRoleAssetWindow : EditorWindow
    {
        private List<string> sceneDataList;
        GUIStyle richTextStyle = new GUIStyle();
        [MenuItem("Tools/策划/场景角色数据窗口")]
        static void CreateWindow()
        {
            // Get existing open window or if none, make a new one:
            SceneRoleAssetWindow window = (SceneRoleAssetWindow)EditorWindow.GetWindow(typeof(SceneRoleAssetWindow));
            window.Show();
            window.Init();

        }
        void Init()
        {
            this.maxSize = this.minSize = new Vector2(1000, 600);
            richTextStyle.richText = true;
            richTextStyle.normal.textColor = Color.green;
            sceneDataList = new List<string>();
            var assetGuids = AssetDatabase.FindAssets("t:SceneRoleAsset", new string[] { SceneRoleAsset.DataFolder });
            foreach (var guid in assetGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                sceneDataList.Add(assetPath);
            }
        }
        Vector2 scrollPos = Vector2.zero;
        Vector2 scrollPos2 = Vector2.zero;
        private DataCfg dataCfg;
        private Rect leftTop = new Rect(0, 0, 250, 100);
        private Rect leftBottom = new Rect(0, 100, 250, 500);
        private Rect rightTop = new Rect(250, 0, 750, 100);
        private Rect rightButtom = new Rect(250, 100, 750, 500);
        private void OnGUI()
        {
            BeginWindows();
            GUI.Window(1000, leftTop, DoFuncWindow, "操作区");
            GUI.Window(2000, leftBottom, DoSceneDataWindow, "配置区");
            if (null != dataCfg)
            {
                GUI.Window(3000, rightTop, DoSceneFuncWindow, $"当前配置:{dataCfg.name}");
                GUI.Window(4000, rightButtom, DoSceneMonsterWindow, "场景怪物数据");
            }
            EndWindows();
        }
        private void DoFuncWindow(int id)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("刷新配置", GUILayout.Height(30), GUILayout.Width(115)))
            {
                this.Init();
            }
            if (GUILayout.Button($"导出[{SceneManager.GetActiveScene().name}]", GUILayout.Height(30), GUILayout.Width(115)))
            {
                var assetPath = SceneRoleAssetTool.CreateSceneRoleDataAsset();
                this.Init();
                LoadAssetData(assetPath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private void DoSceneDataWindow(int id)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));
            foreach (var assetPath in sceneDataList)
            {
                if (GUILayout.Button($"{assetPath}"))
                {
                    LoadAssetData(assetPath);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        private void DoSceneFuncWindow(int id)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("配置定位", GUILayout.Width(250), GUILayout.Height(30)))
            {
                EditorGUIUtility.PingObject(dataCfg.srAsset);
            }
            if (SceneManager.GetActiveScene().name == dataCfg.name)
            {
                if (GUILayout.Button("加载角色数据", GUILayout.Height(30)))
                {
                    SceneRoleAsset.Reload(dataCfg.srAsset);
                }
                if (GUILayout.Button("删除角色数据", GUILayout.Height(30)))
                {
                    SceneRoleAsset.RemoveSceneRole();
                }
            }
            else
            {
                if (GUILayout.Button("打开场景", GUILayout.Height(30)))
                {
                    SceneRoleAsset.OpenScene(dataCfg.srAsset);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        private void LoadAssetData(string assetPath)
        {
            dataCfg = new DataCfg(assetPath, AssetDatabase.LoadAssetAtPath<SceneRoleAsset>(assetPath));
        }
        private void DoSceneMonsterWindow(int id)
        {
            if (null != dataCfg.mSceneRoleData)
            {
                scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                foreach (var data in dataCfg.mSceneRoleData)
                {
                    GUILayout.Space(5f);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"名称: {data.name}", richTextStyle);
                    if (GUILayout.Button("Select"))
                    {
                        var asset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(data.guid));
                        if (null != asset)
                        {
                            EditorGUIUtility.PingObject(asset);
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"位置: {data.worldPos}");
                    GUILayout.Label($"角度: {data.angle}");
                    GUILayout.EndHorizontal();
                    GUILayout.TextField($"guid: {data.guid}");
                    GUILayout.TextField($"资源路径: {data.path}");
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }
}