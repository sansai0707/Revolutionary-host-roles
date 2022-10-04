using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace RevolutionaryHostRoles
{
   /* [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]*///なんかBeplnExのテンプレダウンロードした時についてきたやつ
    [BepInPlugin(Id, "RevolutionaryHostRoles", VersionString)]
    [BepInProcess("Among Us.exe")]
    public class RevolutionaryHostRolesPlugin : BasePlugin
    {
        public const string Id = "jp.sansai0707.RevolutionaryHostRoles";

        public const string VersionString = "0.0.0";
        public Harmony Harmony { get; } = new(Id);
        public static RevolutionaryHostRolesPlugin Instance;
        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public override void Load()
        {
            Logger = Log;
            Instance = this;
            Harmony.PatchAll();
        }
    }
}
/*
 */