// models.rs

// C#と共通のデータ構造
#[repr(C)]
pub struct PlayerStatus {
    pub hp: i32,
    pub max_hp: i32,
    pub attack: i32,
    pub defense: i32,
}