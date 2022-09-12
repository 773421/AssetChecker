using UnityEditor;

namespace Assets.Checker
{
    [Checker(AssetType.Model,"模型Read/Write开关检查")]
    public class ModelReadWrite : BaseChecker
    {
        public ModelReadWrite(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            if(importer is ModelImporter modelImporter){
                if (modelImporter.isReadable) {
                    this.AddLog($"模型Read/Write开关开启");
                    return false;
                }
            }
            return true;
        }
    }
}
