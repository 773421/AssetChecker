using System;

namespace Assets.Checker
{
    public class CheckerAttribute: Attribute {
        public AssetType assetType;
        public string tip;
        public CheckerAttribute(AssetType checkerType)
        {
            this.assetType = checkerType;
        }

        public CheckerAttribute(AssetType checkerType, string tip)
        {
            this.assetType = checkerType;
            this.tip = tip;
        }
    }
}
