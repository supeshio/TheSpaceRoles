using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using TSR.Patch;

namespace TSR.Patch;

/// <summary>
/// 日本語設定時、全TMP_FontAssetのfallbackFontAssetTableで日本語フォントを中国語フォントより先に並べる。
/// </summary>
[SmartPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
public static class FontFallbackPatch
{
    public static void Postfix()
    {
        if (TSR.TranslationId.Value != "ja_jp") return;

        var fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        var count = 0;
        foreach (var font in fonts)
        {
            if (font == null) continue;
            var table = font.fallbackFontAssetTable;
            if (table == null || table.Count == 0) continue;

            var list = new TMP_FontAsset[table.Count];
            for (var i = 0; i < table.Count; i++)
                list[i] = table[i];
            var reordered = ReorderJpBeforeCn(list);
            if (!reordered.SequenceEqual(list))
            {
                table.Clear();
                foreach (var f in reordered)
                    table.Add(f);
                count++;
            }
        }
        if (count > 0)
            Logger.Info($"FontFallback: {count} font(s) reordered for Japanese priority");
    }

    private static TMP_FontAsset[] ReorderJpBeforeCn(TMP_FontAsset[] list)
    {
        static bool IsJapanese(TMP_FontAsset? f)
        {
            if (f == null) return false;
            var n = (f.name ?? "").ToLowerInvariant();
            return n.Contains("jp") || n.Contains("japanese") || n.EndsWith("jp");
        }

        static bool IsChinese(TMP_FontAsset? f)
        {
            if (f == null) return false;
            var n = (f.name ?? "").ToLowerInvariant();
            return n.Contains("sc") || n.Contains("tc") || n.Contains("cn") || n.Contains("chinese")
                || n.Contains("cjk") && !n.Contains("jp");
        }

        var jp = new List<TMP_FontAsset>();
        var cn = new List<TMP_FontAsset>();
        var other = new List<TMP_FontAsset>();

        foreach (var f in list)
        {
            if (IsJapanese(f)) jp.Add(f);
            else if (IsChinese(f)) cn.Add(f);
            else other.Add(f);
        }

        var result = new List<TMP_FontAsset>();
        result.AddRange(jp);
        result.AddRange(cn);
        result.AddRange(other);
        return result.ToArray();
    }
}
