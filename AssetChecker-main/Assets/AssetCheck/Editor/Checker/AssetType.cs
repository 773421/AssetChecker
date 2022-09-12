namespace Assets.Checker
{
    /// <summary>
    /// 指定要检查的资源类型以及该类型对应的格式
    /// </summary>
    public enum AssetType
    {
        [AssetType(".(jpg|png|tga|jpeg|dds)$")]
        Texture = 1<<1,
        [AssetType(".(prefab)$")]
        Prefab = 1<<2,
        [AssetType(".(unity)$")]
        Scene = 1<<3,
        [AssetType(".(fbx)$")]
        Model = 1 << 4,
    }
}
