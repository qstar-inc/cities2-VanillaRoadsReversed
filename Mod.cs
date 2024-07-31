using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.UI.Menu;
using Unity.Entities;

namespace VanillaRoadsReversed
{
    public class Mod : IMod
    {
        public const string ModName = "Vanilla Roads Reversed";
        //public static ILog log = LogManager.GetLogger($"{nameof(VanillaRoadsReversed)}").SetShowsErrorsInUI(false);
        public static VanillaRoadsReversedSetting Setting;
        public static NotificationUISystem notificationUISystem;
        public static ModManager modManager = GameManager.instance.modManager;
        public ThumbnailProcessor thumbnailProcessor;
        public void OnLoad(UpdateSystem updateSystem)
        {
            notificationUISystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NotificationUISystem>();
            
            Setting = new VanillaRoadsReversedSetting(this);
            Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(Setting));
            AssetDatabase.global.LoadSettings(nameof(VanillaRoadsReversed), Setting, new VanillaRoadsReversedSetting(this));

            thumbnailProcessor = new ThumbnailProcessor(this);
            World.DefaultGameObjectInjectionWorld.AddSystemManaged(thumbnailProcessor);
        }

        public void OnDispose()
        {
            if (Setting != null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }
        }
    }

}