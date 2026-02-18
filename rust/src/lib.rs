// rust/src/lib.rs

#[unsafe(no_mangle)] // または #[no_mangle]
pub extern "C" fn get_initial_hp() -> i32 {  // ←ここが重要！
    9999
}