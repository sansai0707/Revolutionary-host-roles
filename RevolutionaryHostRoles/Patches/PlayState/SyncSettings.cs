using HarmonyLib;
using Hazel;
using RevolutionaryHostRoles.Patches;
using RevolutionaryHostRoles;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using RevolutionaryHostRoles.Roles;
using AmongUs.GameOptions;

namespace RevolutionaryHostRoles
{
    //SuperNewRolesより！！！！！ありがとうございます！！！！
    public static class SyncSetting
    {
        public static IGameOptions OptionData;
        public static void CustomSyncSettings(this PlayerControl player)
        {
            if (!AmongUsClient.Instance.AmHost) return;
            var role = player.GetRole();
            var optdata = OptionData.DeepCopy();
            switch (role)
            {
                case CustomRoleId.Tricker:
                    optdata.SetFloat(FloatOptionNames.KillCooldown, RoleDatas.Tricker.IsChangeKillCool ? CustomOptionHolder.TrickerKillCool.GetFloat() * 2 : CustomOptionHolder.TrickerKillCool.GetFloat());
                    break;
                case CustomRoleId.SecretlyKiller:
                    optdata.SetFloat(FloatOptionNames.KillCooldown,RoleDatas.SecretlyKiller.IsChangeKillCool ? CustomOptionHolder.SecretlyKillerKillCool.GetFloat() * 2 : CustomOptionHolder.SecretlyKillerKillCool.GetFloat());
                    break;
                case CustomRoleId.UnderDog:
                    optdata.SetFloat(FloatOptionNames.KillCooldown, player.IsLastImpostor() ? CustomOptionHolder.UnderDogChangeKillCool.GetFloat() : CustomOptionHolder.UnderDogKillCool.GetFloat());
                    break;
            }
            if (player.IsDead()) optdata.SetBool(BoolOptionNames.AnonymousVotes,false);
            optdata.SetBool(BoolOptionNames.ShapeshifterLeaveSkin,false);
            if (player.AmOwner) GameOptionsManager.Instance.CurrentGameOptions = optdata;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RpcCalls.SyncSettings, SendOption.None, player.GetClientId());
            writer.WriteBytesAndSize(GameOptionsManager.Instance.gameOptionsFactory.ToBytes(optdata));
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        public static float KillCoolSet(float cool) { return cool <= 0 ? 0.001f : cool; }
        public static void CustomSyncSettings()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (!p.Data.Disconnected)
                {
                    CustomSyncSettings(p);
                }
            }
        }
        public static IGameOptions DeepCopy(this IGameOptions opt)
        {
            var optByte = GameOptionsManager.Instance.gameOptionsFactory.ToBytes(opt);
            return GameOptionsManager.Instance.gameOptionsFactory.FromBytes(optByte);
        }
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGame))]
        public class StartGame
        {
            public static void Postfix()
            {
                OptionData = GameOptionsManager.Instance.CurrentGameOptions.DeepCopy();
            }
        }
    }
}