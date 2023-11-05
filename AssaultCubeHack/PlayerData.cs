using ProcessMemoryReaderLib;
using System.Diagnostics;

namespace AssaultCubeHack
{
    internal class PlayerData
    {
        int base_address; // "ac_client.exe"+0058A690

        // If a player's memory address starts at 0x00000000 for example, 
        // HP will be stored in 0x000000EC, Armour at 0x000000F0, and so on.
        // All of these offsets has been discovered manually using Cheat Engine.

        // I have not had the opportunity to check if this works on another computer or not,
        // since I am not sure if the same pointer applies to separate devices.
        // For example, the player data for AssaultCube is stored in 0x0058A690 offset
        // starting from the AssaultCube main process, but I am not sure if the same thing applies
        // for everyone else's computer.

        int hp_offset = 0xEC;
        int armour_offset = 0xF0;
        int ammo_offset = 0x140;
        int grenade_offset = 0x144;
        int x_pos_offset = 0x4;
        int y_pos_offset = 0xC;
        int z_pos_offset = 0x8;
        int x_angle_offset = 0x34;
        int y_angle_offset = 0x38;

        public int hp;
        public int armour;
        public int ammo;
        public int grenade;
        public float x_pos;
        public float y_pos;
        public float z_pos;
        public float x_angle;
        public float y_angle;

        public PlayerData(int address)
        {
            base_address = address;

            hp = 0;
            armour = 0;
            ammo = 0;
            grenade = 0;
            x_pos = 0;
            y_pos = 0;
            z_pos = 0;
            x_angle = 0;
            y_angle = 0;
        }

        // Setting up player data, using the saved offsets
        public void SetPlayerData(ProcessMemoryReader mem)
        {
            hp = mem.ReadInt(base_address + hp_offset);
            armour = mem.ReadInt(base_address + armour_offset);
            ammo = mem.ReadInt(base_address + ammo_offset);
            grenade = mem.ReadInt(base_address + grenade_offset);
            x_pos = mem.ReadFloat(base_address + x_pos_offset);
            y_pos = mem.ReadFloat(base_address + y_pos_offset);
            z_pos = mem.ReadFloat(base_address + z_pos_offset);
            x_angle = mem.ReadFloat(base_address + x_angle_offset);
            y_angle = mem.ReadFloat(base_address + y_angle_offset);
        }

        // Below are functions which does the hacking process itself
        internal void hackHP(ProcessMemoryReader memory)
        {
            memory.WriteInt(base_address + hp_offset, 10000);
        }

        internal void hackAmmo(ProcessMemoryReader memory)
        {
            memory.WriteInt(base_address + ammo_offset, 10000);
        }

        internal void hackGrenade(ProcessMemoryReader memory)
        {
            memory.WriteInt(base_address + grenade_offset, 10);
        }

        // Aimbots require the player to face the very exact direction, and this is the function which does it.
        internal void activateAimbot(ProcessMemoryReader memory, double x_angle, double y_angle)
        {
            memory.WriteFloat(base_address + x_angle_offset, (float) x_angle);
            memory.WriteFloat(base_address + y_angle_offset, (float) y_angle);
        }

        internal double getAimErr(ProcessMemoryReader memory, double temp_x_angle, double temp_y_angle)
        {
            return Math.Pow(temp_x_angle - x_angle, 2) + Math.Pow(temp_y_angle - y_angle, 2);
        }
    }
}












