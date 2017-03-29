﻿// <copyright file="GPGSUpgrader.cs" company="Google Inc.">
// Copyright (C) 2014 Google Inc.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))

namespace GooglePlayGames.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// GPGS upgrader handles performing and upgrade tasks.
    /// </summary>
    [InitializeOnLoad]
    public class GPGSUpgrader
    {
        /// <summary>
        /// Initializes static members of the <see cref="GooglePlayGames.GPGSUpgrader"/> class.
        /// </summary>
        static GPGSUpgrader()
        {
            string prevVer = GPGSProjectSettings.Instance.Get(GPGSUtil.LASTUPGRADEKEY, "00000");
            if (!prevVer.Equals(PluginVersion.VersionKey))
            {
                // if this is a really old version, upgrade to 911 first, then 915
                if (!prevVer.Equals(PluginVersion.VersionKeyCPP))
                {
                    prevVer = Upgrade911(prevVer);
                }

                prevVer = Upgrade915(prevVer);

                prevVer = Upgrade927Patch(prevVer);

                // Upgrade to remove gpg version of jar resolver
                prevVer = Upgrade928(prevVer);

                prevVer = Upgrade930(prevVer);

                prevVer = Upgrade931(prevVer);

                prevVer = Upgrade935(prevVer);

                // there is no migration needed to 930+
                if (!prevVer.Equals(PluginVersion.VersionKey))
                {
                    Debug.Log("Upgrading from format version " + prevVer + " to " + PluginVersion.VersionKey);
                    prevVer = PluginVersion.VersionKey;
                }

                string msg = GPGSStrings.PostInstall.Text.Replace(
                                 "$VERSION",
                                 PluginVersion.VersionString);
                EditorUtility.DisplayDialog(GPGSStrings.PostInstall.Title, msg, "OK");
            }

            GPGSProjectSettings.Instance.Set(GPGSUtil.LASTUPGRADEKEY, prevVer);
            GPGSProjectSettings.Instance.Set(GPGSUtil.PLUGINVERSIONKEY,
                PluginVersion.VersionString);
            GPGSProjectSettings.Instance.Save();

            // clean up duplicate scripts if Unity 5+
            int ver = GPGSUtil.GetUnityMajorVersion();

            if (ver >= 5)
            {
                string[] paths =
                    {
                        "Assets/ThirdParty/GooglePlayGames",
                        "Assets/Plugins/Android",
                        "Assets/ThirdParty/PlayServicesResolver"
                    };
                foreach (string p in paths)
                {
                    CleanDuplicates(p);
                }

                // remove support lib from old location.
                string jarFile =
                    "Assets/Plugins/Android/libs/android-support-v4.jar";
                if (File.Exists(jarFile))
                {
                    File.Delete(jarFile);
                }

                // remove the massive play services client lib
                string clientDir = "Assets/Plugins/Android/google-play-services_lib";
                GPGSUtil.DeleteDirIfExists(clientDir);
            }

            // Check that there is a AndroidManifest.xml file
            if (!GPGSUtil.AndroidManifestExists())
            {
                GPGSUtil.GenerateAndroidManifest();
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Cleans the duplicate files.  There should not be any since
        /// we are keeping track of the .meta files.
        /// </summary>
        /// <param name="root">Root of the directory to clean.</param>
        private static void CleanDuplicates(string root)
        {
            string[] subDirs = Directory.GetDirectories(root);

            // look for .1 and .2
            string[] dups = Directory.GetFiles(root, "* 1.*");
            foreach (string d in dups)
            {
                Debug.Log("Deleting duplicate file: " + d);
                File.Delete(d);
            }

            dups = Directory.GetFiles(root, "* 2.*");
            foreach (string d in dups)
            {
                Debug.Log("Deleting duplicate file: " + d);
                File.Delete(d);
            }

            // recurse
            foreach (string s in subDirs)
            {
                CleanDuplicates(s);
            }
        }

        /// <summary>
        /// Upgrade to 0.9.35
        /// </summary>
        /// <remarks>
        /// This cleans up some unused files mostly related to the improved jar resolver.
        /// </remarks>
        /// <param name="prevVer">Previous ver.</param>
        private static string Upgrade935(string prevVer)
        {
            string[] obsoleteFiles =
                {
                "Assets/ThirdParty/GooglePlayGames/Editor/CocoaPodHelper.cs",
                "Assets/ThirdParty/GooglePlayGames/Editor/CocoaPodHelper.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Editor/GPGSInstructionWindow.cs",
                "Assets/ThirdParty/GooglePlayGames/Editor/GPGSInstructionWindow.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Editor/Podfile.txt",
                "Assets/ThirdParty/GooglePlayGames/Editor/Podfile.txt.meta",
                "Assets/ThirdParty/GooglePlayGames/Editor/cocoapod_instructions",
                "Assets/ThirdParty/GooglePlayGames/Editor/cocoapod_instructions.meta",
                "Assets/ThirdParty/GooglePlayGames/Editor/ios_instructions",
                "Assets/ThirdParty/GooglePlayGames/Editor/ios_instructions.meta",

                "Assets/ThirdParty/PlayServicesResolver/Editor/DefaultResolver.cs",
                "Assets/ThirdParty/PlayServicesResolver/Editor/DefaultResolver.cs.meta",
                "Assets/ThirdParty/PlayServicesResolver/Editor/IResolver.cs",
                "Assets/ThirdParty/PlayServicesResolver/Editor/IResolver.cs.meta",
                "Assets/ThirdParty/PlayServicesResolver/Editor/JarResolverLib.dll",
                "Assets/ThirdParty/PlayServicesResolver/Editor/JarResolverLib.dll.meta",
                "Assets/ThirdParty/PlayServicesResolver/Editor/PlayServicesResolver.cs",
                "Assets/ThirdParty/PlayServicesResolver/Editor/PlayServicesResolver.cs.meta",
                "Assets/ThirdParty/PlayServicesResolver/Editor/ResolverVer1_1.cs",
                "Assets/ThirdParty/PlayServicesResolver/Editor/ResolverVer1_1.cs.meta",
                "Assets/ThirdParty/PlayServicesResolver/Editor/SampleDependencies.cs",
                "Assets/ThirdParty/PlayServicesResolver/Editor/SampleDependencies.cs.meta",
                "Assets/ThirdParty/PlayServicesResolver/Editor/SettingsDialog.cs",
                "Assets/ThirdParty/PlayServicesResolver/Editor/SettingsDialog.cs.meta",

                "Assets/Plugins/Android/play-services-plus-8.4.0.aar",
                "Assets/ThirdParty/PlayServicesResolver/Editor/play-services-plus-8.4.0.aar.meta",

                // not an obsolete file, but delete the cache since the schema changed.
                "ProjectSettings/GoogleDependencyGooglePlayGames.xml"
            };
            foreach (string file in obsoleteFiles)
            {
                if (File.Exists(file))
                {
                    Debug.Log("Deleting obsolete file: " + file);
                    File.Delete(file);
                }
            }

            return PluginVersion.VersionKey;
        }

        /// <summary>
        /// Upgrade to 0.9.31
        /// </summary>
        /// <remarks>
        /// This cleans up some unused files.
        /// </remarks>
        /// <param name="prevVer">Previous ver.</param>
        private static string Upgrade931(string prevVer)
        {
            string[] obsoleteFiles =
                {
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSExportPackageUI.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSExportPackageUI.cs.meta"
                };
            foreach (string file in obsoleteFiles)
            {
                if (File.Exists(file))
                {
                    Debug.Log("Deleting obsolete file: " + file);
                    File.Delete(file);
                }
            }

            return PluginVersion.VersionKey;
        }

        /// <summary>
        /// Upgrade to 930 from the specified prevVer.
        /// </summary>
        /// <param name="prevVer">Previous ver.</param>
        /// <returns>the version string upgraded to.</returns>
        private static string Upgrade930(string prevVer)
        {
            Debug.Log("Upgrading from format version " + prevVer + " to " + PluginVersion.VersionKeyNativeCRM);

            // As of 930, the CRM API is handled by the Native SDK, not GmsCore.
            string[] obsoleteFiles =
            {
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Games.cs",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Games.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/LoadPlayerStatsResultObject.cs",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/LoadPlayerStatsResultObject.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/PlayerStats.cs",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/PlayerStats.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/PlayerStatsObject.cs",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/PlayerStatsObject.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/Stats.cs",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/Stats.cs.meta",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/StatsObject.cs",
                "Assets/ThirdParty/GooglePlayGames/Platforms/Android/Gms/Games/Stats/StatsObject.cs.meta"
            };

            // only delete these if we are not version 0.9.34
            if (string.Compare(PluginVersion.VersionKey, PluginVersion.VersionKeyJNIStats,
                               System.StringComparison.Ordinal) <= 0)
            {
                foreach (string file in obsoleteFiles)
                {
                    if (File.Exists(file))
                    {
                        Debug.Log("Deleting obsolete file: " + file);
                        File.Delete(file);
                    }
                }
            }

            return PluginVersion.VersionKeyNativeCRM;
        }

        private static string Upgrade928(string prevVer)
        {
            //remove the jar resolver and if found, then
            // warn the user that restarting the editor is required.
            string[] obsoleteFiles =
                {
                    "Assets/ThirdParty/GooglePlayGames/Editor/JarResolverLib.dll",
                    "Assets/ThirdParty/GooglePlayGames/Editor/JarResolverLib.dll.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/BackgroundResolution.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/BackgroundResolution.cs.meta"
                };

            bool found = File.Exists(obsoleteFiles[0]);

            foreach (string file in obsoleteFiles)
            {
                if (File.Exists(file))
                {
                    Debug.Log("Deleting obsolete file: " + file);
                    File.Delete(file);
                }
            }

            if (found)
            {
                GPGSUtil.Alert("This update made changes that requires that you restart the editor");
            }

            Debug.Log("Upgrading from version " + prevVer + " to " + PluginVersion.VersionKeyJarResolver);
            return PluginVersion.VersionKeyJarResolver;
        }

        /// <summary>
        /// Upgrade to 0.9.27a.
        /// </summary>
        /// <remarks>This removes the GPGGizmo class, which broke the editor</remarks>
        /// <returns>The patched version</returns>
        /// <param name="prevVer">Previous version</param>
        private static string Upgrade927Patch(string prevVer)
        {
            string[] obsoleteFiles =
                {
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGGizmo.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGGizmo.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/BasicApi/OnStateLoadedListener.cs",
                    "Assets/ThirdParty/GooglePlayGames/BasicApi/OnStateLoadedListener.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Platforms/Native/AndroidAppStateClient.cs",
                    "Assets/ThirdParty/GooglePlayGames/Platforms/Native/AndroidAppStateClient.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Platforms/Native/UnsupportedAppStateClient.cs",
                    "Assets/ThirdParty/GooglePlayGames/Platforms/Native/UnsupportedAppStateClient.cs.meta"
                };
            foreach (string file in obsoleteFiles)
            {
                if (File.Exists(file))
                {
                    Debug.Log("Deleting obsolete file: " + file);
                    File.Delete(file);
                }
            }

            return PluginVersion.VersionKey27Patch;
        }

        /// <summary>
        /// Upgrade to 915 from the specified prevVer.
        /// </summary>
        /// <param name="prevVer">Previous ver.</param>
        /// <returns>the version string upgraded to.</returns>
        private static string Upgrade915(string prevVer)
        {
            Debug.Log("Upgrading from format version " + prevVer + " to " + PluginVersion.VersionKeyU5);

            // all that was done was moving the Editor files to be in GooglePlayGames/Editor
            string[] obsoleteFiles =
                {
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSAndroidSetupUI.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSAndroidSetupUI.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSDocsUI.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSDocsUI.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSIOSSetupUI.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSIOSSetupUI.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSInstructionWindow.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSInstructionWindow.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSPostBuild.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSPostBuild.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSProjectSettings.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSProjectSettings.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSStrings.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSStrings.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSUpgrader.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSUpgrader.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSUtil.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GPGSUtil.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GameInfo.template",
                    "Assets/ThirdParty/GooglePlayGames/Editor/GameInfo.template.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/PlistBuddyHelper.cs",
                    "Assets/ThirdParty/GooglePlayGames/Editor/PlistBuddyHelper.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/PostprocessBuildPlayer",
                    "Assets/ThirdParty/GooglePlayGames/Editor/PostprocessBuildPlayer.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/ios_instructions",
                    "Assets/ThirdParty/GooglePlayGames/Editor/ios_instructions.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/projsettings.txt",
                    "Assets/ThirdParty/GooglePlayGames/Editor/projsettings.txt.meta",
                    "Assets/ThirdParty/GooglePlayGames/Editor/template-AndroidManifest.txt",
                    "Assets/ThirdParty/GooglePlayGames/Editor/template-AndroidManifest.txt.meta",
                    "Assets/Plugins/Android/libs/armeabi/libgpg.so",
                    "Assets/Plugins/Android/libs/armeabi/libgpg.so.meta",
                    "Assets/Plugins/iOS/GPGSAppController 1.h",
                    "Assets/Plugins/iOS/GPGSAppController 1.h.meta",
                    "Assets/Plugins/iOS/GPGSAppController 1.mm",
                    "Assets/Plugins/iOS/GPGSAppController 1.mm.meta"
                };

            foreach (string file in obsoleteFiles)
            {
                if (File.Exists(file))
                {
                    Debug.Log("Deleting obsolete file: " + file);
                    File.Delete(file);
                }
            }

            return PluginVersion.VersionKeyU5;
        }

        /// <summary>
        /// Upgrade to 911 from the specified prevVer.
        /// </summary>
        /// <param name="prevVer">Previous ver.</param>
        /// <returns>the version string upgraded to.</returns>
        private static string Upgrade911(string prevVer)
        {
            Debug.Log("Upgrading from format version " + prevVer + " to " + PluginVersion.VersionKeyCPP);

            // delete obsolete files, if they are there
            string[] obsoleteFiles =
                {
                    "Assets/ThirdParty/GooglePlayGames/OurUtils/Utils.cs",
                    "Assets/ThirdParty/GooglePlayGames/OurUtils/Utils.cs.meta",
                    "Assets/ThirdParty/GooglePlayGames/OurUtils/MyClass.cs",
                    "Assets/ThirdParty/GooglePlayGames/OurUtils/MyClass.cs.meta",
                    "Assets/Plugins/GPGSUtils.dll",
                    "Assets/Plugins/GPGSUtils.dll.meta",
                };

            foreach (string file in obsoleteFiles)
            {
                if (File.Exists(file))
                {
                    Debug.Log("Deleting obsolete file: " + file);
                    File.Delete(file);
                }
            }

            // delete obsolete directories, if they are there
            string[] obsoleteDirectories =
                {
                    "Assets/Plugins/Android/BaseGameUtils"
                };

            foreach (string directory in obsoleteDirectories)
            {
                if (Directory.Exists(directory))
                {
                    Debug.Log("Deleting obsolete directory: " + directory);
                    Directory.Delete(directory, true);
                }
            }

            Debug.Log("Done upgrading from format version " + prevVer + " to " + PluginVersion.VersionKeyCPP);
            return PluginVersion.VersionKeyCPP;
        }
    }
}
#endif
