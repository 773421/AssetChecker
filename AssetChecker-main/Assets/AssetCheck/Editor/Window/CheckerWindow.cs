using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Assets.Checker
{
    public class CheckerWindow : EditorWindow
    {
        [MenuItem("Tools/CheckerWindow #5")]
        public static void CreateWindow()
        {
            var window = EditorWindow.GetWindow<CheckerWindow>("资源检查工具");
            window.Init();
            window.Show();
        }
        public void Init()
        {
            this.maxSize = this.minSize = new Vector2(1000, 600);
            centerTxtStyle = new GUIStyle();
            centerTxtStyle.alignment = TextAnchor.MiddleCenter;
            centerTxtStyle.normal.textColor = Color.white;
            DiscoverClass();
        }
        private Dictionary<AssetType, List<CheckerData>> checkerDict = new Dictionary<AssetType, List<CheckerData>>();
        private List<ATData> atList = new List<ATData>();
        private List<LogInfo> logsList = new List<LogInfo>();
        private void DiscoverClass()
        {
            checkerDict.Clear();
            atList.Clear();
            var assembly = Assembly.GetExecutingAssembly();
            var checkerCls = assembly.GetExportedTypes()
                .Select(t => { return t; })
                .Where(t => t.IsSubclassOf(typeof(BaseChecker)));
            foreach (var checkerType in checkerCls)
            {
                var cAttr = checkerType.GetCustomAttribute<CheckerAttribute>();
                if (null != cAttr)
                {
                    var enumType = cAttr.assetType;
                    List<CheckerData> list = null;
                    if (!checkerDict.TryGetValue(enumType, out list))
                    {
                        list = new List<CheckerData>();
                        checkerDict[cAttr.assetType] = list;
                    }
                    list.Add(new CheckerData(checkerType, cAttr.tip, AssetTypeAttribute.GetPattern(enumType)));
                }
            }
            var enumVal = Enum.GetValues(typeof(AssetType));
            foreach (var o in enumVal)
            {
                var e = (AssetType)o;
                atList.Add(new ATData(e, AssetTypeAttribute.GetPattern(e)));
            }
        }
        private Rect leftTop = new Rect(0, 0, 250, 100);
        private Rect leftBottom = new Rect(0, 100, 250, 500);
        private Rect rightTop = new Rect(250, 0, 750, 100);
        private Rect rightButtom = new Rect(250, 100, 750, 500);
        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, leftTop.width, leftTop.height + leftBottom.height), "");
            BeginWindows();
            DrawAssetTypeWin(10000);
            DrawCheckersWindow(10000 + 1);
            DrawRightTopArea(11000);
            DrawRightBottom(12000);
            EndWindows();
        }
        private AssetType at;
        private Vector2 scrollLeftTop = Vector2.zero;
        /// <summary>
        /// 绘制资源类型选择窗口
        /// </summary>
        private void DrawAssetTypeWin(int id)
        {
            GUI.Window(id, leftTop, DoAssetTypeWin, "类型选择");
        }
        /// <summary>
        /// 检查项类型选择区域
        /// </summary>
        /// <param name="rect"></param>
        private void DoAssetTypeWin(int id)
        {
            scrollLeftTop = EditorGUILayout.BeginScrollView(scrollLeftTop);
            foreach (var at in atList)
            {
                at.IsCheck = EditorGUILayout.ToggleLeft(at.tip, at.IsCheck);
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }
        private Vector2 scrollLeftBottom = Vector2.zero;
        /// <summary>
        /// 绘制资源检查器选择窗口
        /// </summary>
        /// <param name="id"></param>
        private void DrawCheckersWindow(int startId)
        {
            int winIndex = startId;
            float nextPosY = leftBottom.y;
            scrollLeftBottom = EditorGUILayout.BeginScrollView(scrollLeftBottom);
            foreach (var ad in atList)
            {
                if (ad.IsCheck && checkerDict.ContainsKey(ad.assetType))
                {
                    var list = checkerDict[ad.assetType];
                    Rect winRect = new Rect(leftBottom.x, nextPosY, leftBottom.width, 30f + 18f * list.Count);
                    GUI.Window(winIndex, winRect, wid =>
                    {
                        foreach (var cd in list)
                        {
                            cd.IsCheck = EditorGUILayout.ToggleLeft(cd.tip, cd.IsCheck);
                        }
                    }, $"{ad.assetType}");
                    winIndex += 1;
                    nextPosY += winRect.height;
                }
            }
            EditorGUILayout.EndScrollView();
        }
        string folderPath = "";

        List<BaseChecker> checkerList = new List<BaseChecker>();
        private void DrawRightTopArea(int id)
        {
            GUI.Window(id, rightTop, s =>
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                if (string.IsNullOrEmpty(folderPath))
                {
                    folderPath = Application.dataPath;
                }
                GUILayout.TextField(folderPath);
                if (GUILayout.Button("Browse"))
                {
                    folderPath = EditorUtility.OpenFolderPanel("选择文件夹", folderPath, folderPath);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (GUILayout.Button("开始检查"))
                {
                    checkerList.Clear();
                    var iter = checkerDict.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        var list = iter.Current.Value;
                        foreach (var cd in list)
                        {
                            if (cd.IsCheck)
                            {
                                var inst = Activator.CreateInstance(cd.checkerType, cd.pattern) as BaseChecker;
                                checkerList.Add(inst);
                            }
                        }
                    }
                    if (checkerList.Count > 0)
                    {
                        string filePath = "";
                        try
                        {
                            logsList.Clear();
                            var path = folderPath.Substring(folderPath.IndexOf("Assets"));
                            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Select(t=>t).Where(t=> {
                                return !t.EndsWith(".meta");
                            }).ToArray();
                            int index = 0;
                            foreach(var file in files)
                            {
                                index += 1;
                                filePath = file;
                                foreach (var checker in checkerList)
                                {
                                    if (checker.IsMatch(filePath))
                                    {
                                        checker.BeginCheck(filePath, this.OnLogCallBack);
                                    }
                                }
                                EditorUtility.DisplayProgressBar("资源检查", filePath, index * 1f / files.Length);
                            }
                        }
                        catch (Exception e) {
                            Debug.LogError($"{filePath}\n{e.ToString()}");
                        }
                        finally
                        {
                            EditorUtility.ClearProgressBar();
                            maxPage = Mathf.CeilToInt(logsList.Count * 1 / 50);
                            pageIndex = 0;
                        }
                    }
                }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("<<"))
                {
                    pageIndex -= 1;
                    if (pageIndex < 0)
                    {
                        pageIndex = 0;
                    }
                }
                if (null != centerTxtStyle)
                {
                    GUILayout.Label($"{pageIndex}/{maxPage}", centerTxtStyle, GUILayout.Width(50));
                }
                if (GUILayout.Button(">>"))
                {
                    pageIndex += 1;
                    if (pageIndex > maxPage)
                    {
                        pageIndex = maxPage;
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }, "操作区");
        }
        private Vector2 scrollrightBottom = Vector2.zero;
        int pageIndex = 0;
        int maxPage = 0;
        int numOfPage = 50;
        GUIStyle centerTxtStyle;
        private void DrawRightBottom(int id)
        {
            GUI.Window(id, rightButtom, s =>
            {
                scrollrightBottom = EditorGUILayout.BeginScrollView(scrollrightBottom);
                EditorGUILayout.BeginVertical();
                for(int index= pageIndex * numOfPage; index < (pageIndex + 1) * numOfPage && index < logsList.Count; index++) {
                    var logInfo = logsList[index];
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("select", GUILayout.Width(60)))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(logInfo.assetPath));
                    }
                    GUILayout.Label($"{index}.{logInfo.assetPath}");
                    GUILayout.Label(logInfo.log, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }, "");
        }
        private void OnLogCallBack(string assetPath, string log) {
            logsList.Add(new LogInfo(assetPath, log));
        }
    }
}
