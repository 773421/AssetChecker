namespace Assets.Checker
{
    /// <summary>
    /// 资源检查结果
    /// </summary>
    internal class LogInfo {
        public string assetPath { get; private set; }
        public string log { get; private set; }
        public LogInfo(string assetPath, string log)
        {
            this.assetPath = assetPath;
            this.log = log;
        }
    }
}
