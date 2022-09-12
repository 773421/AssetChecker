namespace Assets.Checker
{
    /// <summary>
    /// 资源类型数据
    /// </summary>
    internal class ATData
    {
        public AssetType assetType;
        public string pattern;
        public bool IsCheck = false;
        public string tip = "";
        public ATData(AssetType at, string pattern, bool isCheck = false)
        {
            this.assetType = at;
            this.pattern = pattern;
            IsCheck = isCheck;
            tip = $"{at}({pattern})";
        }
    }
}
