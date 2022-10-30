using HarmonyLib;
using Hazel;
using InnerNet;
using Rewired;
using Rewired.Utils.Platforms.Windows;
using System;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace RevolutionaryHostRoles.Patches
{
    public static class SendChat
    {
        /// <summary>
        /// 特定の人にチャットを送る、使い方 : (送るPlayer(PlayerControl)).SendChatPrivate((送られるPlayer(PlayerControl)), (送るテキスト(string)), 送ったメッセージが見えるプレイヤー(送り主にしか見えない場合は書かなくて良い))
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="Text"></param>
        /// <param name="see"></param>
        public static void SendChatPrivate(this PlayerControl player, PlayerControl target, string Text, PlayerControl see = null)//特定の人にチャットを送る
        {
            //player = 送るプレイヤー
            //target = 送られるプレイヤー
            PlayerControl SeePlayer = see;//見えるプレイヤー
            if (see == null) SeePlayer = player;//見えるプレイヤーいないなら自分だけ見える
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)RpcCalls.SendChat, SendOption.None, SeePlayer.GetClientId());
            //SendOptionをReliableにすると部屋追放されるから変更しないでね！！
            Writer.Write(Text);
            MessageExtensions.WriteNetObject(Writer, target);
            AmongUsClient.Instance.FinishRpcImmediately(Writer);
        }
        public static void RpcSetNames(this PlayerControl player, string Text, PlayerControl see = null)//特定の人の名前を一定時間変える
        {
            string PlayerName = player.Data.PlayerName;
            //player = 送るプレイヤー
            //target = 送られるプレイヤー
            PlayerControl SeePlayer = see;//見えるプレイヤー
            if (see == null) SeePlayer = player;//見えるプレイヤーいないなら自分だけ見える
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)RpcCalls.SetName, SendOption.None, SeePlayer.GetClientId());
            //SendOptionをReliableにすると部屋追放されるから変更しないでね！！
            Writer.Write(Text);
            AmongUsClient.Instance.FinishRpcImmediately(Writer);

            new LateTask(() => 
            {
                PlayerControl SeePlayer = see;//見えるプレイヤー
                if (see == null) SeePlayer = player;//見えるプレイヤーいないなら自分だけ見える
                MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)RpcCalls.SetName, SendOption.None, SeePlayer.GetClientId());
                //SendOptionをReliableにすると部屋追放されるから変更しないでね！！
                Writer.Write(PlayerName);
                AmongUsClient.Instance.FinishRpcImmediately(Writer);
            }, 0.05f, "ResetName");
        }


        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
        public class CoSpawnPlayerPatch
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                if (AmongUsClient.Instance.AmHost)
                {

                    __instance.myPlayer.SendChatPrivate(__instance.myPlayer, $"この部屋はMOD【Revolutionary Host Roles】\n通称【RHR】が実装されている部屋です。\n現在のホストは【{AmongUsClient.Instance.GetHost().PlayerName}】です。\n/cと送ることでコマンド一覧を確認出来ます。");
                }
            }
        }
        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        class AddChatPatch
        {
            public static bool Prefix([HarmonyArgument(0)] PlayerControl SourcePlayer, [HarmonyArgument(1)] string chatText)
            {
                var Commands = chatText.Split(" ");
                //switchの書き方わかんねぇ！！()

                if (Commands[0].Equals("/c", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/C", StringComparison.OrdinalIgnoreCase))//送られたメッセージ
                {
                    SourcePlayer.SendChatPrivate(SourcePlayer, $"コマンド一覧\n/c : コマンド一覧を記載します。\n/h : ホストの名前を記載します\n"
                        + "/ar : 全ての役職の選出する確率を記載します\n/ar c: クルー役職の選出する確率を記載します\n/ar i: インポスター役職の選出する確率を記載します\n/ar n 第三陣営役職の選出する確率を記載します: \n" +
                        "/re : 全ての役職の設定を記載します\n/re c: クルーの役職の設定を記載します\n/re i: インポスターの役職の設定を記載します\n/re n: 第三陣営の役職の設定を記載します\n"
                        + "/se : 現在のRHRの設定を記載します");
                    return false;
                }
                else if (Commands[0].Equals("/h", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/H", StringComparison.OrdinalIgnoreCase))
                {
                    SourcePlayer.SendChatPrivate(SourcePlayer, $"ホスト : {AmongUsClient.Instance.GetHost().PlayerName}");
                    return false;
                }
                else if (Commands[0].Equals("/ar", StringComparison.OrdinalIgnoreCase))
                {
                    if (Commands.Length == 1)
                    {
                        SourcePlayer.RpcSetNames($"全ての役職の確率一覧\n{GameOptionsDataPatch.buildRoleOptions()}");
                        SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                    }
                    switch (Commands[1])
                    {

                        case "c":
                             SourcePlayer.RpcSetNames($"クルー役職の確率一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, true)}");
                            SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                            break;
                        case "i":
                             SourcePlayer.RpcSetNames($"インポスター役職の確率一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.Impostor, true)}");
                            SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                            break;
                        case "n":
                             SourcePlayer.RpcSetNames($"第三陣営役職の確率一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.Neutral, true)}");
                            SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                            break;
                    }
                    return false;
                }
                else if (Commands[0].Equals("/re", StringComparison.OrdinalIgnoreCase))
                {
                    if (Commands.Length == 1)
                    {
                         SourcePlayer.RpcSetNames($"全ての設定一覧\n{GameOptionsDataPatch.buildRoleSettings()}");
                        SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                    }
                    switch (Commands[1])
                    {

                        case "c":
                             SourcePlayer.RpcSetNames($"クルーの設定一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false)}");
                            SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                            break;
                        case "i":
                             SourcePlayer.RpcSetNames($"インポスターの設定一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false)}");
                            SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                            break;
                        case "n":
                             SourcePlayer.RpcSetNames($"第三陣営の設定一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false)}");
                            SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                            break;
                    }
                    return false;
                }
                else if (Commands[0].Equals("/SettingsExplain", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/SE", StringComparison.OrdinalIgnoreCase))
                {
                    SourcePlayer.RpcSetNames($"RHRの設定一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.General, false)}");
                     SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                    return false;
                }
                else if (Commands[0].Equals("/rename", StringComparison.OrdinalIgnoreCase))
                {
                    if (SourcePlayer.AmOwner)
                    {
                        if (Commands.Length > 1)
                        {
                            SourcePlayer.RpcSetName(Commands[1]);
                        }
                    }
                    return false;
                }
                else if (Commands[0].ToUpper().Contains("/gr", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/n", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/l", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/w", StringComparison.OrdinalIgnoreCase))
                {
                    SourcePlayer.SendChatPrivate(SourcePlayer, "無効なコマンドです");
                    return false;
                }
                else
                {
                    return true;
                }


            }
        }
    }
}