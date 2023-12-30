using System.Collections.Generic;
using System.IO;
using System.Linq;
using HybridCLR.Editor.Commands;
using Newtonsoft.Json;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace UnityEditor.AddressableAssets.Build
{
    public class BuildTools : Editor
    {
        [MenuItem("Tools/打包流程/资源初始化")]
        public static void InitBuildAssets()
        {
            //编译DLL和环境
            Debug.Log("开始 GenerateAll...");
            PrebuildCommand.GenerateAll();
            Debug.Log("完成 GenerateAll...");

            CompileDllCommand.CompileDllActiveBuildTarget();

            //生成热更
            HotUpdate();

            //全量编译AddressAble
            AddressableAssetSettings.CleanPlayerContent();

            AssetDatabase.Refresh();

            AddressableAssetSettings.BuildPlayerContent();

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/打包流程/打包本平台")]
        public static void BuildGame()
        {
            //刷新资源系统
            AssetDatabase.Refresh();

            var dict = new Dictionary<BuildTarget, string>()
            {
                { BuildTarget.StandaloneWindows64, ".exe" },
                { BuildTarget.Android, ".apk" },
            };
            string prefix = dict.GetValueOrDefault(EditorUserBuildSettings.activeBuildTarget, "");
            // string outputPath = Application.dataPath.Replace("/Assets", $"/BuildOutput/{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}{prefix}");
            string outputPath =
                Application.dataPath.Replace("/Assets", $"/BuildOutput/{EditorUserBuildSettings.activeBuildTarget}");
            //如果没有创建目录
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            //会删除顶级目录
            Directory.Delete(outputPath, true);
            BuildPlayerOptions buildOptions = new BuildPlayerOptions();
            List<string> scenes = new();

            //进包场景
            foreach (var editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                scenes.Add(editorBuildSettingsScene.path);
            }

            buildOptions.scenes = scenes.ToArray();
            buildOptions.target = EditorUserBuildSettings.activeBuildTarget;
            // 输出到实际的文件名下
            buildOptions.locationPathName = $"{outputPath}/{PlayerSettings.productName}{prefix}";
            BuildPipeline.BuildPlayer(buildOptions);
            // EditorApplication.Exit(0);
        }

        [MenuItem("Tools/打包流程/生成热更新")]
        public static void HotUpdate()
        {
            Debug.Log("热更新流程");

            //编译热更C# 代码
            CompileDllCommand.CompileDllActiveBuildTarget();

            CopyHotUpdateDll();

            CopyAOTDll();

            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem("Tools/打包流程/一键重出本平台包")]
        public static void OneKeyBuild()
        {
            InitBuildAssets();
            BuildGame();
        }

        protected static void CopyHotUpdateDll()
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var dataPath = Application.dataPath;
            // 需要转移的文件(手动维护)
            string[] files = new string[] { "HotUpdate.dll" };
            // 来源地址
            string sourcePath = dataPath.Replace("/Assets", "/HybridCLRData/HotUpdateDlls/" + activeBuildTarget);
            // 目标地址
            string destPath = $"{dataPath}/GameMain/Dlls";
            // 粘贴过去            
            CopyDll(sourcePath, destPath, files);
        }

        protected static void CopyAOTDll()
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var dataPath = Application.dataPath;
            // 需要转移的文件
            var files = AOTGenericReferences.PatchedAOTAssemblyList.ToArray();
            // 来源地址
            string sourcePath =
                dataPath.Replace("/Assets", "/HybridCLRData/AssembliesPostIl2CppStrip/" + activeBuildTarget);
            // 目标地址
            string destPath = $"{dataPath}/GameMain/AOTDlls";
            // 粘贴过去
            CopyDll(sourcePath, destPath, files);
        }

        /// <summary>
        /// 拷贝热更文件到目标文件夹
        /// </summary>
        protected static void CopyDll(string sourcePath, string destPath, string[] files)
        {
            foreach (var file in files)
            {
                string source = $"{sourcePath}/{file}";
                string dest = $"{destPath}/{file}.bytes";
                CoverFile(source, dest);
            }

            // 写入配置
            var json = JsonConvert.SerializeObject(files);
            GameUtils.FileWrite($"{destPath}/info.txt", json);
            //刷新资源系统
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 覆盖文件
        /// </summary>
        protected static void CoverFile(string source, string dest)
        {
            //如果有则删除
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }

            FileUtil.CopyFileOrDirectory(source, dest);
        }
    }
}