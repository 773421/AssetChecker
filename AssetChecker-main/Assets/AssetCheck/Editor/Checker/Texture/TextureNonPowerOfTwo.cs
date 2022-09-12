using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Assets.Checker
{
    [Checker(AssetType.Texture, "图片大小不是2的次幂")]
    public class TextureNonPowerOfTwo : BaseChecker
    {
        public TextureNonPowerOfTwo(string pattern) : base(pattern)
        {
        }

        public override bool OnCheck(string assetPath, AssetImporter importer)
        {
            var textureImporter = importer as TextureImporter;
            if (textureImporter.textureShape != TextureImporterShape.TextureCube)
            {
                Texture target = AssetDatabase.LoadAssetAtPath<Object>(assetPath) as Texture;
                var type = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");

                MethodInfo methodInfo = type.GetMethod("IsNonPowerOfTwo", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo != null)
                {
                    string isNon = methodInfo.Invoke(null, new object[] { target }).ToString();
                    if (isNon.Contains("True"))
                    {
                        this.AddLog("图片大小不是2的次幂");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
