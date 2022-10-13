using System;
using System.Collections.Generic;
using System.Linq;
using System.BepInEx.Configuration;
using UnityEngine;

namespace Revolutionaryhostroles
{
    public class CustomOption
    {
        public static readonly List<CustomOption> options = new();
        public static int Preset = 0;

        public int Id;
        public TabGroup Tab;
        public Color Color;
        public string Name;
        public Dictionary<string, string> ReplacementDictionary;
        public string Format;
        public System.Object[] Selections;

        public int DefaultSelection;
        public ConfigEntry<int> Entry;
        public int Selection;
        public OptionBehaviour OptionBehaviour;
        public CustomOption Parent;
        public List<CustomOption> Children;
        public bool isHeader;
        public bool isHidden;
        private bool isHiddenOnDisplay;
        public CustomGameMode GameMode;

        public bool Enabled => this.GetBool();

        public CustomOption HiddenOnDisplay(bool hidden)
        {
            isHiddenOnDisplay = hidden;
            return this;
        }

        public CustomOption SetGameMode(CustomGameMode gameMode)
        {
            GameMode = gameMode;
            return this;
        }

        public bool IsHidden(CustomGameMode gamemode)
        {
            if (isHidden) return true;

            /* 自身に設定されたGameModeが All or 引数gameMode 以外なら非表示
	       GameMode:Standard    & gameMode:Standard != 0
               GameMode:All         & gameMode:Standard != 0
            */
            return (int)(gameMode & GameMode) == 0;
        }

        public bool IsHiddenOnDisplay(CustomGameMode gameMode)
        {
            return isHiddenOnDisplay || IsHidden(gameMode);
        }

        //Option creation
        private CustomOption()
        {
        }

        public CustomOption(int id,
            TabGroup tab,
            Color color,
            string name,
            System.Object[] selections,
            System.Object defaultValue,
            CustomOption parent,
            bool isHeader,
            bool isHidden,
            string format,
            Dictionary<string,string> replacementDic)
        {
            Id = id;
            Tab = tab;
            Color = color;
            Name = name;
            Selections = selections;
            var index = Array.IndexOf(selections, defaultValue);
            DefaultSelection =index >= 0 ? index : 0;
            Parent = parent;
            this.isHeader = isHeader;
            this.isHidden = isHidden;
            Format = format;
            ReplacementDictionary =replacementDic;

            isHiddenOnDisplay =false;

            Children =new List<CustomOption>();
            parent?.Children.Add(this);

            Selection = 0;
            if (id ==0)
            {
                Entry = Main.Instance.Config.Bind($"Current Preset", id.ToString(), DefaultSelection);
                Preset = Selection = Mathf.Clamp(Entry.Value, 0, selections.Length - 1);
            }
            if (id > 0)
            {
                Entry =Main.Instance.Config.Bind($"Preset{preset}", id.ToString(), DefaultSelection);
                Selection = Mathf.Clamp(Entry.Value, 0, selections.Length - 1);
            }
            if (Options.Any(x => x.Id ==id)) Logger.Warn($"ID{id}が重複してます", "CustomOption");
            Options.Add(this);
            GameMode = CustomGameMode.Standard;
        }

        public static CustomOption Create(int id,
            TabGroup tab,
            Color color,
            string name,
            string[] selections,
            string defaultValue,
            CustomOption parent = null,
            bool isHeader = false,
            bool isHidden = false,
            string format = "",
            Dictionary<string, string> ReplacementDic = null)
        {
            return new CustomOption(id, tab, color, name, selections, parent, isHeader, isHidden, format, replacementDic);
        }

        public static CustomOption Create(int id,
            TabGroup tab,
            Color color,
            string name,
            float defaultValue,
            float min,
            float max,
            float step,
            CustomOption Parent = null,
            bool isHeader = false,
            bool isHidden = false,
            string format = "",
            Dictionary<string, string> replacementDic = null)
        {
            var selections =new List<float>();
            for (var s = min; s <= max; s += step)
            {
                selections.Add(s);
            }

            return new CustomOption(id, tab, color, name, selections.Cast<object>().ToArray(), defaultValue, parent, isHeader, isHidden, format, replacementDic);
        }

