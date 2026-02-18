// rust/src/lib.rs

use std::ffi::{CStr, CString};
use std::os::raw::c_char;

pub mod models;
pub mod player;

use models::PlayerStatus;

// ▼ 1. C#側の関数を受け取るための型定義
// 「文字列(charポインタ)を受け取って、何も返さない関数」という意味
type LogCallback = extern "C" fn(*const c_char);

// ▼ 2. コールバック関数を保存しておく場所（静的変数）
static mut UNITY_LOG_CALLBACK: Option<LogCallback> = None;

// ▼ 3. Unityから関数を受け取る登録用関数
#[unsafe(no_mangle)]
pub extern "C" fn register_logger(callback: LogCallback) {
    unsafe {
        UNITY_LOG_CALLBACK = Some(callback);
    }
}

// ▼ 4. Rustの中で使いやすくするためのラッパー関数
// これを player.rs などから呼ぶ
pub fn unity_log(message: &str) {
    let c_str = CString::new(message).unwrap();
    unsafe {
        if let Some(callback) = UNITY_LOG_CALLBACK {
            callback(c_str.as_ptr());
        }
    }
}

// --- 以下、既存の関数 ---

#[unsafe(no_mangle)]
pub extern "C" fn get_player_status() -> PlayerStatus {
    // ログのテスト：呼び出されたことをUnityに通知！
    unity_log("Rust: get_player_status が呼ばれました！");
    player::create_default_player()
}

#[unsafe(no_mangle)]
pub extern "C" fn calculate_damage(status: PlayerStatus, damage: i32) -> PlayerStatus {
    // ダメージ計算のログ
    unity_log(&format!("Rust: {} ダメージを受けました！", damage));
    player::apply_damage(status, damage)
}