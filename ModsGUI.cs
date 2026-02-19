using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarmakerSpeedupMod
{
    internal class ModsGUI : MonoBehaviour
    {
        public interface IPageGUI
        {
            void RenderPage();
            string GetPageDescription();
        }

        public static ModsGUI Instance;
        private ModsGUI() { }

        public static float ModWindowWidth = 850f;//(float)(Screen.width / 4) + 100f;
        public static float ModWindowLineHeight = 26f;
        public static int ModWindowPadding = 6;
        public static float ModWindowOffsetX = 10;
        public static float ModWindowOffsetY = 10;

        private static int CurrentLineCount;
        private bool ShowMod;
        private int CurrentPage = 1;
        private int TotalPages = 1;

        private static Dictionary<int, IPageGUI> pages = new Dictionary<int, IPageGUI>();


        public void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            gameObject.AddComponent<Page1GUI>();
        }

        private void Start()
        {
            int page = 0;
            pages.Add(++page, Page1GUI.Instance);
        }

        private void Update()
        {
            HandleKeys();
        }

        public void HandleKeys()
        {
            if (Input.GetKeyDown(ModConfig.Instance.GetShowModsKey()))
                ShowMod = !ShowMod;
        }


        // due to MonoBehaviour, called on each frame from unity
        private void OnGUI()
        {
            // reset line counter
            CurrentLineCount = 0;

            if (ShowMod)
                ShowCheatGUIPages();
            else if (ModConfig.Instance.IsToggleShowOpenModHint())
                ShowCheatGUIHint();
        }

        public void PageChange(int page)
        {
            CurrentPage = Mathf.Clamp(CurrentPage + page, 1, TotalPages);
        }

        public void ShowCheatGUIPages()
        {
            NewLine(() =>
            {
                GUILayout.Label(Mods.Instance.Info.Metadata.Version + " - " + Mods.Instance.Info.Metadata.Name, ModGUIStyles.TitleStyle);
                GUILayout.Space(ModWindowPadding);
            });

            pages[CurrentPage].RenderPage();
        }

        public bool IsCurrentPage(IPageGUI page)
        {
            return ShowMod && pages[CurrentPage] == page;
        }

        public void ShowCheatGUIHint()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(ModWindowOffsetY);
            GUILayout.BeginHorizontal();
            GUILayout.Space(ModWindowOffsetX);
            string onOrOff = (ModConfig.Instance.HasSpeedyTransitions() ? "ON" : "OFF");
            GUILayout.Label("Speedy Transitions " + onOrOff + " (Menu: " + ModConfig.Instance.GetShowModsKey() + ")", ModGUIStyles.TitleStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static void NewLine(params Action[] actions)
        {
            StartLine();
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Invoke();
            }
            EndLine();
        }
        public static Action HalfWidth(Action a)
        {
            return () =>
            {
                GUILayout.BeginHorizontal(ModGUIStyles.HalfWidthContainer);
                a();
                GUILayout.EndHorizontal();
            };
        }

        private static void StartLine()
        {
            GUILayout.BeginArea(new Rect(ModWindowOffsetX, ModWindowOffsetY + (CurrentLineCount * (ModWindowLineHeight + ModWindowPadding)), ModWindowWidth, ModWindowLineHeight + ModWindowPadding));
            GUILayout.BeginHorizontal(ModGUIStyles.BackgroundStyle);
        }
        private static void EndLine()
        {
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            CurrentLineCount++;
        }

        public static void ClearFocus()
        {
            GUIUtility.keyboardControl = 0;
        }

        public static bool CMButton(string text, GUIStyle style, bool clearFocus = true, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(text, style, options))
            {
                if (clearFocus)
                    ClearFocus();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
