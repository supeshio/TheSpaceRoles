using System;
using System.Collections.Generic;
using UnityEngine;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class Trickster : CustomRole
    {
        public Trickster()
        {
            Team = Teams.Impostor;
            Role = Roles.Trickster;
            Color = Palette.ImpostorRed;
        }
        CustomButton TricksterSetBoxButton;
        CustomButton TricksterLightdownButton;
        public override void HudManagerStart(HudManager __instance)
        {
            Boxes = [];
            TricksterSetBoxButton = new CustomButton(
                __instance, "TricksterSetBoxButton"
                , this,
                ButtonPos.Custom/*ボタンタイプ*/,
                KeyCode.F/*ボタンキー*/,
                12.5f/*クールダウン*/,
                () => 0/*-1以外なら成功判定*/,
                Sprites.GetSpriteFromResources("ui.button.vampire_bite.png", 100f),//トリックスター､箱設置
                () =>
                {
                    var vec2 = CreateBox();
                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 0);
                    writer.Write(vec2.x);
                    writer.Write(vec2.y);
                    writer.EndRpc();

                },
                () =>
                {
                    TricksterSetBoxButton.Timer = TricksterSetBoxButton.maxTimer;
                    if (TricksterSetBoxButton.RemainCount <= 0)
                    {
                        TricksterSetBoxButton.actionButton.Hide();
                        TricksterLightdownButton.actionButton.Show();

                    }
                },
                "Box",
                false/*HasEffect*/, remainUses: 3/*残り使用回数*/);

            TricksterLightdownButton = new CustomButton(
                __instance, "TricksterLightdownButton"
                , this,
                ButtonPos.Custom/*ボタンタイプ*/,
                KeyCode.F/*ボタンキー*/,
                30/*クールダウン*/,
                () => 0/*-1以外なら成功判定*/,
                Sprites.GetSpriteFromResources("ui.button.evilhacker_hack.png", 100f),//トリックスター､停電
                () =>
                {
                    LightDown();
                },
                () =>
                {
                    TricksterLightdownButton.Timer = TricksterLightdownButton.maxTimer;
                    if (TricksterSetBoxButton.RemainCount <= 0)
                    {
                        TricksterLightdownButton.actionButton.Show();

                    }
                },
                "Lightout",
                true/*HasEffect*/, EffectDuration: 7.5f, remainUses: -1/*残り使用回数*/);
            //Effect
            TricksterLightdownButton.actionButton.Hide();

        }
        public static CustomOption LightOutSize;
        public static CustomOption LightOutTime;
        public override void OptionCreate()
        {
            LightOutSize = CustomOption.Create(CustomOption.OptionType.Impostor, "role.trickster.lightoutsize", new CustomFloatRange(0.1f, 1.0f, 0.1f));
            LightOutTime = CustomOption.Create(CustomOption.OptionType.Impostor, "role.trickster.lightouttime", new CustomFloatRange(0.5f, 20f, 0.5f));

            Options = [LightOutSize, LightOutTime];
        }

        public Dictionary<GameObject, bool> Boxes = new();
        public static GameObject BoxParent;
        public Vector2 CreateBox(bool active = false)
        {
            if (BoxParent?.layer == null)
            {
                BoxParent = new("Boxes");
                BoxParent.transform.SetParent(ShipStatus.Instance.transform, false);
            }

            GameObject box = new("Box" + (Boxes.Count + 1));
            box.transform.SetParent(BoxParent.transform);
            var sp = box.AddComponent<SpriteRenderer>();
            sp.sprite = Sprites.GetSpriteFromResources("object.box.png", 256f);
            sp.color = Helper.ColorEditHSV(Color.white, a: active ? 1f : 0.5f);
            Boxes.Add(box, active);
            var vec2 = PlayerControl.GetTruePosition();
            box.transform.position = new(vec2.x, vec2.y, 1);
            box.layer = 11;
            return vec2;
        }
        public static Dictionary<GameObject, bool> OtherBoxes = new();
        public static Vector2 CreateOtherBox(Vector2 vec2, bool active = false)
        {
            if (BoxParent?.layer == null)
            {
                BoxParent = new("Boxes");
                BoxParent.transform.SetParent(ShipStatus.Instance.transform, false);
            }

            GameObject box = new("Box" + (OtherBoxes.Count + 1));
            box.transform.SetParent(BoxParent.transform);
            var sp = box.AddComponent<SpriteRenderer>();
            sp.sprite = Sprites.GetSpriteFromResources("object.box.png", 256f);
            box.SetActive(active);
            OtherBoxes.Add(box, active);
            box.transform.position = new(vec2.x, vec2.y, 1);
            box.layer = 11;
            return vec2;
        }
        public void AppearBox()
        {
            bool active = false;
            foreach (var item in Boxes)
            {
                if (!item.Value)
                {
                    Boxes[item.Key] = true;

                    var pc = GetPlayerById(CustomButton.SetTarget());
                    active = true;
                    item.Key.GetComponent<SpriteRenderer>().color = Helper.ColorEditHSV(Color.white, a: 1f);
                }
            }
            if (active)
            {
                var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 1);
                writer.EndRpc();
            }

        }
        public static void AppearOtherBox()
        {
            foreach (var item in OtherBoxes)
            {
                if (!item.Value)
                {
                    OtherBoxes[item.Key] = true;

                    var pc = GetPlayerById(CustomButton.SetTarget());
                    item.Key.GetComponent<SpriteRenderer>().color = Helper.ColorEditHSV(Color.white, a: 1f);
                }
            }

        }
        public void LightDown()
        {
            var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 2);

            writer.EndRpc();
            Trickster.TrickLightDown(PlayerControl);

        }
        public float Light = 1.0f;
        public static void TrickLightDown(PlayerControl pc)
        {
            Trickster f = (Trickster)pc.GetCustomRole();
            LateTask.AddRepeatedTask(0, 0.3f, (sec) =>
            {
                if (sec == 0)
                {
                    return;
                }
                f.Light = 0.3f / sec;
                if (sec >= 0.3f)
                {
                    f.Light = 0f;
                }

            });

            LateTask.AddRepeatedTask(LightOutTime.GetFloatValue(), 0.3f, (sec) =>
            {
                if (sec == 0)
                {
                    return;
                }
                f.Light = 1 - (0.3f / sec);
                if (sec >= 0.3f)
                {
                    f.Light = 1f;
                }

            });
        }


        public override Tuple<ChangeLightReason, float> GetOtherLight(PlayerControl pc, ShipStatus shipStatus, float num)
        {
            if (Light == 1f)
            {
                return Tuple.Create(ChangeLightReason.None, -1f);
            }
            return Tuple.Create(ChangeLightReason.TricksterLightdown, num - LightOutSize.GetFloatValue() * Light + LightOutSize.GetFloatValue());
        }

    }
}
