using UnityEditor;

namespace Assets.Checker
{
    [Checker(AssetType.Model, "动画导入开关检查")]
    public class ModelAnimation : BaseChecker
    {
        public ModelAnimation(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            if (importer is ModelImporter modelImporter)
            {
                if (modelImporter.clipAnimations.Length > 0 && !modelImporter.importAnimation)
                {
                    this.AddLog($"带有动画，需要开启动画导入开关");
                    return false;
                } else if (modelImporter.clipAnimations.Length <= 0 && modelImporter.importAnimation) {
                    this.AddLog($"不带动画，需要关闭动画导入开关");
                    return false;
                }
            }
            return true;
        }
    }
}
