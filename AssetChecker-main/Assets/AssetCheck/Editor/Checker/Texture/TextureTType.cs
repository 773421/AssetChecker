using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Assets.Checker
{
    /// <summary>
    /// 贴图格式检查
    /// </summary>
    [Checker(AssetType.Texture, "贴图类型设置检查")]
    public class TextureTType : BaseChecker
    {
        public TextureTType(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            var textureImporter = importer as TextureImporter;
            if (textureImporter.textureType == TextureImporterType.GUI || textureImporter.textureType == TextureImporterType.Cookie || textureImporter.textureType == TextureImporterType.Cursor)
            {
                this.AddLog($"类型不符合, 现在类型是: {textureImporter.textureType}");
                return false;
            }
          
            return true;
        }
    }
}
