
using System.Collections.Generic;

namespace RevolutionaryHostRoles.Roles
{
    public static class RoleDatas
    {
        public static void DataLoads()
        {
            Tricker.DataLoad();
        }
        public static class Tricker
        {
            public static List<PlayerControl> TrickerPlayer;
            public static Dictionary<bool, bool> IsTrick;
            public static bool IsTrickOK;
            public static bool IsTrickNO;
            public static Dictionary<bool, bool> IsTricked;
            public static void DataLoad()
            {
                TrickerPlayer = new();
                IsTrick = new Dictionary<bool, bool>();
                IsTricked = new Dictionary<bool, bool>();
                IsTrickOK = false;
                IsTrickNO = false;
            }
        }
    }
}