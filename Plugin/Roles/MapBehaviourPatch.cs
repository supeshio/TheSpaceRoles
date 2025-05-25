using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class MapBehaviourPatch
    {
        public static Color invisible = ColorFromColorcode("#00000000");
        [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Show))]
        private static class MapShow
        {
            static bool Prefix(MapBehaviour __instance, [HarmonyArgument(0)] MapOptions mapOptions)
            {
                var map = __instance;
                var f = Helper.GetCustomRole(PlayerControl.LocalPlayer);


                f.ShowMap(ref map);
                //if (mapOptions.Mode == MapOptions.Modes.Normal)
                //{
                //    return true;
                //}

                if (mapOptions.Mode == MapOptions.Modes.CountOverlay)
                {
                    map.ShowCountOverlay(false, true, true);
                    return false;
                }



                if ((bool)f.ImpostorMap)
                {
                    map.ShowSabotageMap();
                    //re = true;
                    if ((bool)f.AdminMap)
                    {

                        map.countOverlay.enabled = true;
                        map.countOverlay.gameObject.SetActive(true);
                        map.countOverlay.SetOptions((bool)f.ShowingMapAllowedToMove, (bool)f.ShowingAdminIncludeDeadBodies);
                        map.countOverlayAllowsMovement = true;
                        map.taskOverlay.Hide();
                        map.countOverlay.showLivePlayerPosition = true;
                        map.countOverlay.transform.SetLocalZ(-10f);
                        //map.infectedOverlay.allButtons.Do(x => x.transform.localPosition += new Vector3(0, -0.1f, 0));
                        map.infectedOverlay.allButtons.Do(x => x.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f));
                        map.ColorControl.baseColor = Palette.ImpostorRed;
                        map.countOverlay.BackgroundColor.baseColor = Palette.ImpostorRed;
                    }
                }
                else
                {
                    map.ShowNormalMap();
                    //re = true;
                    if ((bool)f.AdminMap)
                    {
                        map.countOverlay.BackgroundColor.baseColor = invisible;
                        map.countOverlay.enabled = true;
                        map.countOverlayAllowsMovement = (bool)f.ShowingMapAllowedToMove;
                        map.countOverlay.includeDeadBodies = true;
                        map.countOverlay.showLivePlayerPosition = true;
                    }
                }
                if (f.MapBackColor != null)
                {
                    map.ColorControl.baseColor = (Color)f.MapBackColor;
                    map.countOverlay.BackgroundColor.baseColor = (Color)f.MapBackColor;
                }
                return false;
                //if ((bool)f.AdminMap)
                //{
                //    map.ShowCountOverlay((bool)f.ShowingMapAllowedToMove, true, (bool)f.ShowingMapAllowedToMove);
                //    map.countOverlay.enabled = true;
                //    map.countOverlayAllowsMovement = (bool)f.ShowingMapAllowedToMove;
                //    map.countOverlay.includeDeadBodies = true;
                //    map.countOverlay.showLivePlayerPosition = true;

                //}

            }
        }
    }
}
