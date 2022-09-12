using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Assets.Checker
{
    [Checker( AssetType.Texture, "贴图是否开启Sreamingmipmap")]
    public class TextureStreamingMimpaOpen : BaseChecker
    {
        public TextureStreamingMimpaOpen(string pattern) : base(pattern)
        {
        }
        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            var textureImporter = importer as TextureImporter;
            if (textureImporter.streamingMipmaps)
            {
                this.AddLog("贴图开启了streamingMipmaps");
                return false;
            }
            return true;
        }
    }
}
