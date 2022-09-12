using UnityEditor;

namespace Assets.Checker
{
    [Checker(AssetType.Texture, "贴图/Read/Write开关检查")]
    public class TextureReadWrite : BaseChecker
    {
        public TextureReadWrite(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            var textureImporter = importer as TextureImporter;
            if (textureImporter.isReadable) {
                this.AddLog($"{assetPath} 读写选项开启");
                return false;
            }
            return true;
        }
    }
}
