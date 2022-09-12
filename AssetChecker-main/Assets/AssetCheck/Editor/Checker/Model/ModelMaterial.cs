using UnityEditor;

namespace Assets.Checker
{
    [Checker(AssetType.Model, "模型材质导入开关检查")]
    public class ModelMaterial : BaseChecker
    {
        public ModelMaterial(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            if (importer is ModelImporter modelImporter) {
                if (modelImporter.importMaterials) {
                    this.AddLog($"模型材质导入开关开启");
                    return false;
                }
            }
            return true;
        }
    }
}
