
namespace RevolutionaryHostRoles.Patches
{
    public static class SetNamePatch
    {

        public static void SetRoleName()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                string AddName = "";
                string TaskText = "";
                if (AmongUsClient.Instance.GameState == AmongUsClient.GameStates.Started)
                {
                    string Name = "<size=75%>" + p.RoleName() + "</size>\n" + p.PlayerName();
                    p.RpcSetNamePrivate(Name);
                }
            }
        }
    }
}