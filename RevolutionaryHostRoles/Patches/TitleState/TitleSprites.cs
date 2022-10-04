using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;
namespace RevolutionaryHostRoles.Patches
{
    /* [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    */
    [HarmonyPatch]
    public static class TitlePatch
    {


        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class VersionShowerPatch
        {
            public static string Authors = "<color=#ffff00>Authors</color>";
            public static string Developers = "<color=#0000ff>Developer</color>";
            public static string Sansai = "<color=#9acd32>Sansai</color>";
            public static string Haron = "<color=#00fa9a>haron</color>";
            public static string Oshurecat = "<color=#00ffff>oshurecat</color>";
            public static string Juki = "<color=#00bfff>JukiTuna</color>";
            public static string Lemons = "<color=#ffff00>Lemons</color>";
            public static string Syanpan = "<color=#0000ff>Syanpan</color>";
            public static string Serori = "<color=#00ff00>Serori</color>";
            static void Postfix(VersionShower __instance)
            {

                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo == null) return;
                /*=============製作者の名前=====================*/
                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                credentials.transform.position = new Vector3(0, 334f, 0);
                string credentialsText = "";
                credentialsText += "";
                credentials.SetText(credentialsText);

                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.9f;

                var RHRName = UnityEngine.Object.Instantiate(credentials);
                RHRName.transform.position = new Vector3(0, -0.2f, 0);
                RHRName.SetText(string.Format("<size=125%>" + Authors + " : " + Sansai + "\n" + Developers + " : " + Sansai + " " + Haron + " " + Oshurecat + " " + Juki + " " + Lemons + " " + Syanpan + " " + Serori + "</size>"));

                credentials.transform.SetParent(amongUsLogo.transform);
                RHRName.transform.SetParent(amongUsLogo.transform);
                /*=============製作者の名前=====================*/
            }
        }
        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class leftTitle
        {
            public static string RHRNAME = $"<size=130%><color=#32cd32>RevolutionaryHostRoles</color>v{RevolutionaryHostRolesPlugin.Version}</size>";
            static void Postfix(VersionShower __instance)
            {

                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                /*=============このMODの名前=====================*/
                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                credentials.transform.position = new Vector3(0, 334f, 0);
                string credentialsText = "";
                credentialsText += "";
                credentials.SetText(credentialsText);

                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.9f;

                var RHRName = UnityEngine.Object.Instantiate(credentials);
                RHRName.transform.position = new Vector3(3.3f, 2.8f, -5);
                RHRName.transform.localScale = new Vector3(1f, 1f, 10);
                RHRName.SetText(string.Format(RHRNAME));

                credentials.transform.SetParent(amongUsLogo.transform);
                RHRName.transform.SetParent(amongUsLogo.transform);

            }
        }


        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public class TitleLogoPatch
        {

            public static void Postfix()
            {

                var AmongUsLogo = GameObject.Find("bannerLogo_AmongUs");//ロゴ検出
                AmongUsLogo.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                AmongUsLogo.transform.position = new Vector3(0f, 2.5f, -0.1f);
                var TitleLogo = new GameObject("RHRLogo");
                TitleLogo.transform.position = new Vector3(0f, 1f, 0f);
                var renderer = TitleLogo.AddComponent<SpriteRenderer>();
                renderer.sprite = Helpers.LoadSpriteFromResources("RevolutionaryHostRoles.Resources.RHRTitleLogo.png", 210f);
                DestroyableSingleton<ModManager>.Instance.ShowModStamp();
            }

        }
    }

}