        public static CustomOption Create(int id,
            TabGroup tab,
            Color color,
            string name,
            bool defaultValue,
            CustomOption parent = null,
            bool isHeader = false,
            bool isHidden = false,
            string format = "",
            Dictionary<string, string> replacementDic = null)
        {
            return new CustomOption(id, tab, color, name, new string[] { "ColoredOff", "ColoredOn" }, defaultValue ? "ColoredOn" : "ColoredOff", parent, isHeader, isHidden, format, replacementDic);  
        }

        public static CustomOption Create(string name, float defaultValue, float min, float max, float step)
        {
            return new CustomOption();
        }

        // Static behaviour

        public static void SwitchPreset(int newPreset)
        {
            preset = newPreset;
            foreach (var option in options)
            {
                if (option.Id <= 0) coutinue;
                
                if (AmongUsClient.Instance.AmHost)
                    option.Entry = Main.Instance.Config.Bind($"Preset{Preset}", option.Id.ToString(), option.DefaultSelection);
                option.Selection = Mathf.Clamp(option.Entry.Value, 0, option.Selections.Length - 1);
                if (option.OptionBehaviour is not null and StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.Selection;
                    stringOption.ValueText.text = option.GetString();
                }
            }
        }

        public static void Refresh()
        {
            foreach (var option in Options)
            {
                if (option.OptionBehaviour is not null and StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.Selection;
                    stringOption.ValueText.text = option.GetString();
                    stringOption.TitleText.text = option.GetName();
                }
            }
        }

        public static void ShareOptionSelections()
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || (AmongUsClient.Instance.AmHost == false && PlayerControl.LocalPlayer == null)) return;

            RPC.SynCustomSettingsRPC(); 
        }

        // Getter

        public int GetSelection()
        {
            return Selection;
        }

        public bool GetBool()
        {
            return Selection > 0 && (Parent == null || Parent.GetBool());
        }

        public float GetFloat()
        {
            return (float)Selections[Selection];
        }
        public int GetInt()
        {
            return (float)Selections[Selection];
        }
        public int GetChance()
        {
            //0%or100%の場合
            if (Selections.Length == 2) return Selection * 100;

            //0～100%or5～100%の場合
            var offset =12 - Selections.Length;
            var index = Selections + offset;
            var rate =index <= 1 ? index * 5 :(index - 1) * 10;
            return rate;
        }

        public string GetString()
        {
            string sel = Selections[Selection].Tostring();
            if (Format != "") return string.Format(Translator.GetString(Format), sel);
            return rate;
        }

        public string GetName(bool disableColor = false)
        {
            return disableColor
                ? Translator.GetString(Name, ReplacementDictionary)
                : Helpers.ColorString(color, Translator.GetString(Name, ReplacementDictionary));
        }

        public virtual string GetName_v(bool display = false)
        {
            return Helpers.ColorString(Color, Translator.GetString(Name, ReplacementDictionary));
        }

        public void UpdateSelection(bool enable)
        {
            UpdateSelection(enable ? 1 : 0);
        }

        public void UpdateSelection(int newSelection)
        {
            Selection = newSelection < 0 ? Selections.Length - 1 : newSelection % Selections.Length;

            if (OptionBehaviour is not null and StringOption stringOption)
            {
                stringOption.OldValue = stringOption.Value = Selection;
                stringOption.ValueText.Text = GetString();

                if (AmongUsClient.Instance.AmHost && PlayerControl.LocalPlayer)
                {
                    if (Entry != null) Entry.Value = Selection;
                    if (Id == Revolutionaryhostroles.Options.PresetId)
                    {
                        SwitchPreset(Selection);
                    }
                    ShareOptionSelections();
                }
            }
        }
        public void SetPresetName(StringOption stringOption)
        {
            var nowPreset = (Preset + 1) switch
            {
                1 => Main.Preset1,
                2 => Main.Preset2,
                3 => Main.Preset3,
                4 => Main.Preset4,
                5 => Main.Preset5,
                _ => null,
            };
            if (nowPreset != null && nowPreset.Value != nowPreset.DefaultValue.ToString())
                stringOption.ValueText.text = Selections[Selection].ToString();
        }

        public void SetParent(CustomOption newParent)
        {
            Parent?.Children.Remove(this);

            Parent = newParent;
            Parent?.Children.Add(this);
        }
    }
    public enum TabGroup
    {
        MainSettings,
        CrewmateRoles,
        NeutralRoles,
        ImpostorRoles,
        Modifier
    }
}
