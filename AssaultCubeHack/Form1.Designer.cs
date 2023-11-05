namespace AssaultCubeHack
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            title = new Label();
            hpHack = new Button();
            ammoHack = new Button();
            grenadeHack = new Button();
            processSelector = new ComboBox();
            playerDataBox = new GroupBox();
            positionLabel = new Label();
            angleLabel = new Label();
            grenadeLabel = new Label();
            ammoLabel = new Label();
            armourLabel = new Label();
            hpLabel = new Label();
            selectProcessText = new Label();
            exitButton = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            aimbot = new Button();
            aimbotLabel = new Label();
            hpActivated = new Label();
            ammoActivated = new Label();
            grenadeActivated = new Label();
            button1 = new Button();
            preference = new Label();
            playerDataBox.SuspendLayout();
            SuspendLayout();
            // 
            // title
            // 
            title.AutoSize = true;
            title.Font = new Font("Stencil", 16F, FontStyle.Regular, GraphicsUnit.Point);
            title.Location = new Point(226, 28);
            title.Name = "title";
            title.Size = new Size(326, 38);
            title.TabIndex = 0;
            title.Text = "AssaultCube Hack";
            // 
            // hpHack
            // 
            hpHack.Location = new Point(84, 310);
            hpHack.Name = "hpHack";
            hpHack.Size = new Size(145, 33);
            hpHack.TabIndex = 1;
            hpHack.Text = "Immortal";
            hpHack.UseVisualStyleBackColor = true;
            hpHack.Click += hpHack_Click;
            // 
            // ammoHack
            // 
            ammoHack.Location = new Point(84, 356);
            ammoHack.Name = "ammoHack";
            ammoHack.Size = new Size(145, 33);
            ammoHack.TabIndex = 2;
            ammoHack.Text = "Ammo Hack";
            ammoHack.UseVisualStyleBackColor = true;
            ammoHack.Click += ammoHack_Click;
            // 
            // grenadeHack
            // 
            grenadeHack.Location = new Point(84, 401);
            grenadeHack.Name = "grenadeHack";
            grenadeHack.Size = new Size(145, 33);
            grenadeHack.TabIndex = 3;
            grenadeHack.Text = "Grenade Hack";
            grenadeHack.UseVisualStyleBackColor = true;
            grenadeHack.Click += grenadeHack_Click;
            // 
            // processSelector
            // 
            processSelector.FormattingEnabled = true;
            processSelector.Location = new Point(80, 134);
            processSelector.Name = "processSelector";
            processSelector.Size = new Size(259, 33);
            processSelector.TabIndex = 4;
            processSelector.SelectedIndexChanged += processSelector_SelectedIndexChanged;
            processSelector.Click += processSelector_Click;
            // 
            // playerDataBox
            // 
            playerDataBox.Controls.Add(positionLabel);
            playerDataBox.Controls.Add(angleLabel);
            playerDataBox.Controls.Add(grenadeLabel);
            playerDataBox.Controls.Add(ammoLabel);
            playerDataBox.Controls.Add(armourLabel);
            playerDataBox.Controls.Add(hpLabel);
            playerDataBox.Location = new Point(387, 121);
            playerDataBox.Name = "playerDataBox";
            playerDataBox.Size = new Size(382, 256);
            playerDataBox.TabIndex = 5;
            playerDataBox.TabStop = false;
            playerDataBox.Text = "Player Data";
            // 
            // positionLabel
            // 
            positionLabel.AutoSize = true;
            positionLabel.Location = new Point(146, 122);
            positionLabel.Name = "positionLabel";
            positionLabel.Size = new Size(79, 25);
            positionLabel.TabIndex = 5;
            positionLabel.Text = "Position:";
            // 
            // angleLabel
            // 
            angleLabel.AutoSize = true;
            angleLabel.Location = new Point(146, 50);
            angleLabel.Name = "angleLabel";
            angleLabel.Size = new Size(62, 25);
            angleLabel.TabIndex = 4;
            angleLabel.Text = "Angle:";
            // 
            // grenadeLabel
            // 
            grenadeLabel.AutoSize = true;
            grenadeLabel.Location = new Point(14, 164);
            grenadeLabel.Name = "grenadeLabel";
            grenadeLabel.Size = new Size(87, 25);
            grenadeLabel.TabIndex = 3;
            grenadeLabel.Text = "Grenade: ";
            // 
            // ammoLabel
            // 
            ammoLabel.AutoSize = true;
            ammoLabel.Location = new Point(14, 124);
            ammoLabel.Name = "ammoLabel";
            ammoLabel.Size = new Size(71, 25);
            ammoLabel.TabIndex = 2;
            ammoLabel.Text = "Ammo:";
            // 
            // armourLabel
            // 
            armourLabel.AutoSize = true;
            armourLabel.Location = new Point(14, 85);
            armourLabel.Name = "armourLabel";
            armourLabel.Size = new Size(77, 25);
            armourLabel.TabIndex = 1;
            armourLabel.Text = "Armour:";
            // 
            // hpLabel
            // 
            hpLabel.AutoSize = true;
            hpLabel.Location = new Point(14, 50);
            hpLabel.Name = "hpLabel";
            hpLabel.Size = new Size(39, 25);
            hpLabel.TabIndex = 0;
            hpLabel.Text = "HP:";
            // 
            // selectProcessText
            // 
            selectProcessText.AutoSize = true;
            selectProcessText.Location = new Point(80, 106);
            selectProcessText.Name = "selectProcessText";
            selectProcessText.Size = new Size(140, 25);
            selectProcessText.TabIndex = 6;
            selectProcessText.Text = "Select ac_client -";
            // 
            // exitButton
            // 
            exitButton.Location = new Point(624, 398);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(145, 33);
            exitButton.TabIndex = 7;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += exitButton_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // aimbot
            // 
            aimbot.Location = new Point(84, 187);
            aimbot.Name = "aimbot";
            aimbot.Size = new Size(145, 33);
            aimbot.TabIndex = 8;
            aimbot.Text = "Aimbot";
            aimbot.UseVisualStyleBackColor = true;
            aimbot.Click += aimbot_Click;
            // 
            // aimbotLabel
            // 
            aimbotLabel.AutoSize = true;
            aimbotLabel.Location = new Point(249, 192);
            aimbotLabel.Name = "aimbotLabel";
            aimbotLabel.Size = new Size(105, 25);
            aimbotLabel.TabIndex = 9;
            aimbotLabel.Text = "Deactivated";
            // 
            // hpActivated
            // 
            hpActivated.AutoSize = true;
            hpActivated.Location = new Point(249, 315);
            hpActivated.Name = "hpActivated";
            hpActivated.Size = new Size(105, 25);
            hpActivated.TabIndex = 10;
            hpActivated.Text = "Deactivated";
            // 
            // ammoActivated
            // 
            ammoActivated.AutoSize = true;
            ammoActivated.Location = new Point(249, 361);
            ammoActivated.Name = "ammoActivated";
            ammoActivated.Size = new Size(105, 25);
            ammoActivated.TabIndex = 11;
            ammoActivated.Text = "Deactivated";
            // 
            // grenadeActivated
            // 
            grenadeActivated.AutoSize = true;
            grenadeActivated.Location = new Point(249, 406);
            grenadeActivated.Name = "grenadeActivated";
            grenadeActivated.Size = new Size(105, 25);
            grenadeActivated.TabIndex = 12;
            grenadeActivated.Text = "Deactivated";
            // 
            // button1
            // 
            button1.Location = new Point(84, 233);
            button1.Name = "button1";
            button1.Size = new Size(145, 35);
            button1.TabIndex = 13;
            button1.Text = "Change Aim";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // preference
            // 
            preference.AutoSize = true;
            preference.Location = new Point(249, 238);
            preference.Name = "preference";
            preference.Size = new Size(58, 25);
            preference.TabIndex = 15;
            preference.Text = "Angle";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 460);
            Controls.Add(preference);
            Controls.Add(button1);
            Controls.Add(grenadeActivated);
            Controls.Add(ammoActivated);
            Controls.Add(hpActivated);
            Controls.Add(aimbotLabel);
            Controls.Add(aimbot);
            Controls.Add(exitButton);
            Controls.Add(selectProcessText);
            Controls.Add(playerDataBox);
            Controls.Add(processSelector);
            Controls.Add(grenadeHack);
            Controls.Add(ammoHack);
            Controls.Add(hpHack);
            Controls.Add(title);
            Name = "Form1";
            Text = "AssaultCube_Hack";
            Load += Form1_Load;
            playerDataBox.ResumeLayout(false);
            playerDataBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label title;
        private Button hpHack;
        private Button ammoHack;
        private Button grenadeHack;
        private ComboBox processSelector;
        private GroupBox playerDataBox;
        private Label grenadeLabel;
        private Label ammoLabel;
        private Label armourLabel;
        private Label hpLabel;
        private Label positionLabel;
        private Label angleLabel;
        private Label selectProcessText;
        private Button exitButton;
        private System.Windows.Forms.Timer timer1;
        private Button aimbot;
        private Label aimbotLabel;
        private Label hpActivated;
        private Label ammoActivated;
        private Label grenadeActivated;
        private Button button1;
        private Label preference;
    }
}