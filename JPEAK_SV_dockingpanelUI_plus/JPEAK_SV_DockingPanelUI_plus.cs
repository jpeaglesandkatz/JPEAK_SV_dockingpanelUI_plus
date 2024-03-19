using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace JPEAK_SV_dockingpanelUI_plus
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class JPEAK_SV_DockingPanelUI_plus : BaseUnityPlugin
    {

        private const string MyGUID = "com.jpb.JPEAK_SV_dockingpanelUI_plus";
        private const string PluginName = "JPEAK_SV_dockingpanelUI_plus";
        private const string VersionString = "1.0.0";
        
        public static ConfigEntry<float> cfgScaling;
        public static string cfgScalingKey = "Docking screen scaling factor";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {

            cfgScaling = Config.Bind("Docking Panel Scaling", cfgScalingKey, 0.85f,
                new ConfigDescription("Scales some elements of the G docking panel",
                    new AcceptableValueRange<float>(0.5f, 1.0f)));

            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.CreateAndPatchAll(typeof(JPEAK_SV_DockingPanelUI_plus), null);
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            Log = Logger;
        }


        [HarmonyPatch(typeof(ShipInfo), "LoadData")]
        [HarmonyPostfix]
        static void LoadData2(Transform ___itemPanel)
        {

            for (int i = 0; i < ___itemPanel.childCount; i++)
            {
                var childTransform = ___itemPanel.GetChild(i);
                var buttonComponent = childTransform.GetChild(0).GetComponent<Button>();

                if (!buttonComponent.interactable)
                {
                    var textComponent = childTransform.GetChild(0).GetComponentInChildren<Text>();
                    string strippedText = $"<size=15><color=#ffa500><b>{StripTags(textComponent.text)}</b></color></size>";
                    textComponent.text = strippedText;
                }

                // Scale panel
                ___itemPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            }
        }

        [HarmonyPatch(typeof(ShipInfo), "ShowShipInfo")]
        [HarmonyPostfix]
        static void ShowShipInfo(ref GameObject ___shipDataScreen, SpaceShip ___ss)
        {
            //___shipDataScreen.GetComponentInChildren<Text>().text = "<size=10>" + ___ss.stats.GetStats(___ss) + "</size>";
            ___shipDataScreen.transform.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);

        }

        static string StripTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }


        [HarmonyPatch(typeof(Inventory), "LoadItems")]
        [HarmonyPostfix]
        static void LoadItems(Transform ___itemPanel, ref CargoSystem ___cs)
        {

            for (int i = 0; i < ___itemPanel.childCount; i++)
            {
                var childTransform = ___itemPanel.GetChild(i);
                var buttonComponent = childTransform.GetChild(0).GetComponent<Button>();

                if (!buttonComponent.interactable)
                {
                    var textComponent = childTransform.GetChild(0).GetComponentInChildren<Text>();
                    string strippedText = $"<size=15><color=#ffa500><b>{StripTags(textComponent.text)}</b></color></size>";
                    textComponent.text = strippedText;
                }
            }

            ___itemPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
        }

        [HarmonyPatch(typeof(DockingUI), "ShowQuests")]
        [HarmonyPostfix]
        static void ShowQuests_post(Transform ___questsPanel, DockingUI __instance, GameObject ___lobbyPanel, Transform ___contactsPanel)
        {
            // Scales the whole docking screen
            //__instance.transform.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___questsPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___contactsPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___lobbyPanel.transform.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);

        }

        [HarmonyPatch(typeof(BlueprintCrafting), "LoadData")]
        [HarmonyPostfix]
        static void LoadData_post(Transform ___itemPanel, Transform ___matsPanel, GameObject ___tierPanel)
        {
            //Transform cp = ___componentsPanel;

            ___itemPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___matsPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___tierPanel.transform.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);

        }

        [HarmonyPatch(typeof(WeaponCrafting), "LoadItems")]
        [HarmonyPostfix]
        static void LoadItemspost2(Transform ___componentsPanel, Transform ___modifiersPanel, Transform ___selComponentsPanel, Transform ___selModifiersPanel)
        {
            //Transform cp = ___componentsPanel;

            ___componentsPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___modifiersPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___selComponentsPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            ___selModifiersPanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
        }

        [HarmonyPatch(typeof(Market), "LoadItems")]
        [HarmonyPostfix]
        static void LoadItemspostfix(Transform ___ItemPanel, GameObject ___shipDataScreen)
        {
            Transform itempanel = ___ItemPanel;
            GameObject shipdatascreen = ___shipDataScreen;

            itempanel.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);
            shipdatascreen.transform.localScale = new Vector3(cfgScaling.Value, cfgScaling.Value, cfgScaling.Value);

        }

    }
}
