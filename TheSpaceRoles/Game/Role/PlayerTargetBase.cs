using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TSR.Game.Role
{
    public abstract class PlayerTargetBase
    {
        public abstract List<FPlayerControl> AllPlayers();
        public List<FPlayerControl> currentTarget { get; protected set; } = new List<FPlayerControl>();
        public static float Distance() => FPlayerControl.LocalPlayer.FRole.GetAbilityDistance().Distance;
        /// <summary>
        /// 元を呼んでくれればハイライトにcurrentTargetを送ってあげる
        /// 呼んでくれないなら自分で実装してね^^
        /// </summary>
        public virtual void SetTarget()
        {
            FGameManager.Manager.Local.Highlight.SetHighlight(currentTarget);
        }
        protected List<FPlayerControl> GetPlayersInAbilityRangeSorted(bool ignoreColliders)
        {
            List<FPlayerControl> outputList = [];
            float abilityDistance = Distance();
            Vector2 myPos = FPlayerControl.LocalPlayer.GetTruePosition();
            List<FPlayerControl> allPlayers = AllPlayers();
            for (int i = 0; i < allPlayers.Count; i++)
            {
                FPlayerControl fp = allPlayers[i];
                if (fp.IsValidTarget() && !fp.IsLocalPlayer)
                {
                    FPlayerControl @object = fp;
                    if (!@object.IsNull && @object.Collider.enabled)
                    {
                        Vector2 vector = @object.GetTruePosition() - myPos;
                        float magnitude = vector.magnitude;
                        if (magnitude <= abilityDistance &&
                            (ignoreColliders || !PhysicsHelpers.AnyNonTriggersBetween(myPos, vector.normalized, magnitude, Constants.ShipAndObjectsMask)))
                        {
                            outputList.Add(fp);
                        }
                    }
                }
            }
            outputList.Sort((a, b) =>
            {
                float da = (a.GetTruePosition() - myPos).sqrMagnitude;
                float db = (b.GetTruePosition() - myPos).sqrMagnitude;
                return da.CompareTo(db);
            });
            return outputList;
        }
    }
}
