using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Hazel;
using RevolutionaryHostRoles.Patches;
using RevolutionaryHostRoles.Roles;
using RevolutionaryHostRoles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking.Types;
using Rewired.Libraries.SharpDX.RawInput;
using System.Text.RegularExpressions;

namespace RevolutionaryHostRoles
{
    public static class UnderDog
    {
        public static bool IsLastImpostor()
        {
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                int DeadImpostor = 0;
                //pが死んでるandインポスターandアンダードッグじゃないならDeadImpostorを追加
                if (p.IsDead() && p.IsImpostor() && !p.IsRole(CustomRoleId.UnderDog)) DeadImpostor++;
                //設定のインポスター数 - アンダードッグ以外の死んでるインポスター数 = 1
                if (PlayerControl.GameOptions.NumImpostors - DeadImpostor == 1)
                    return true;
                else
                    return false;
            }
            return true;
        }
    }
}