
using BepInEx;
using GameCreator.Runtime.Common;
using HarmonyLib;
using StarmakerSpeedupMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[BepInPlugin(NAMESPACE, TITLE, VERSION)]
public class Mods : BaseUnityPlugin
{
    public const string NAMESPACE = "StarmakerSpeeupMod";
    public const string TITLE = "starmaker speedup mod - selectivepaperclip";
    public const string VERSION = "0.0.1";

    // entry point, called once from Unity due to BaseUnityPlugin (MonoBehaviour)
    // Mind the split between awake and start, awake should only handle instance creation and gameObject relevant stuff
    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
        gameObject.AddComponent<ModsGUI>();

        var harmony = new Harmony(NAMESPACE);
        harmony.PatchAll();
    }

    public static Mods Instance;
    private Mods() { }

    // Start is called once from Unity before first frame and after all awakes are handled, due to BaseUnityPlugin (MonoBehaviour),
    private void Start()
    {
        ModConfig.Instance.ConfigBindings(Instance.Config);
    }

    // Update is called for each frame from Unity due to BaseUnityPlugin (MonoBehaviour)
    private void Update()
    {
        HandleKeys();
    }

    // handle all keypresses here
    private void HandleKeys()
    {
        if (Input.GetKeyDown(ModConfig.Instance.GetToggleSpeedyTransitionsKey()))
            ModConfig.Instance.ToggleSpeedyTransitions();
    }

    internal class GamePatches
    {
        [HarmonyPatch(typeof(CharacterIntroFade), "StartIntroSequence")]
        class CharacterIntroFadePatch
        {
            public static bool Prefix(CharacterIntroFade __instance)
            {
                if (ModConfig.Instance.HasSpeedyFadeIn())
                {
                    PatchedIntroSequence(__instance);
                    return false;
                } else
                {
                    return true;
                }

            }

            public static IEnumerator PatchedIntroSequence(CharacterIntroFade __instance)
            {
                Traverse traverser = Traverse.Create(__instance);
                SpriteRenderer spriteRenderer = (SpriteRenderer)traverser.Method("spriteRenderer").GetValue();
                Color mainColor = spriteRenderer.color;
                mainColor.a = 0f;
                spriteRenderer.color = mainColor;
                yield return null;
                Transform blinkChild = (Transform)traverser.Method("blinkChild").GetValue();
                if (blinkChild != null)
                {
                    blinkChild.gameObject.SetActive(false);
                }
                Transform expressionsChild = (Transform)traverser.Method("expressionsChild").GetValue();
                if (expressionsChild != null)
                {
                    expressionsChild.gameObject.SetActive(false);
                }
                GameObject shadowSprite = (GameObject)traverser.Method("shadowSprite").GetValue();
                if (shadowSprite == null)
                {
                    shadowSprite = new GameObject("ShadowSprite");
                    traverser.Method("shadowSprite").SetValue(shadowSprite);
                    shadowSprite.transform.SetParent(__instance.transform);
                    shadowSprite.transform.localPosition = Vector3.zero;
                    shadowSprite.transform.localRotation = Quaternion.identity;
                    shadowSprite.transform.localScale = Vector3.one;
                    traverser.Method("shadowRenderer").SetValue(shadowSprite.AddComponent<SpriteRenderer>());
                }
                else
                {
                    shadowSprite.SetActive(true);
                }
                SpriteRenderer shadowRenderer = (SpriteRenderer)traverser.Method("shadowRenderer").GetValue();
                shadowRenderer.sprite = spriteRenderer.sprite;
                shadowRenderer.material = spriteRenderer.material;
                shadowRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
                shadowRenderer.sortingOrder = spriteRenderer.sortingOrder + global::UnityEngine.Random.Range(8, 16);
                shadowRenderer.color = new Color(0f, 0f, 0f, 0f);
                shadowSprite.SetActive(false);
                shadowSprite.SetActive(true);
                float elapsed = 0f;
                while (elapsed < 0.1f)
                {
                    elapsed += Time.deltaTime;
                    Color color = shadowRenderer.color;
                    color.a = Mathf.Clamp01(elapsed / 0.1f);
                    shadowRenderer.color = color;
                    yield return null;
                }
                shadowRenderer.color = new Color(0f, 0f, 0f, 1f);
                if (blinkChild != null)
                {
                    blinkChild.gameObject.SetActive(true);
                }
                if (expressionsChild != null)
                {
                    expressionsChild.gameObject.SetActive(true);
                }
                mainColor.a = 1f;
                spriteRenderer.color = mainColor;
                elapsed = 0f;
                while (elapsed < 0.1f)
                {
                    elapsed += Time.deltaTime;
                    Color color2 = shadowRenderer.color;
                    color2.a = Mathf.Clamp01(1f - elapsed / 0.1f);
                    shadowRenderer.color = color2;
                    yield return null;
                }
                shadowRenderer.color = new Color(0f, 0f, 0f, 0f);
                shadowSprite.SetActive(false);
                traverser.Method("activeCoroutine").SetValue(null);
                yield break;
            }
        }


        [HarmonyPatch]
        class TimePatch
        {
            static MethodBase TargetMethod()
            {
                // Use reflection to get the protected method
                return AccessTools.Method(typeof(GameCreator.Runtime.VisualScripting.InstructionCommonTimeWait), "Run");
            }

            public static void Prefix(GameCreator.Runtime.VisualScripting.InstructionCommonTimeWait __instance, out PropertyGetDecimal __state)
            {
                __state = (PropertyGetDecimal)Traverse.Create(__instance).Field("m_Seconds").GetValue();
                if (ModConfig.configLoaded && ModConfig.Instance.HasSpeedyTransitions())
                {
                    Traverse.Create(__instance).Field("m_Seconds").SetValue(new PropertyGetDecimal(0.01f));
                }
            }

            public static void Postfix(PropertyGetDecimal __state, GameCreator.Runtime.VisualScripting.InstructionCommonTimeWait __instance)
            {
                if (ModConfig.configLoaded && ModConfig.Instance.HasSpeedyTransitions())
                {
                    Traverse.Create(__instance).Field("m_Seconds").SetValue(__state);
                }
            }
        }

        [HarmonyPatch]
        class TypewriterDurationPatch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(GameCreator.Runtime.Dialogue.Typewriter), "GetDuration");
            }

            public static bool Prefix(ref float __result)
            {
                if (ModConfig.Instance.HasInstantText())
                {
                    __result = 0f;
                    return false;   
                }
                return true;
            }
        }

        [HarmonyPatch]
        class TypewriterCharactersPatch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(GameCreator.Runtime.Dialogue.Typewriter), "GetCharactersVisible");
            }

            public static bool Prefix(ref int __result)
            {
                if (ModConfig.Instance.HasInstantText())
                {
                    __result = int.MaxValue;
                    return false;
                }
                return true;
            }
        }
    }

}