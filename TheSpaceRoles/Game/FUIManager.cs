using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSR.Game.Role.Ability;

namespace TSR.Game
{
    public static class FUIManager
    {
        public static void Reset()
        {

        }
        public static void Update()
        {
            //Localなボタンだけ実行させるのだ
            FPlayerControl.LocalPlayer?.FRole?.CustomButtons?.Do(x => x.Update());
        }
    }
}
