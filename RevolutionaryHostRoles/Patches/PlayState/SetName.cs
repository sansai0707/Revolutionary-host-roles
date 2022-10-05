using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    public static class SetNamePatch
    {

        public static void SetRoleName()
        {
            string AddName = "";
            string TaskText = "";
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (StartPatch.BeginStart.IsStart)
                {
                    string name = "<size=75%>" + p.RoleName() + TaskText + "</size>\n" + p.PlayerName() + AddName;
                    p.RpcSetNamePrivate(name);//自分以外見えない
                    /*foreach (PlayerControl Dead in DeadPlayers)
                    {
                        if (p.PlayerId != Dead.PlayerId) p.RpcSetNamePrivate(name, Dead);
                    }//自分が死んでなかったら死んでる人にしかロール見えないように
                */
                }
            }
        }
    }
}