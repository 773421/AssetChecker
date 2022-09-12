using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Checker
{
    /// <summary>
    /// 资源检测器基类
    /// </summary>
    public abstract class BaseChecker
    {
        private StringBuilder logStrBuilder = null;
        private bool result = true;
        private AssetImporter importer = null;
        public string pattern { get; private set; }
        protected BaseChecker(string pattern)
        {
            this.pattern = pattern;
            logStrBuilder = new StringBuilder();
        }
        protected void AddLog(string _log) {
            logStrBuilder.AppendLine(_log);
        }
     
        public bool GetResult() {
            return result;
        }

        public bool IsMatch(string assetPath) {
            return Regex.IsMatch(assetPath, pattern);
        }
        public void BeginCheck(string assetPath, Action<string,string> OnLogCallBack = null) {
            importer = AssetImporter.GetAtPath(assetPath);
            result = this.OnCheck(assetPath, importer);
            if (!result)
            {
                var needInport = this.OnProcessFormat(assetPath, importer);
                if (needInport)
                {
                    importer.SaveAndReimport();
                }
            }
            if (logStrBuilder.Length > 0 && null != OnLogCallBack)
            {
                OnLogCallBack(assetPath, logStrBuilder.ToString());
            }
            logStrBuilder.Clear();
        }
        /// <summary>
        /// 在这边做资源检查
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public virtual bool OnCheck(string assetPath, AssetImporter importer) {
            return true;
        }
        /// <summary>
        /// 资源检查失败时执行，可在这将问题修正，有修改时需要return true
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="importer"></param>
        public virtual bool OnProcessFormat(string assetPath, AssetImporter importer) {
            return false;
        }
    }
}
