using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles
{
    public enum CustomRoleId
    {
        //バニラ役職
        NormalRoles,
        //インポスター役職
        Tricker,
        SecretlyKiller,
        UnderDog,
        Mafia,
        //クルー役職
        Bait
        //Python作成

        //なんでも役職
    }
}