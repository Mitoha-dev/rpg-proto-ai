using System.Runtime.InteropServices;

namespace RpgProto.Models
{
    // Rust側とメモリ配置を合わせる
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerStatus
    {
        public int hp;
        public int max_hp;
        public int attack;
        public int defense;
    }
}