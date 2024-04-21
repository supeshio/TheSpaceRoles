using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using IntPtr = System.IntPtr;
using Object = UnityEngine.Object;

namespace TheSpaceRoles;

public class Sprites
{

    public static Dictionary<string, Sprite> CachedSprites = new();

    public static Dictionary<string, Texture2D> CachedTexture = [];


    public static Sprite GetSprite(string path, float pixelsPerUnit = 115f)
    {

        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;

            Texture2D val = LoadTextureFromResources(path);
            sprite = Sprite.Create(val, new Rect(0f, 0f, val.width, val.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch (System.Exception ex)
        {
            Logger.Warning("Error can't load sprite path:" + path + "\nError : " + ex, "", "GetSprite");
        }
        return null;
    }

    public static Sprite GetSpriteFromResources(string path, float pixelsPerUnit = 115f)
    {
        var a = CachedSprites.TryGetValue(path + pixelsPerUnit, out _);
        return GetSprite("TheSpaceRoles.Resources." + path, pixelsPerUnit);
    }

    public static Texture2D LoadTextureFromResources(string path)
    {
        try
        {
            if (CachedTexture.TryGetValue(path, out var texture2D)) return texture2D;
            Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(path);
            var byteTexture = new byte[stream.Length];
            var read = stream.Read(byteTexture, 0, (int)stream.Length);
            LoadImage(texture, byteTexture, false);
            texture.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedTexture[path] = texture;
        }
        catch
        {
            Logger.Warning("Error loading texture from resources: " + path, "", "LoadTextureFromResources");
        }
        return null;
    }
    public static Texture2D LoadTextureFromDisk(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            }
        }
        catch
        {
            Logger.Error("Error loading texture from disk: " + path);
        }
        return null;
    }
    internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    internal static d_LoadImage iCall_LoadImage;

    private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
    {
        iCall_LoadImage ??= IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
        var il2cppArray = (Il2CppStructArray<byte>)data;
        return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
    }

    public static GameObject Render(string noResourcePath, string name, float pixelsPerUnit = 115f, int layer = 5, int sortingLayerID = 0, int sortingOrder = 5, GameObject parent = null, bool active = true)
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
        //IL_000e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0017: Unknown result type (might be due to invalid IL or missing references)
        //IL_0020: Expected O, but got Unknown
        //IL_0054: Unknown result type (might be due to invalid IL or missing references)
        GameObject val = new GameObject
        {
            layer = layer,
            active = active,
            name = name
        };
        if (parent != null)
        {
            val.transform.SetParent(parent.transform);
        }
        val.transform.localPosition = new Vector3(0f, 0f, -38f);
        SpriteRenderer val2 = val.AddComponent<SpriteRenderer>();
        val2.sprite = GetSprite("TheSpaceRoles.Resources." + noResourcePath, pixelsPerUnit);
        val2.sortingLayerID = sortingLayerID;
        val2.sortingOrder = sortingOrder;
        return val;
    }

    public static GameObject GobjRender(GameObject @object, string noResourcePath, string name, float scale, float size = 1f, int layer = 5, int sortingLayerID = 50, int sortingOrder = 5, GameObject parent = null, bool active = true)
    {
        //IL_0093: Unknown result type (might be due to invalid IL or missing references)
        Sprite sprite = GetSprite("TheSpaceRoles.Resources." + noResourcePath, scale);
        Logger.Info("TheSpaceRoles.Resources." + noResourcePath + " : path" + (sprite.texture).ToString(), "", "GobjRender");
        GameObject val = Object.Instantiate<GameObject>(@object);
        (val).name = name;
        if (val == null)
        {
            return null;
        }
        if (parent != null)
        {
            val.transform.SetParent(parent.transform);
        }
        val.transform.position = new Vector3(0f, 0f, 0f);
        SpriteRenderer component = val.GetComponent<SpriteRenderer>();
        component.sprite = sprite;
        component.sortingLayerID = sortingLayerID;
        component.sortingOrder = sortingOrder;
        return val;
    }
}
