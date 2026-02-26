using BepInEx.Configuration;
using System;
using UnityEngine;

namespace StarmakerSpeedupMod
{
    internal class ModConfig
    {
        public static ModConfig Instance = new ModConfig();
        private ModConfig() { }

        private ConfigEntry<bool>[] toggleConfigs;
        private ConfigEntry<KeyCode>[] keyConfigs;
        private ConfigEntry<int>[] multiplierConfigs;


        public static bool configLoaded = false;

        public KeyCode GetShowModsKey() => GetConfigEntry("ShowModsKey", keyConfigs).Value;
        public KeyCode GetToggleSpeedyTransitionsKey() => GetConfigEntry("ToggleSpeedyTransitionsKey", keyConfigs).Value;

        public void ToggleShowOpenModHint() => GetConfigEntry("ToggleShowOpenModHint", toggleConfigs).Value = !IsToggleShowOpenModHint();
        public bool IsToggleShowOpenModHint() => GetConfigEntry("ToggleShowOpenModHint", toggleConfigs).Value;

        public void ToggleSpeedyFadeIn() => GetConfigEntry("ToggleSpeedyFadeIn", toggleConfigs).Value = !HasSpeedyFadeIn();
        public bool HasSpeedyFadeIn() => GetConfigEntry("ToggleSpeedyFadeIn", toggleConfigs).Value;

        public void ToggleSpeedyTransitions() => GetConfigEntry("ToggleSpeedyTransitions", toggleConfigs).Value = !HasSpeedyTransitions();
        public bool HasSpeedyTransitions() => GetConfigEntry("ToggleSpeedyTransitions", toggleConfigs).Value;

        public void ToggleSkipMapAnimations() => GetConfigEntry("ToggleSkipMapAnimations", toggleConfigs).Value = !HasSkipMapAnimations();
        public bool HasSkipMapAnimations() => GetConfigEntry("ToggleSkipMapAnimations", toggleConfigs).Value;

        public void ToggleInstantText() => GetConfigEntry("ToggleInstantText", toggleConfigs).Value = !HasInstantText();
        public bool HasInstantText() => GetConfigEntry("ToggleInstantText", toggleConfigs).Value;


        public int GetConfigMinValue(ConfigEntry<int> configEntry) => ((AcceptableValueRange<int>)configEntry.Description.AcceptableValues).MinValue;
        public int GetConfigMaxValue(ConfigEntry<int> configEntry) => ((AcceptableValueRange<int>)configEntry.Description.AcceptableValues).MaxValue;

        private ConfigEntry<bool> GetConfigEntry(string key, ConfigEntry<bool>[] configs) => Array.Find(configs, c => c.Definition.Key.Equals(key));
        private ConfigEntry<KeyCode> GetConfigEntry(string key, ConfigEntry<KeyCode>[] configs) => Array.Find(configs, c => c.Definition.Key.Equals(key));
        private ConfigEntry<int> GetConfigEntry(string key, ConfigEntry<int>[] configs) => Array.Find(configs, c => c.Definition.Key.Equals(key));

        public void ConfigBindings(ConfigFile config)
        {
            configLoaded = true;
            toggleConfigs = [
                config.Bind("Toggles", "ToggleShowOpenModHint", true, "Toggle if 'press ShowModsKey to open' is displayed, when mod is closed"),
                config.Bind("Toggles", "ToggleSpeedyFadeIn", true, "Toggle if characters fade in fast"),
                config.Bind("Toggles", "ToggleSpeedyTransitions", true, "Toggle if transitions are being sped up"),
                config.Bind("Toggles", "ToggleInstantText", true, "Toggle if text is instant"),
                config.Bind("Toggles", "ToggleSkipMapAnimations", true, "Toggle if map animations are skipped"),
            ];

            keyConfigs = [
                config.Bind("Key", "ShowModsKey", KeyCode.X, "Shortcut key for showing or hiding the mod"),
                config.Bind("Key", "ToggleSpeedyTransitionsKey", KeyCode.F2, "Turns speedy transitions on or off"),
            ];

            multiplierConfigs = [
            ];
        }

    }
}
