using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Diagnostics;
using RevolutionaryHostRoles.Roles;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.PlayerLoop;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    public static class PlayerControls
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]//キルされるときのやつ
        class MurderPlayer
        {
            public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
            {

            }
        }
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]//タスク1個終わらした時
        class CompleteTaskPatch
        {
            public static void Postfix(PlayerControl __instance)
            {

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
                            target.Exiled();//強制Rpc
                            RoleDatas.Tricker.IsTrickNO = true;
                            RoleDatas.Tricker.IsTrickOK = false;
                            RoleDatas.Tricker.IsTricked[__instance] = true;
                            RoleDatas.Tricker.IsTrick[__instance] = false;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                }
                __instance.RpcMurderPrivate();
                return false;
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

                            /*if (!RoleDatas.Tricker.IsTricked.ContainsKey(__instance))
                            {
                                RoleDatas.Tricker.IsTricked[__instance] = false;
                                RoleDatas.Tricker.IsTrick[__instance] = true;
                            }
                            */
                            if (!RoleDatas.Tricker.IsTrickNO)
                            {
                                RoleDatas.Tricker.IsTrickOK = true;
                            }
                            else
                            {

                            }
                            break;
                    }
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
        class ReportDeadBodyPatch
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
        }
    }
}