using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Assets.Checker
{
    /// <summary>
    /// ios平台贴图格式是否需要带上alpha通道检查
    /// </summary>
    [Checker(AssetType.Texture, "iOS平台贴图Alpha通道检查")]
    public class TextureAlphaFIos : BaseChecker
    {
        public TextureAlphaFIos(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            string platform = "iPhone";
            var textureImporter = importer as TextureImporter;
            if (textureImporter.GetPlatformTextureSettings(platform, out var maxsize, out var textureImporterFormat))
            {
                if (textureImporter.DoesSourceTextureHaveAlpha() && (
                       textureImporterFormat == TextureImporterFormat.PVRTC_RGB2
                    || textureImporterFormat == TextureImporterFormat.PVRTC_RGB4
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGB_4x4
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGB_5x5
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGB_6x6
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGB_8x8
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGB_10x10
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGB_12x12
                    ))//不带alpha通道的
                {
                    this.AddLog($"{platform} 贴图带有Alpha通道,现在格式是:{textureImporterFormat}");
                    return false;
                }
                else if (!textureImporter.DoesSourceTextureHaveAlpha() && (
                       textureImporterFormat == TextureImporterFormat.PVRTC_RGBA2
                    || textureImporterFormat == TextureImporterFormat.PVRTC_RGBA4
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGBA_4x4
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGBA_5x5
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGBA_6x6
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGBA_8x8
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGBA_10x10
                    || textureImporterFormat == TextureImporterFormat.ASTC_RGBA_12x12
                    ))//带alpha通道的
                {
                    this.AddLog($"{platform} 贴图不带有Alpha通道,现在格式是:{textureImporterFormat}");
                    return false;
                }
            }
            return true;
        }
    }
}
