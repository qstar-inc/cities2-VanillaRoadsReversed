using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;

namespace VanillaRoadsReversed
{
    [FileLocation($"ModsSettings\\StarQ\\{nameof(VanillaRoadsReversed)}")]
    public class VanillaRoadsReversedSetting : ModSetting
    {
        public const string sectionMain = "Main";
        public const string actions = "Actions";

        public VanillaRoadsReversedSetting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        [SettingsUIHidden]
        public bool ProcessedThumbnails { get; set; }


        [SettingsUIButton]
        [SettingsUISection(sectionMain, actions)]
        public bool RedoThumbnail
        {
            set { ThumbnailProcessor.ProcessThumbnail(); }
        }

        public override void SetDefaults()
        {
            ProcessedThumbnails = false;
        }
    }

    public class LocaleEN(VanillaRoadsReversedSetting setting) : IDictionarySource
    {
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { setting.GetSettingsLocaleID(), Mod.ModName },
                { setting.GetOptionTabLocaleID(VanillaRoadsReversedSetting.sectionMain), "Main" },
                { setting.GetOptionGroupLocaleID(VanillaRoadsReversedSetting.actions), "Actions" },

                { setting.GetOptionLabelLocaleID(nameof(VanillaRoadsReversedSetting.RedoThumbnail)), "Reset Thumbnail Cache [powered by Asset Icon Library]" },
                { setting.GetOptionDescLocaleID(nameof(VanillaRoadsReversedSetting.RedoThumbnail)), $"This button will recreate the thumbnails if necessary. The thumbnails are loaded by Asset Icon Library. Make sure you're already subscribed to it to have the thumbnails." },

            };
        }

        public void Unload()
        {

        }
    }
}
