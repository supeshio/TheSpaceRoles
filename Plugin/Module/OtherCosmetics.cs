namespace TheSpaceRoles
{
    //public static class OtherCosmetics
    //{
    //    public static TextMeshPro textTemplate;
    //    public static void Reset()
    //    {  textTemplate = GameObject.Find("HatsGroup").transform.FindChild("Text").GetComponent<TextMeshPro>();
    //    }
    //    private static float CreatePackage(List<Tuple<SkinData, HatExtension>> skins, string packageName, float yStart,
    //        SkinsTab skinTab)
    //    {
    //        var isDefaultPackage = CustomHatManager.InnerslothPackageName == packageName;
    //        if (!isDefaultPackage)
    //        {
    //            skins = skins.OrderBy(x => x.Item1.name).ToList();
    //        }

    //        var offset = yStart;
    //        if (textTemplate != null)
    //        {
    //            var title = UnityEngine.Object.Instantiate(textTemplate, skinTab.scroller.Inner);
    //            title.transform.localPosition = new Vector3(2.25f, yStart, -1f);
    //            title.transform.localScale = Vector3.one * 1.5f;
    //            title.fontSize *= 0.5f;
    //            title.enableAutoSizing = false;
    //            skinTab.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => { title.SetText(packageName); })));
    //            offset -= 0.8f * skinTab.YOffset;
    //        }

    //        for (var i = 0; i < skins.Count; i++)
    //        {
    //            var (skin, ext) = skins[i];
    //            var xPos = skinTab.XRange.Lerp(i % skinTab.NumPerRow / (skinTab.NumPerRow - 1f));
    //            var yPos = offset - i / skinTab.NumPerRow * (isDefaultPackage ? 1f : 1.5f) * skinTab.YOffset;
    //            var colorChip = UnityEngine.Object.Instantiate(skinTab.ColorTabPrefab, skinTab.scroller.Inner);
    //            if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
    //            {
    //                colorChip.Button.OnMouseOver.AddListener((Action)(() => skinTab.SelectSkin(skin)));
    //                colorChip.Button.OnMouseOut.AddListener((Action)(() => skinTab.SelectSkin(DestroyableSingleton<HatManager>.Instance.GetSkinById(DataManager.Player.Customization.Skin))));
    //                colorChip.Button.OnClick.AddListener((Action)skinTab.ClickEquip);
    //            }
    //            else
    //            {
    //                colorChip.Button.OnClick.AddListener((Action)(() => skinTab.SelectSkin(skin)));
    //            }
    //            colorChip.Button.ClickMask = skinTab.scroller.Hitbox;
    //            colorChip.Inner.SetMaskType(PlayerMaterial.MaskType.ScrollingUI);
    //            skinTab.UpdateMaterials(colorChip.Inner.FrontLayer, skin);
    //            var background = colorChip.transform.FindChild("Background");
    //            var foreground = colorChip.transform.FindChild("ForeGround");

    //            if (ext != null)
    //            {
    //                if (background != null)
    //                {
    //                    background.localPosition = Vector3.down * 0.243f;
    //                    background.localScale = new Vector3(background.localScale.x, 0.8f, background.localScale.y);
    //                }
    //                if (foreground != null)
    //                {
    //                    foreground.localPosition = Vector3.down * 0.243f;
    //                }

    //                if (textTemplate != null)
    //                {
    //                    var description = UnityEngine.Object.Instantiate(textTemplate, colorChip.transform);
    //                    description.transform.localPosition = new Vector3(0f, -0.65f, -1f);
    //                    description.alignment = TextAlignmentOptions.Center;
    //                    description.transform.localScale = Vector3.one * 0.65f;
    //                    skinTab.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => { description.SetText($"{skin.name}\nby {ext.Author}"); })));
    //                }
    //            }

    //            colorChip.transform.localPosition = new Vector3(xPos, yPos, -1f);
    //            colorChip.set(skin.BundleId, skinTab.HasLocalPlayer() ? PlayerControl.LocalPlayer.Data.DefaultOutfit.ColorId : DataManager.Player.Customization.Color);
    //            colorChip.Inner.transform.localPosition = skin.ChipOffset;
    //            colorChip.Tag = skin;
    //            colorChip.SelectionHighlight.gameObject.SetActive(false);
    //            skinTab.ColorChips.Add(colorChip);
    //        }

    //        return offset - (skins.Count - 1) / skinTab.NumPerRow * (isDefaultPackage ? 1f : 1.5f) * skinTab.YOffset -
    //               1.75f;
    //    }
    //}
}
