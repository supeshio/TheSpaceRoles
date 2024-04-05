using HarmonyLib;
using Il2CppSystem;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TheSpaceRoles.Translation;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace TheSpaceRoles
{
    public class RoleOptions
    {
        public Roles roles;
        public string GetRoleName => GetString($"role.{roles}.name");
        public GameObject @object;
        public int num = 0;
        public TextMeshPro Title_TMP;
        public RoleOptions(Roles roles,int i)
        {
            this.roles = roles;
            this.num = i;
            @object = new GameObject(roles.ToString());
            @object.active = true;
            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.role_option.png", 400);
            renderer.color = Helper.ColorFromColorcode("#222222");
            @object.transform.parent = HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("Roles");
            @object.active = true;
            @object.layer = HudManager.Instance.gameObject.layer;
            @object.transform.localPosition = new( -4.4f,2f -0.36f*num,0);

            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.parent = @object.transform;
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = GetRoleName;
            Title_TMP.color = GetLink.GetCustomRole(roles).Color;
            Title_TMP.fontSize = Title_TMP.fontSizeMax = 2f;
            Title_TMP.fontSizeMin = 1f;
            Title_TMP.alignment = TextAlignmentOptions.Left;
            Title_TMP.enableWordWrapping = false;
            Title_TMP.outlineWidth = 0.8f;
            Title_TMP.autoSizeTextContainer = false;
            Title_TMP.enableAutoSizing = true;
            Title_TMP.transform.localPosition = new Vector3(0f, 0, -1);
            Title_TMP.transform.localScale = Vector3.one;
            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
            Title_TMP.m_sharedMaterial = HudManager.Instance.GameSettings.m_sharedMaterial;
            Title_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Title_TMP.rectTransform.sizeDelta = new Vector2(1.2f, 0.5f);
            var box = @object.AddComponent<BoxCollider2D>();
            box.size = renderer.bounds.size;
            var Button = @object.AddComponent<PassiveButton>();
            Button.OnClick = new();
            Button.OnMouseOut = new UnityEvent();
            Button.OnMouseOver = new UnityEvent();
            Button._CachedZ_k__BackingField = 0.1f;
            Button.CachedZ = 0.1f;
            Button.Colliders = new[] { @object.GetComponent<BoxCollider2D>() };
            Button.OnClick.AddListener((System.Action)(() => {
                RoleOptionsHolder.roleOptions.Do(x=>x.@object.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#222222"));
                
                RoleOptionsHolder.selectedRoles = roles;
                renderer.color = Helper.ColorFromColorcode("#cccccc");









            }));

            Button.OnMouseOver.AddListener((System.Action)(() => {
                renderer.color = RoleOptionsHolder.selectedRoles == roles ? Helper.ColorFromColorcode("#cccccc"): Helper.ColorFromColorcode("#555555");
            }));
            Button.OnMouseOut.AddListener((System.Action)(() => {
                renderer.color = RoleOptionsHolder.selectedRoles == roles ? Helper.ColorFromColorcode("#cccccc"): Helper.ColorFromColorcode("#222222");
            }));
            Button.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
            Button.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;

        }

    }
}
