using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using I2.Loc;
using Microsoft.Extensions.Configuration;
using ShinyShoe.Logging;
using SimpleInjector;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Card;
using TrainworksReloaded.Base.CardUpgrade;
using TrainworksReloaded.Base.Character;
using TrainworksReloaded.Base.Class;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Base.Localization;
using TrainworksReloaded.Base.Prefab;
using TrainworksReloaded.Base.Trait;
using TrainworksReloaded.Base.Trigger;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Impl;
using TrainworksReloaded.Core.Interfaces;
using TrainworksReloaded.Core.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TrainworksReloaded.Base.Extensions;

namespace NetherworldEvokers.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        public void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    // Be sure to include all of your json files if you add more.
                    // Be sure to update the project configuration if you include more folders
                    //   the project only copies json files in the json folder and not in subdirectories.
                    c.AddMergedJsonFile(
                        "champions/champion_silver.json",
                        "characters/character_spirit_butterfly.json",
                        "characters/character_kitsune_spirit.json",
                        "characters/character_moonblighted_outcast.json",
                        "cards/FeralRage.json",
                        "plugin.json",
                        "status.json"
                    );
                }
            );
            Railend.ConfigurePostAction(
                c =>
                {
                    var manager = c.GetInstance<IRegister<CharacterTriggerData.Trigger>>();
                    CharacterTriggerData.Trigger GetTrigger(string name, string mod_reference)
                    {
                        return manager.GetValueOrDefault(name.ToId(mod_reference, TemplateConstants.CharacterTriggerEnum));
                    }
                    Triggers.OnSoulburn = GetTrigger("@OnDebuffed", "Conductor");
                }
            );
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            // Uncomment if you need harmony patches, if you are writing your own custom effects.
            //var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            //harmony.PatchAll();
        }
    }
}
