// player.rs
use crate::models::PlayerStatus;

// プレイヤーの初期ステータスを生成する関数（内部ロジック）
pub fn create_default_player() -> PlayerStatus {
    PlayerStatus {
        hp: 150, // ちょっと増やしてみました
        max_hp: 300,
        attack: 70,
        defense: 45,
    }
}

// player.rs の下に追記

pub fn apply_damage(mut status: PlayerStatus, damage: i32) -> PlayerStatus {
    // ダメージを引く（0以下にはならないようにする）
    status.hp = (status.hp - damage).max(0);
    status
}