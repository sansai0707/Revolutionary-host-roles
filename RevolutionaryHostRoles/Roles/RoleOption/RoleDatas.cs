
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RevolutionaryHostRoles.Roles
{
    public static class RoleDatas
    {

        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
        public static void DataLoads()
        {
            Tricker.DataLoad();
            Bait.DataLoad();
            SecretlyKiller.DataLoad();
            UnderDog.DataLoad();
            Mafia.DataLoad();
        }
        public static class Tricker
        {
            public static Color color = new Color32(255, 255, 0, byte.MaxValue);
            public static List<PlayerControl> TrickerPlayer;
            public static Dictionary<bool, bool> IsTrick;
            public static bool IsTrickOK;
            public static bool IsChangeKillCool;
            public static bool IsTrickNO;
            public static Dictionary<bool, bool> IsTricked;
            public static void DataLoad()
            {
                TrickerPlayer = new();
                IsTrick = new Dictionary<bool, bool>();
                IsTricked = new Dictionary<bool, bool>();
                IsTrickOK = false;
                IsTrickNO = false;
                IsChangeKillCool = false;
            }
        }
        public static class Bait
        {
            public static Color color = new Color32(255, 255, 0, byte.MaxValue);
            public static List<PlayerControl> BaitPlayer;
            public static void DataLoad()
            {
                BaitPlayer = new();
            }
        }
        public static class SecretlyKiller
        {
            public static Color color = new Color32(255, 255, 0, byte.MaxValue);
            public static List<PlayerControl> SecretlyKillerPlayer;
            public static bool IsChangeKillCool;
            public static void DataLoad()
            {
                SecretlyKillerPlayer = new();
                IsChangeKillCool = false;
            }
        }
        public static class UnderDog
        {
            public static Color color = new Color32(255, 255, 0, byte.MaxValue);
            public static List<PlayerControl> UnderDogPlayer;
            public static void DataLoad()
            {
                UnderDogPlayer = new();
            }
        }
        public static class Mafia
        {
            public static Color color = new Color32(255, 255, 0, byte.MaxValue);
            public static List<PlayerControl> MafiaPlayer;
            public static void DataLoad()
            {
                MafiaPlayer = new();
            }
        }
    }
}