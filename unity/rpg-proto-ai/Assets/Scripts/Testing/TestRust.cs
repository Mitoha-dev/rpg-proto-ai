using UnityEngine;
using RpgProto.Infrastructure;
using RpgProto.Models;

public class TestRust : MonoBehaviour
{
    private delegate PlayerStatus GetPlayerStatusDelegate();
    // ▼ 新しいデリゲート（引数に構造体とintを取る）
    private delegate PlayerStatus CalculateDamageDelegate(PlayerStatus status, int damage);

    void Start()
    {
        RustLoader.Load();

        RpgProto.Infrastructure.RustLogger.Register();

        // 1. まず初期ステータスをもらう
        var getStatusFunc = RustLoader.GetFunction<GetPlayerStatusDelegate>("get_player_status");
        var calcDamageFunc = RustLoader.GetFunction<CalculateDamageDelegate>("calculate_damage");

        if (getStatusFunc != null && calcDamageFunc != null)
        {
            // 初期状態
            PlayerStatus currentStatus = getStatusFunc();
            Debug.Log($"【初期状態】HP: {currentStatus.hp}");

            // 2. Unityから「50ダメージ」をRustに送って計算してもらう
            int damageAmount = 50;
            Debug.Log($"{damageAmount} のダメージを送信中...");

            PlayerStatus newStatus = calcDamageFunc(currentStatus, damageAmount);

            // 3. 結果を表示
            Debug.Log($"【Rust計算後】新HP: {newStatus.hp}");
        }
    }

    void OnDestroy()
    {
        RustLoader.Unload();
    }
}