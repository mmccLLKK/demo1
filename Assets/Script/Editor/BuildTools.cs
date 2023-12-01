using System.IO;
using System.Linq;
using HybridCLR.Editor.Commands;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace UnityEditor.AddressableAssets.Build
{
    public class BuildTools : Editor
    {
        [MenuItem("Tools/打包流程/全量打包")]
        public static void Build()
        {
            //编译DLL和环境
            Debug.Log("开始 GenerateAll...");
            PrebuildCommand.GenerateAll();
            Debug.Log("完成 GenerateAll...");

            CopyHotUpdateDll();

            CopyAOTDll();

            //全量编译AddressAble
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem("Tools/打包流程/热更新")]
        public static void HotUpdate()
        {
            Debug.Log("热更新流程");

            CopyHotUpdateDll();

            CopyAOTDll();
        }

        protected static void CopyHotUpdateDll()
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var dataPath = Application.dataPath;
            // 需要转移的文件(手动维护)
            string[] files = new string[] {"HotUpdate.dll"};
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
            string sourcePath = dataPath.Replace("/Assets", "/HybridCLRData/AssembliesPostIl2CppStrip/" + activeBuildTarget);
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