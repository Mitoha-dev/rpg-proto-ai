// lib.rs

// 1. 他のファイル（モジュール）を読み込む宣言
pub mod models;
pub mod player;

// 使うものをインポート
use models::PlayerStatus;
use player::create_default_player;

// 2. C#に公開する窓口（エクスポート用関数）
// ここでは複雑なロジックは書かず、player.rs などの関数を呼び出すだけにする
#[unsafe(no_mangle)]
pub extern "C" fn get_player_status() -> PlayerStatus {
    create_default_player()
}

// lib.rs の下に追記

#[unsafe(no_mangle)]
pub extern "C" fn calculate_damage(status: PlayerStatus, damage: i32) -> PlayerStatus {
    player::apply_damage(status, damage)
}