using System.Diagnostics;
using ProcessMemoryReaderLib;

namespace AssaultCubeHack
{
    public partial class Form1 : Form
    {
        Process[] myProcesses; // List of processes
        ProcessMemoryReader memory = new ProcessMemoryReader();

        Boolean attached = false;
        Boolean hpHacked = false;
        Boolean ammoHacked = false;
        Boolean grenadeHacked = false;
        Boolean aimbotActivated = false;
        int aimPreference = 0; // aimPreference saves which kind of algorithm the player wants to use

        PlayerData mainPlayer;
        PlayerData[] enemyPlayer = new PlayerData[30];
        Process attachedProcess;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Exit button
        // Might not seem like much, but was the first functionality I created with an actual project
        // that has a real window
        private void exitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Abort;
                Application.Exit();
            }
        }

        // Behaviour for actions when ac_client process is selected
        private void processSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (processSelector.SelectedIndex != -1)
                {
                    String selectedProcess = processSelector.SelectedItem.ToString();

                    String pidString = new String(selectedProcess.Where(Char.IsDigit).ToArray());
                    int pid = Int32.Parse(pidString);
                    attachedProcess = Process.GetProcessById(pid);

                    memory.ReadProcess = attachedProcess;   // Choose which process to open
                    memory.OpenProcess();                   // Opens the process

                    int player_ptr = attachedProcess.MainModule.BaseAddress.ToInt32() + 0x0058A690;
                    int player_address = memory.ReadInt(player_ptr);
                    mainPlayer = new PlayerData(player_address);
                    attached = true;
                }
            }
            catch (Exception ex)
            {
                attached = false;
                MessageBox.Show("Process failed to open: " + ex.Message);
            }

        }

        // Shows list of processes when clicked
        // Gets rid of all other processes other than ac_client using a string match
        private void processSelector_Click(object sender, EventArgs e)
        {
            processSelector.Items.Clear(); // Resets process list from previous load

            myProcesses = Process.GetProcesses();
            for (int i = 0; i < myProcesses.Length; i++)
            {
                if (myProcesses[i].ProcessName == "ac_client")
                {
                    String processName = myProcesses[i].ProcessName + " (" + myProcesses[i].Id + ")";
                    processSelector.Items.Add(processName);
                }
            }
        }

        // Actions to do in every single tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (attached)
                {
                    if (hpHacked)
                    {
                        mainPlayer.hackHP(memory);
                    }
                    if (ammoHacked)
                    {
                        mainPlayer.hackAmmo(memory);
                    }
                    if (grenadeHacked)
                    {
                        mainPlayer.hackGrenade(memory);
                    }

                    if (checkRightClicked() && aimbotActivated)
                    {
                        getEnemyState(memory); // get information about enemies

                        if (aimPreference == 0) // only angle
                        {
                            // 1. Make the minimum angle difference an absurdly large value so it will be changed
                            // 2. Calculate the angle difference between the player's current angle
                            //    and the angle between the enemy and the player.
                            // 3. Saves the angle between the enemy and the player for the minimum angle difference
                            // 4. Points at the closest enemy when mouse is right-clicked
                            //
                            // Ignores dead enemies

                            double minAngle = 10000;
                            double angle;
                            double min_x_angle = 0;
                            double min_y_angle = 0;

                            for (int i = 0; i < 30; i++)
                            {
                                double x_angle = getXDegree(mainPlayer, enemyPlayer[i]);
                                double y_angle = getYDegree(mainPlayer, enemyPlayer[i]);
                                angle = mainPlayer.getAimErr(memory, x_angle, y_angle);

                                if (minAngle > angle && checkAlive(enemyPlayer[i]))
                                {
                                    minAngle = angle;
                                    min_x_angle = x_angle;
                                    min_y_angle = y_angle;
                                }
                            }
                            mainPlayer.activateAimbot(memory, min_x_angle, min_y_angle);
                        }
                        else // angle + distance
                        {
                            // 1. Gather all distances, aim differences, angle differences (both x and y)
                            // 2. If enemy is dead, initialise array values with absurdly large value
                            //    so it will never get chosen
                            // 3. Score for shortest distance and closest angle is calculated.
                            // e.g.) Closest enemy has distance score 1, second closest has distance score 2
                            //       Same mechanism applies for aim difference
                            // 4. Enemy with lowest score is chosen
                            //
                            // If a map is initialised with less than 32 players, the enemies are not 'non-existent';
                            // they are stored somewhere in the map in the same location. There was a bug where the aimbot
                            // pointed at a random wall at the exact same angle all the time, and I came to the conclusion
                            // the leftover enemies were stored somewhere outside the map.
                            // I manually calculate the number of enemies by checking how many enemies have the exact same
                            // coordinates, where I have implemented this in the getBestAim function.
                            double[] distances = new double[30];
                            double[] aim_diffs = new double[30];
                            double[] x_angles = new double[30];
                            double[] y_angles = new double[30];

                            for (int i = 0; i < 30; i++)
                            {
                                double distance = getDistance(mainPlayer, enemyPlayer[i]);
                                double x_angle = getXDegree(mainPlayer, enemyPlayer[i]);
                                double y_angle = getYDegree(mainPlayer, enemyPlayer[i]);
                                double aim_diff = mainPlayer.getAimErr(memory, x_angle, y_angle);

                                
                                x_angles[i] = x_angle;
                                y_angles[i] = y_angle;
                                
                                if (checkAlive(enemyPlayer[i]))
                                {
                                    distances[i] = distance;
                                    aim_diffs[i] = aim_diff;
                                }
                                else
                                {
                                    distances[i] = 10000;
                                    aim_diffs[i] = 10000;
                                }
                            }

                            int minIndex = getBestAim(distances, aim_diffs);
                            mainPlayer.activateAimbot(memory, x_angles[minIndex], y_angles[minIndex]);
                        }
                    }

                    mainPlayer.SetPlayerData(memory);
                    hpLabel.Text = "HP: " + mainPlayer.hp;
                    armourLabel.Text = "Armour: " + mainPlayer.armour;
                    ammoLabel.Text = "Ammo: " + mainPlayer.ammo;
                    grenadeLabel.Text = "Grenade: " + mainPlayer.grenade;
                    angleLabel.Text = "Angle: " + mainPlayer.x_angle.ToString("#.##") + ", " + mainPlayer.y_angle.ToString("#.##");
                    positionLabel.Text = "Position: " + mainPlayer.x_pos.ToString("#.##") + ", " + mainPlayer.y_pos.ToString("#.##") + ", " + mainPlayer.z_pos.ToString("#.##");
                }
            }
            catch
            {

            }
        }

        // The working mechanism for this has been explained above
        private int getBestAim(double[] distances, double[] aim_diffs)
        {
            int playerNum = 0;
            for (int i = 0; i < 29; i++)
            {
                if (distances[i] == distances[i + 1] && aim_diffs[i] == aim_diffs[i + 1])
                {
                    playerNum = i;
                    break;
                }
            }

            if (playerNum == 0)
            {
                return 0;
            }

            double[] newDistances = new double[playerNum];
            Array.Copy(distances, 0, newDistances, 0, playerNum);
            double[] newAimdiffs = new double[playerNum];
            Array.Copy(aim_diffs, 0, newAimdiffs, 0, playerNum);

            // Sorting and giving out 'scores' for distances and angles
            int[] distancesResult = newDistances.Select((x, i) => (x, i))
                .OrderBy(t => t.x)
                .Select(t => t.i)
                .ToArray();
            int[] aimdiffsResult = newAimdiffs.Select((x, i) => (x, i))
                .OrderBy(t => t.x)
                .Select(t => t.i)
                .ToArray();

            // Calculating the best possible score and the index
            int minSum = 100;
            int minIndex = 0;
            for (int i = 0; i < playerNum; i++)
            {
                int sum = distancesResult[i] + aimdiffsResult[i];
                if (sum < minSum)
                {
                    minSum = sum;
                    minIndex = i;
                }
            }
            return minIndex;
        }

        private double getDistance(PlayerData mainPlayer, PlayerData enemyPlayer)
        {
            // used Pythagoras Theorem
            double dist = Math.Sqrt(Math.Pow(mainPlayer.x_pos - enemyPlayer.x_pos, 2)
                + Math.Pow(mainPlayer.y_pos - enemyPlayer.y_pos, 2)
                + Math.Pow(mainPlayer.z_pos - enemyPlayer.z_pos, 2));
            return dist;
        }

        private double getXDegree(PlayerData mainPlayer, PlayerData enemyPlayer)
        {
            double x_dist = mainPlayer.x_pos - enemyPlayer.x_pos;
            double z_dist = mainPlayer.z_pos - enemyPlayer.z_pos;
            double correction = 270;

            if (x_dist < 0) correction = 90;

            return correction + Math.Atan(z_dist / x_dist) * 180 / Math.PI;
        }

        private double getYDegree(PlayerData mainPlayer, PlayerData enemyPlayer)
        {
            double xz_dist = Math.Sqrt(Math.Pow(mainPlayer.x_pos - enemyPlayer.x_pos, 2) + Math.Pow(mainPlayer.z_pos - enemyPlayer.z_pos, 2));
            double y_dist = mainPlayer.y_pos - enemyPlayer.y_pos;

            if (y_dist >= 0)
            {
                return -1 * Math.Abs(Math.Atan(y_dist / xz_dist) * 180 / Math.PI);
            }
            else
            {
                return Math.Abs(Math.Atan(y_dist / xz_dist) * 180 / Math.PI);
            }
        }

        private bool checkAlive(PlayerData enemyPlayer)
        {
            if (enemyPlayer.hp > 0 && enemyPlayer.hp <= 100) return true;
            else return false;
        }

        // Saves value of enemy using the saved addresses
        // The offsets are exactly the same for Player and Enemy
        // so we can reuse the PlayerData class to store the values of
        // both Player and Enemy
        private void getEnemyState(ProcessMemoryReader memory)
        {
            int enemy_ptr = attachedProcess.MainModule.BaseAddress.ToInt32() + 0x005B5B20;

            for (int i = 0; i < 30; i++)
            {
                int[] offsetArray = { i * 4, 0 };
                int enemy_address = memory.ReadMultiLevelPointer(enemy_ptr, 4, offsetArray);
                enemyPlayer[i] = new PlayerData(enemy_address);
                enemyPlayer[i].SetPlayerData(memory);
            }
        }

        private void hpHack_Click(object sender, EventArgs e)
        {
            if (hpHacked)
            {
                hpHacked = false;
                hpActivated.Text = "Deactivated";
            }
            else
            {
                hpHacked = true;
                hpActivated.Text = "Activated";
            }
        }

        private void ammoHack_Click(object sender, EventArgs e)
        {
            if (ammoHacked)
            {
                ammoHacked = false;
                ammoActivated.Text = "Deactivated";
            }
            else
            {
                ammoHacked = true;
                ammoActivated.Text = "Activated";
            }
        }

        private void grenadeHack_Click(object sender, EventArgs e)
        {
            if (grenadeHacked)
            {
                grenadeHacked = false;
                grenadeActivated.Text = "Deactivated";
            }
            else
            {
                grenadeHacked = true;
                grenadeActivated.Text = "Activated";
            }
        }

        private void aimbot_Click(object sender, EventArgs e)
        {
            if (aimbotActivated)
            {
                aimbotActivated = false;
                aimbotLabel.Text = "Deactivated";
            }
            else
            {
                aimbotActivated = true;
                aimbotLabel.Text = "Activated";
            }
        }

        private bool checkRightClicked()
        {
            int hotkey = ProcessMemoryReaderApi.GetKeyState(0x02);
            if ((hotkey & 0x8000) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (aimPreference == 0)
            {
                aimPreference = 1;
                preference.Text = "Dist + Angle";
            }
            else
            {
                aimPreference = 0;
                preference.Text = "Angle";
            }
        }
    }
}
