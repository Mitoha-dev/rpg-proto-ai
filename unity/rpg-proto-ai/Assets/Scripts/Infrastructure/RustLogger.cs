using UnityEngine;
using System;
using System.Runtime.InteropServices;
using AOT;

namespace RpgProto.Infrastructure
{
    public static class RustLogger
    {
        // 1. Rustから送られてくる「ログ出力関数」の型
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LogCallback(IntPtr message);

        // ▼▼▼ ここを修正（Actionではなく、専用の型を作る） ▼▼▼
        // 2. Rustの「register_logger」関数の型
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void RegisterLoggerDelegate(LogCallback callback);
        // ▲▲▲▲▲▲

        [MonoPInvokeCallback(typeof(LogCallback))]
        private static void OnRustLog(IntPtr messagePtr)
        {
            string message = Marshal.PtrToStringAnsi(messagePtr);
            Debug.Log($"<color=orange>[Rust]</color> {message}");
        }

        public static void Register()
        {
            // ▼▼▼ ここを修正（Action<LogCallback> をやめる） ▼▼▼
            var registerFunc = RustLoader.GetFunction<RegisterLoggerDelegate>("register_logger");

            if (registerFunc != null)
            {
                registerFunc(OnRustLog);
                Debug.Log("Rustへのロガー登録完了");
            }
        }
    }
}