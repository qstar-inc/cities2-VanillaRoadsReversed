using Colossal.PSI.Common;
using Colossal.PSI.Environment;
using Game;
using Game.Modding;
using Game.UI.Menu;
using System.IO;
using System.Linq;

namespace VanillaRoadsReversed
{
    public partial class ThumbnailProcessor(Mod mod) : GameSystemBase
    {
        public Mod _mod = mod;
        public static VanillaRoadsReversedSetting Setting = Mod.Setting;
        public static NotificationUISystem notificationUISystem = Mod.notificationUISystem;
        public static ModManager modManager = Mod.modManager;
        public static string notificationName = "starq-vanilla-roads-reversed-thumbnail-process";

        protected override void OnCreate()
        {
            base.OnCreate();
            if (!Setting.ProcessedThumbnails)
            {
                ProcessThumbnail();
            } 
        }

        protected override void OnUpdate()
        {
        }

        public static void ProcessThumbnail()
        {
            string ailCustomFolder = EnvPath.kUserDataPath + "/ModsData/AssetIconLibrary/CustomThumbnails";
            bool ailFound = false;

            var thumbNotification = notificationUISystem.AddOrUpdateNotification(notificationName,
                title: "Processing Thumbnails (Vanilla Roads Reversed)",
            text: "Starting system...",
                progressState: ProgressState.Indeterminate,
                progress: 0);

            foreach (var modInfo in modManager)
            {
                string modName = modInfo.asset.name;
                if (modName.StartsWith("AssetIconLibrary"))
                {
                    ailFound = true;
                    break;
                    
                }
            }

            if (ailFound)
            {
                thumbNotification.text = "Asset Icon Library found...";
                thumbNotification.progressState = ProgressState.Progressing;
                thumbNotification.progress = (2 / 4 * 100);
                if (!CheckExistingFiles(ailCustomFolder, thumbNotification))
                {
                    FindAndCopyFiles(ailCustomFolder, thumbNotification);
                }
            }
            else
            {
                thumbNotification.text = "Asset Icon Library not found, cancelling...";
                thumbNotification.progressState = ProgressState.Complete;
                thumbNotification.progress = (4 / 4 * 100);
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
        }

        protected static bool CheckExistingFiles(string ailCustomFolder, NotificationUISystem.NotificationInfo thumbNotification = null)
        {
            string[] files =
            {
                "StarQ Highway Twoway - 6 lanes.svg",
                "StarQ Highway Twoway - 8 lanes.svg",
                "StarQ Reversed Highway Twoway - 2 lanes.svg",
                "StarQ Reversed Highway Twoway - 3 lanes.svg",
                "StarQ Reversed Highway Twoway - 4 lanes.svg",
                "StarQ Reversed Highway Twoway - 6 lanes.svg",
                "StarQ Reversed Highway Twoway - 8 lanes.svg",
                "StarQ Reversed Truss Arch Bridge - Highway Twoway - 2 Lanes.svg",
                "StarQ Reversed Cable-stayed Bridge - XL Road Divided - 8 Lanes.svg",
                "StarQ Reversed Extradosed Bridge - Large Road Divided - 6 Lanes.svg",
                "StarQ Reversed Large Road.svg",
                "StarQ Reversed Large Road Asymmetric.svg",
                "StarQ Reversed Large Road Divided.svg",
                "StarQ Reversed XL Road Divided.svg",
                "StarQ Reversed Golden Gate Bridge.svg",
                "StarQ Reversed Golden Gate Road.svg",
                "StarQ Reversed Grand Bridge.svg",
                "StarQ Reversed Medium Road.svg",
                "StarQ Reversed Medium Road Asymmetric.svg",
                "StarQ Reversed Medium Road Divided.svg",
                "StarQ Reversed Tied Arch Bridge - 4 lanes.svg",
                "StarQ Reversed Alley.svg",
                "StarQ Reversed Double Public Transport Lane.svg",
                "StarQ Reversed Gravel Road.svg",
                "StarQ Reversed Pedestrian Street.svg",
                "StarQ Reversed Small Road.svg",
                "StarQ Reversed Small Road Asymmetric.svg",
                "StarQ Reversed Truss Arch Bridge - Small Road - 2 Lanes.svg",
                "StarQ Reversed Wooden Covered Bridge - 2 lanes.svg",
            };

            Directory.CreateDirectory(ailCustomFolder);
            string[] existingFiles = Directory.GetFiles(ailCustomFolder, "*.svg");
            int count = 0;

            if (existingFiles.Length > 0)
            {
                foreach (var file in files)
                {
                    if (existingFiles.Contains(ailCustomFolder + "\\" + file))
                    {
                        count++;
                    }
                }
            }

            if (thumbNotification != null && count == files.Length)
            {
                thumbNotification.text = "Thumbnails already exists; cancelling...";
                thumbNotification.progressState = ProgressState.Complete;
                thumbNotification.progress = (4 / 4 * 100);
                Setting.ProcessedThumbnails = true;
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
            return count == files.Length;

        }

        protected static void FindAndCopyFiles(string ailCustomFolder, NotificationUISystem.NotificationInfo thumbNotification)
        {
            string assetPath = null;
            foreach (var modInfo in modManager)
            {
                if (modInfo.asset.path.Contains("/VanillaRoadsReversed.dll"))
                {
                    assetPath = modInfo.asset.path.Replace("/VanillaRoadsReversed.dll", "");
                }
                else if (modInfo.asset.path.Contains("\\VanillaRoadsReversed.dll"))
                {
                    assetPath = modInfo.asset.path.Replace("\\VanillaRoadsReversed.dll", "");
                }

            }

            if (assetPath != null)
            {
                string thumbdirectory = assetPath + "\\thumbs";
                string[] files = Directory.GetFiles(thumbdirectory, "*.svg");
                foreach (var file in files)
                {
                    File.Copy(file, ailCustomFolder + "\\" + file.Substring(file.LastIndexOf("\\")), true);
                    thumbNotification.text = $"Configuring thumbnails...";
                    thumbNotification.progressState = ProgressState.Progressing;
                    thumbNotification.progress = (3 / 4 * 100);
                }

            }
            if (CheckExistingFiles(ailCustomFolder))
            {
                thumbNotification.text = "Thumbnail processing completed...";
                thumbNotification.progressState = ProgressState.Complete;
                thumbNotification.progress = (4 / 4 * 100);
                Setting.ProcessedThumbnails = true;
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
        }
    }
}
