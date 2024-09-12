using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    public static class PlayerControlButtonControls
    {
        /// <summary>
        /// ターゲットに対しての距離を測って一番近いplayerのidを出力
        /// </summary>
        /// <param name="targetdistance">ターゲットの許容距離</param>
        /// <param name="notIncludeTeamIds">含まないチーム</param>
        /// <param name="notIncludeIds">含まないPlayerID</param>
        /// <param name="target">誰基準か</param>
        /// <param name="canBeTargetInVentPlayer">ベント内のプレイヤーを含むか</param>
        /// <returns>一番近いplayerのid  もし-1が帰ってきたらターゲットいないです</returns>
        public static int SetTarget(float targetdistance, Color color, Teams[] notIncludeTeamIds = null, int[] notIncludeIds = null, int target = -1, bool canBeTargetInVentPlayer = false)
        {
            DataBase.AllPlayerControls().Do(x => x.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f));
            DataBase.AllPlayerControls().Do(x => x.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color));
            int id = -1;
            float distance = float.MaxValue;

            if (target == -1)
            {
                target = PlayerControl.LocalPlayer.PlayerId;
            }
            foreach (var x in PlayerControl.AllPlayerControls)
            {
                if (notIncludeIds != null && notIncludeIds.Length > 0)
                {
                    if (notIncludeIds.Contains(x.PlayerId))
                    {
                        continue;
                    }
                }
                if (notIncludeTeamIds != null && notIncludeTeamIds.Length > 0)
                {
                    foreach (var v in DataBase.AllPlayerRoles[x.PlayerId])
                    {

                        if (notIncludeTeamIds.Contains(v.team))
                        {
                            continue;
                        }
                    }

                }
                if (x.inVent)
                {
                    if (!canBeTargetInVentPlayer) { continue; }
                }
                if (x.Data.IsDead)
                {
                    continue;
                }
                if (target == x.PlayerId) continue;
                PlayerControl p = Helper.GetPlayerControlFromId(target);
                Vector2 truePosition = x.GetTruePosition();
                Vector2 vector = new Vector2(p.transform.position.x, p.transform.position.y) - truePosition;
                float magnitude = vector.magnitude;
                if (magnitude <= distance && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                {
                    id = x.PlayerId;
                    distance = magnitude;
                }

            }
            if (targetdistance >= distance)
            {
                Helper.GetPlayerControlFromId(id).cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
                return id;


            }
            return -1;



        }
    }

}
