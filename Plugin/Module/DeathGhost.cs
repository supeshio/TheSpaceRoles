using AmongUs.Data.Legacy;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using Cpp2IL.Core.Extensions;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public static class DeathGhost 
    {
        public static List<GameObject> Ghosts = [];
        public static void ShowGhosts(int[] players)
        {
            foreach (int player in players)
            {
                ShowGhost(DataBase.AllPlayerData[player].DeathPosition,player);
            }
        }
        public static void ShowGhost(Vector2 vector2,int id)
        {
            GameObject Ghost = new($"Ghost{id}");
            Ghost.SetActive(true);
            Ghost.layer = 14;//Ghostレイヤー
            //Ghost.transform.SetParent();
            Ghost.transform.SetPositionAndRotation(vector2+new Vector2(0f,0.3f), Quaternion.identity);
            var sp = Ghost.AddComponent<SpriteRenderer>();
            sp.sprite = Sprites.GetSpriteFromResources("object.ghost.png",400);
            sp.color = ColorEditHSV(Color.white, a: 0.6f);
            Ghosts.Add(Ghost);
            
        }
        public static void DisapperGhosts()
        {
            foreach (GameObject Ghost in Ghosts)
            {
                try
                {

                    Ghost.SetActive(false);
                    GameObject.Destroy(Ghost);
                }
                catch
                {

                }

            }
            Ghosts.Clear();
        }
    }

}