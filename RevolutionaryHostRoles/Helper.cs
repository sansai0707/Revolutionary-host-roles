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
    public static class Helpers
    {
        private delegate bool DelegateLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        private static DelegateLoadImage _callLoadImage;
        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _callLoadImage ??= IL2CPP.ResolveICall<DelegateLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2cppArray = (Il2CppStructArray<byte>)data;

            return _callLoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }
        public static unsafe Texture2D loadTextureFromResources(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteTexture = new byte[stream.Length];
                var read = stream.Read(byteTexture, 0, (int)stream.Length);
                LoadImage(texture, byteTexture, false);
                return texture;
            }
            catch
            {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }
        public static Sprite LoadSpriteFromResources(string name, float size)
        {

            try
            {
                var texture = Helpers.loadTextureFromResources(name);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), size);
            }
            catch
            {

            }
            return null;
        }
        public static T SetRandom<T>(List<T> list)
        {
            var indexdate = UnityEngine.Random.Range(0, list.Count);
            return list[indexdate];
        }

        public static string cs(Color c, string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == id)
                    return player;
            return null;
        }
    }
    //ロールに関するやつ
    public static class PlayerHelper
    {
        public static bool IsAlive(this PlayerControl p)
        {
            return !p.Data.IsDead;
        }
        public static bool IsDead(this PlayerControl p)
        {
            return p.Data.IsDead;
        }
        public static bool IsCrew(this PlayerControl p)
        {
            return !p.Data.Role.IsImpostor;
        }
        public static bool IsImpostor(this PlayerControl p)
        {
            return p.Data.Role.IsImpostor;
        }
        public static bool IsRole(this PlayerControl p, CustomRoleId role)
        {
            CustomRoleId MyRole;
            MyRole = p.GetRole();
            return MyRole == role;
        }
        public static CustomRoleId GetRole(this PlayerControl p)
        {
            if (RoleDatas.Tricker.TrickerPlayer.IsCheckListPlayerControl(p)) return CustomRoleId.Tricker;
            else if (RoleDatas.Bait.BaitPlayer.IsCheckListPlayerControl(p)) return CustomRoleId.Bait;
            else if (RoleDatas.SecretlyKiller.SecretlyKillerPlayer.IsCheckListPlayerControl(p)) return CustomRoleId.SecretlyKiller;
            else if (RoleDatas.UnderDog.UnderDogPlayer.IsCheckListPlayerControl(p)) return CustomRoleId.UnderDog;
            else return CustomRoleId.NormalRoles;
        }
        public static bool IsCheckListPlayerControl(this List<PlayerControl> ListDate, PlayerControl CheckPlayer)
        {
            foreach (PlayerControl Player in ListDate)
            {
                if (Player.PlayerId == CheckPlayer.PlayerId)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static class NameHelper
    {
        public static Dictionary<int, string> PlayerNames;
        public static string PlayerName(this PlayerControl p)
        {
          
            var playerid = p.PlayerId;
            if (PlayerNames.ContainsKey(playerid))
            {
                return PlayerNames[playerid];
            }
            else
            {
                PlayerNames[playerid] = p.Data.PlayerName;
                return PlayerNames[playerid];
            }
        }
        public static string RoleName(this PlayerControl p)
        {
            switch (p.GetRole())
            {
                case CustomRoleId.Tricker:
                    return "<color=#ff0000>トリッカー</color>";
                case CustomRoleId.Bait:
                    return "<color=yellow>ベイト</color>";
                case CustomRoleId.SecretlyKiller:
                    return "<color=red>シークレットリーキラー</color>";
                case CustomRoleId.UnderDog:
                    return "<color=red>アンダードッグ</color>";
                default:
                    switch (p.Data.RoleType)
                    {
                        case RoleTypes.Crewmate:
                            return "<color=#00FFFF>クルーメイト</color>";
                        case RoleTypes.Engineer:
                            return "<color=#1e90ff>エンジニア</color>";
                        case RoleTypes.Scientist:
                            return "<color=#7fff00>科学者</color>";
                        case RoleTypes.Impostor:
                            return "<color=#FF0000>インポスター</color>";
                        case RoleTypes.Shapeshifter:
                            return "<color=#FF0000>シェイプシフター</color>";
                        default: return "<color=#ffff00>守護天使</color>";
                    }
            }
        }

        /*=====================ykundesuさんありがとうございます！！！=========================*/
        public static InnerNet.ClientData GetClient(this PlayerControl player)
        {
            var client = AmongUsClient.Instance.allClients.ToArray().Where(cd => cd.Character.PlayerId == player.PlayerId).FirstOrDefault();
            return client;
        }
        public static int GetClientId(this PlayerControl player)
        {
            var client = player.GetClient();
            return client == null ? -1 : client.Id;
        }
        public static void RpcSetNamePrivate(this PlayerControl TargetPlayer, string NewName, PlayerControl SeePlayer = null)
        {
            if (TargetPlayer == null || NewName == null || !AmongUsClient.Instance.AmHost) return;
            if (SeePlayer == null) SeePlayer = TargetPlayer;
            var clientId = SeePlayer.GetClientId();
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(TargetPlayer.NetId, (byte)RpcCalls.SetName, SendOption.Reliable, clientId);
            writer.Write(NewName);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        /*=====================ykundesuさんありがとうございます！！！=========================*/
        public static void RpcMurderPrivate(this PlayerControl TargetPlayer,PlayerControl SeePlayer = null)
        {
            if (TargetPlayer == null || !AmongUsClient.Instance.AmHost) return;
            List<PlayerControl> AllPlayers = new();
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.PlayerId == TargetPlayer.PlayerId)
                {

                }
                else
                {
                    AllPlayers.Add(p);
                }
            }
            foreach (PlayerControl AllPlayer in AllPlayers)
                if (SeePlayer == null) SeePlayer = AllPlayer;
            var clientId = SeePlayer.GetClientId();
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(TargetPlayer.NetId, (byte)RpcCalls.MurderPlayer, SendOption.Reliable, clientId);
            writer.Write(TargetPlayer);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
    public static class ModeHelper
    {
        
        public static CustomPlusModeId GetMode()
        {
            if (CustomOptionHolder.NotButton.GetBool())
            {
                return CustomPlusModeId.NotButton;
            }
            if (CustomOptionHolder.NotReport.GetBool())
            {
                return CustomPlusModeId.NotReport;
            }
            else return CustomPlusModeId.No;
        }
        public static bool IsMode(CustomPlusModeId Mode)
        {
            CustomPlusModeId ThisMode;
            ThisMode = GetMode();
            return ThisMode == Mode;
        }
        
    }
}