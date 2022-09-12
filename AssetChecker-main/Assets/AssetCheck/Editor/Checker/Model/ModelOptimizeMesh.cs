using UnityEditor;

namespace Assets.Checker
{
    [Checker(AssetType.Model, "模型OptimizeMesh开关检查")]
    public class ModelOptimizeMesh : BaseChecker
    {
        public ModelOptimizeMesh(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            if (importer is ModelImporter modelImporter)
            {
                if (!modelImporter.optimizeMesh)
                {
                    this.AddLog($"模型定点优化开关未开启(网格变形、粒子发射器效果等，该项需要关闭)");
                    return false;
                }
            }
            return true;
        }
    }
}
