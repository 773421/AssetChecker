using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Assets.Checker
{
    [Checker( AssetType.Texture, "贴图是否开启mipmap")]
    public class TextureMimpaOpen : BaseChecker
    {
        public TextureMimpaOpen(string pattern) : base(pattern)
        {
        }
        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            var textureImporter = importer as TextureImporter;
            if (textureImporter.mipmapEnabled)
            {
                this.AddLog("贴图开启了minmap");
                return false;
            }
            return true;
        }
    }
}
