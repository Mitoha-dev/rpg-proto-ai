using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace RpgProto.Infrastructure
{
    public static class RustLoader
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        private static IntPtr _dllHandle;

        // DLLをロードする
        public static void Load()
        {
            if (_dllHandle != IntPtr.Zero) return;

            string dllPath = FindDllPath();
            if (string.IsNullOrEmpty(dllPath))
            {
                Debug.LogError("RustのDLLが見つかりません。");
                return;
            }

            _dllHandle = LoadLibrary(dllPath);
            if (_dllHandle == IntPtr.Zero)
            {
                Debug.LogError($"DLLのロードに失敗しました: {dllPath}");
            }
        }

        // DLLを解放する（ホットリロードのために必須）
        public static void Unload()
        {
            if (_dllHandle != IntPtr.Zero)
            {
                FreeLibrary(_dllHandle);
                _dllHandle = IntPtr.Zero;
                Debug.Log("Rust DLLを解放しました。");
            }
        }

        // Rustの関数をC#のデリゲートに変換して取得する
        public static T GetFunction<T>(string name) where T : Delegate
        {
            if (_dllHandle == IntPtr.Zero) Load();

            IntPtr ptr = GetProcAddress(_dllHandle, name);
            if (ptr == IntPtr.Zero)
            {
                Debug.LogError($"関数 {name} が見つかりません。");
                return null;
            }
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        // 自動探索ロジック
        private static string FindDllPath()
        {
            var currentDir = new DirectoryInfo(Application.dataPath);
            while (currentDir != null)
            {
                string path = Path.Combine(currentDir.FullName, "rust", "target", "release", "rpg_logic.dll");
                if (File.Exists(path)) return path;
                currentDir = currentDir.Parent;
            }
            return null;
        }
    }
}