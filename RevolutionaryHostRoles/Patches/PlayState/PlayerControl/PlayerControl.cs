using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Diagnostics;
using RevolutionaryHostRoles.Roles;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;
using UnityEngine.PlayerLoop;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]//キルされるときのやつ
    class MurderPlayer
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (target.IsRole(CustomRoleId.Bait))
                new LateTask(() => { __instance.CmdReportDeadBody(target.Data); }, CustomOptionHolder.BaitReportTime.GetFloat(), "BaitKill");
        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]//タスク1個終わらした時
    class CompleteTaskPatch
    {
        public static void Postfix(PlayerControl __instance)
        {

        }
    }
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.RepairSystem))]
    class RepairSystemPatch
    {
        public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] SystemTypes systemType, [HarmonyArgument(1)] PlayerControl player, [HarmonyArgument(2)] byte amount)
        {
            switch (player.GetRole())
            {
                case CustomRoleId.Temple:
                //サボタージュ不可役職
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoEnterVent))]
    class CoEnterVentPatch
    {
        public static bool Prefix(PlayerPhysics __instance, [HarmonyArgument(0)] int ventid)
        {

            switch (__instance.myPlayer.GetRole())
            {
                case CustomRoleId.Temple:
                //ベント不可役職
                    new LateTask(() =>
                    {
                        foreach (PlayerControl p in CachedPlayer.AllPlayers)
                        {
                            MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(__instance.NetId, (byte)RpcCalls.BootFromVent, SendOption.Reliable, p.GetClientId());

                            writer2.Write(ventid);
                            AmongUsClient.Instance.FinishRpcImmediately(writer2);
                            __instance.myPlayer.inVent = false;
                        }
                    }, 0.1f, "Anti Vent");


                    return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckMurder))]//キルしようとした時
    class CheckMurderPatch
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {

            switch (__instance.GetRole())
            {
                case CustomRoleId.Tricker:
                    if (RoleDatas.Tricker.IsTrickOK)
                    {

                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(target.NetId, (byte)RpcCalls.Exiled, SendOption.None);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        target.Exiled();//Rpc
                        RoleDatas.Tricker.IsTrickNO = true;
                        RoleDatas.Tricker.IsTrickOK = false;
                        RoleDatas.Tricker.IsTricked[__instance] = true;
                        RoleDatas.Tricker.IsTrick[__instance] = false;
                        __instance.RpcProtectPlayer(__instance, 0);
                        RoleDatas.Tricker.IsChangeKillCool = true;
                        new LateTask(() => { SyncSetting.CustomSyncSettings(__instance); }, 0.01f, "KillCoolReset");
                        new LateTask(() => { RoleDatas.Tricker.IsChangeKillCool = false; __instance.RpcMurderPlayer(__instance); }, 0.05f, "KillCoolReset");
                        new LateTask(() => { SyncSetting.CustomSyncSettings(__instance); }, 0.1f, "KillCoolReset");
                        return false;
                    }
                    else
                    {
                        SyncSetting.CustomSyncSettings(__instance);
                        return true;
                    }
                case CustomRoleId.SecretlyKiller:
                    target.RpcMurderPlayer(target);
                    target.RpcProtectPlayer(target, 0);
                    RoleDatas.SecretlyKiller.IsChangeKillCool = true;

                    new LateTask(() => { SyncSetting.CustomSyncSettings(__instance); }, 0.01f, "KillCoolReset");
                    new LateTask(() => { RoleDatas.SecretlyKiller.IsChangeKillCool = false; __instance.RpcMurderPlayer(target); }, 0.05f, "KillCoolReset");
                    new LateTask(() => { SyncSetting.CustomSyncSettings(__instance); }, 0.1f, "KillCoolReset");
                    return false;
                case CustomRoleId.UnderDog:
                    SyncSetting.CustomSyncSettings();//かくにん
                    new LateTask(() => {__instance.RpcMurderPlayer(target); }, 0.05f, "Kill");
                    return false;
                case CustomRoleId.Mafia:
                    return __instance.IsLastImpostor();
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Shapeshift))]//シェイプした時
    class ShapesihftPatch
    {

        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (!(__instance.PlayerId == target.PlayerId))
            {
                switch (__instance.GetRole())
                {
                    case CustomRoleId.Tricker:
                        if (!RoleDatas.Tricker.IsTrickNO)
                        {
                            RoleDatas.Tricker.IsTrickOK = true;
                        }
                        break;
                }
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
    public class ReportDeadBodyPatch
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo target)
        {
            if (target == null && ModeHelper.IsMode(CustomPlusModeId.NotButton))
            {
                return false;
            }
            else if (target != null && ModeHelper.IsMode(CustomPlusModeId.NotReport))
            {
                return false;
            }
            return true;
        }
        public static List<PlayerControl> DiePlayers;

    }
}