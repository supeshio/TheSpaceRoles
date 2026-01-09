using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;

namespace TSR.Assets
{
    public static class AssetLoader
    {
        public static readonly Dictionary<string, Sprite> Sprites = new();
        public static readonly Dictionary<string, AudioClip> AudioClips = new();
        public static readonly Dictionary<string, Material> Materials = new();
        public static readonly Dictionary<string, Shader> Shaders = new();
        public static void LoadAssetsFromEmbeddedBundle()
        {
            var assembly = Assembly.GetExecutingAssembly();

            const string resourceName = "TSR.Resources.tsr";
            // ↑ リソース名は "プロジェクトのルート名前空間 + フォルダ + ファイル名" 形式です。
            // 例: TheSpaceRoles/Resource/tsr → "TSR.Resources.tsr"

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                Debug.LogError($"[TSR] 埋め込みリソースが見つかりません: {resourceName}");
                return;
            }

            using MemoryStream ms = new();
            stream.CopyTo(ms);
            byte[] data = ms.ToArray();

            AssetBundle bundle = AssetBundle.LoadFromMemory(data);
            if (bundle == null)
            {
                Debug.LogError("[TSR] AssetBundle の読み込みに失敗しました");
                return;
            }

            foreach (var asset in bundle.LoadAllAssets())
            {
                if (asset.TryCast<SpriteAtlas>() != null)
                {
                    SpriteAtlas atlas = asset.Cast<SpriteAtlas>();
                    Debug.Log($"[TSR] SpriteAtlas : {atlas.name}");
                    RegisterSpritesFromAtlas(atlas);
                }
                else
                if (asset.TryCast<Sprite>() != null)
                {
                    Sprite sprite = asset.Cast<Sprite>();
                    Sprites[sprite.name] = (sprite);
                    sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                    Debug.Log($"[TSR] Sprite : {sprite.name}");
                }
                else if (asset.TryCast<AudioClip>())
                {
                    AudioClip clip = asset.Cast<AudioClip>();
                    AudioClips[clip.name] = clip;
                    Debug.Log($"[TSR] AudioClip : {clip.name}");
                }
                else if (asset.TryCast<Material>())
                {
                    Material mat = asset.Cast<Material>();
                    Materials[mat.name] = mat;
                    Debug.Log($"[TSR] Material : {mat.name}");
                }
                else if (asset.TryCast<Shader>())
                {
                    Shader shader = asset.Cast<Shader>();
                    Shaders[shader.name] = shader;
                    Debug.Log($"[TSR] Shader : {shader.name}");
                }
                else
                {
                    Debug.Log($"[TSR] Ignored : {asset.name} ({asset.GetType()})");
                }
            }
        }

        public static void RegisterSpritesFromAtlas(SpriteAtlas atlas)
        {
            if (atlas == null)
            {
                Debug.LogError("[TSR] SpriteAtlas is null");
                return;
            }

            Debug.Log($"[TSR] SpriteAtlas loaded: {atlas.name}");

            var sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites); // spriteCount は Unity 2021 以降推奨
            foreach (var sprite in sprites)
            {
                if (sprite == null) continue;

                Sprites[sprite.name] = sprite;
                sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                Debug.Log($"[TSR] Sprite regi: {sprite.name}");
            }

            Debug.Log($"[TSR] SpriteAtlas Register From `{atlas.name}`  {sprites.Length} sprite");
        }
    }
}