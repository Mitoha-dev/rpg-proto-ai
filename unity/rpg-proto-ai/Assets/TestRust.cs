using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;

public class TestRust : MonoBehaviour
{
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32", SetLastError = true)]
    static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    private IntPtr _dllHandle;
    private delegate int GetInitialHpDelegate();

     void Start()
    {
        // ▼▼▼ 自動探索ロジック ▼▼▼
        string dllPath = null;

        // スタート地点：Assetsフォルダ
        var currentDir = new DirectoryInfo(Application.dataPath);

        // 上の階層へ登りながら "rust" フォルダを探すループ
        while (currentDir != null)
        {
            // 「もしここに rust/target/release/rpg_logic.dll があったら？」
            string potentialPath = Path.Combine(currentDir.FullName, "rust", "target", "release", "rpg_logic.dll");

            if (File.Exists(potentialPath))
            {
                // 見つけた！これだ！
                dllPath = potentialPath;
                break; // 探索終了
            }

            // 見つからなければ、一つ上の階層へ移動
            currentDir = currentDir.Parent;
        }
        // ▲▲▲ ここまで ▲▲▲

        if (string.IsNullOrEmpty(dllPath))
        {
            Debug.LogError("どこまで探してもRustのDLLが見つかりません！ビルドは済んでいますか？");
            return;
        }

        Debug.Log("DLLを発見しました: " + dllPath);

        // --- 以下は今までと同じ ---
        _dllHandle = LoadLibrary(dllPath);

        if (_dllHandle == IntPtr.Zero)
        {
            Debug.LogError("読み込み失敗！");
            return;
        }

        IntPtr funcPtr = GetProcAddress(_dllHandle, "get_initial_hp");

        if (funcPtr == IntPtr.Zero)
        {
            Debug.LogError("関数が見つかりません！");
            return;
        }

        GetInitialHpDelegate getHp = Marshal.GetDelegateForFunctionPointer<GetInitialHpDelegate>(funcPtr);

        int hp = getHp();
        Debug.Log($"【HotReload成功】Rustから受け取ったHP: {hp}");
    }

    void OnDestroy()
    {
        if (_dllHandle != IntPtr.Zero)
        {
            FreeLibrary(_dllHandle);
            _dllHandle = IntPtr.Zero;
            Debug.Log("DLLを解放しました");
        }
    }
}