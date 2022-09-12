using UnityEditor;

namespace Assets.Checker {
    public class ModelAnimCompression : BaseChecker
    {
        public ModelAnimCompression(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            if (importer is ModelImporter modelImporter)
            {
                if (modelImporter.animationCompression != ModelImporterAnimationCompression.Optimal)
                {
                    this.AddLog($"动画压缩未选择Optimal，选择该选项在运行时可减少内存占用");
                    return false;
                }
            }
            return true;
        }
    }
}
