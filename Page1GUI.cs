using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StarmakerSpeedupMod
{
    // as the name implies, handles everything related to first page of the cheatmod
    // global and player related cheats like toggle infinite stats, cheat money, spawn topics, etc. are found here
    internal class Page1GUI : MonoBehaviour, ModsGUI.IPageGUI
    {
        public static Page1GUI Instance;
        private Page1GUI() { }

        public void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        public string GetPageDescription()
        {
            return "Starmaker Speedup Mod options";
        }

        public void RenderPage()
        {
            ModsGUI.NewLine(ShowSpeedyTransitionsToggle);
            ModsGUI.NewLine(ShowSpeedyFadeInToggle);
            ModsGUI.NewLine(ShowInstantTextToggle);
            ModsGUI.NewLine(ShowModHintToggle);
        }

        private void ShowSpeedyTransitionsToggle()
        {
            GUILayout.Label("Speedier transitions (may break minigames) (" + ModConfig.Instance.GetToggleSpeedyTransitionsKey() + ")", ModGUIStyles.LabelStyle);
            if (ModsGUI.CMButton(ModConfig.Instance.HasSpeedyTransitions().ToString(), ModGUIStyles.BtnStyle))
                ModConfig.Instance.ToggleSpeedyTransitions();
        }
        
        private void ShowSpeedyFadeInToggle()
        {
            GUILayout.Label("Speedy fade-ins", ModGUIStyles.LabelStyle);
            if (ModsGUI.CMButton(ModConfig.Instance.HasSpeedyFadeIn().ToString(), ModGUIStyles.BtnStyle))
                ModConfig.Instance.ToggleSpeedyFadeIn();
        }

        private void ShowInstantTextToggle()
        {
            GUILayout.Label("Instant text", ModGUIStyles.LabelStyle);
            if (ModsGUI.CMButton(ModConfig.Instance.HasInstantText().ToString(), ModGUIStyles.BtnStyle))
                ModConfig.Instance.ToggleInstantText();
        }


        private void ShowModHintToggle()
        {
            GUILayout.Label("Show 'open cheatmod' hint when closed", ModGUIStyles.LabelStyle);
            if (ModsGUI.CMButton(ModConfig.Instance.IsToggleShowOpenModHint().ToString(), ModGUIStyles.BtnStyle))
                ModConfig.Instance.ToggleShowOpenModHint();
        }

    }
}
