﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Trivia_Client
{
    public partial class RoomMenu : Form
    {
        private int timeForQuestion = -1;
        private BackgroundWorker updateThread = new BackgroundWorker();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
 (
      int nLeftRect,     // x-coordinate of upper-left corner
      int nTopRect,      // y-coordinate of upper-left corner
      int nRightRect,    // x-coordinate of lower-right corner
      int nBottomRect,   // y-coordinate of lower-right corner
      int nWidthEllipse, // height of ellipse
      int nHeightEllipse // width of ellipse
 );
        public RoomMenu()
        {
            
            updateThread.WorkerSupportsCancellation = true;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }
            updateThread.DoWork += UpdateScreen;
            updateThread.RunWorkerAsync();

        }


        // updates the room on the screen
        private void UpdateScreen(object sender, EventArgs e)
        {
            while(true)
            {
                if (updateThread.CancellationPending)
                    break;
                RequestHandler.GetRoomState(this);
                Thread.Sleep(2000);

                
            }
            
        }
        // aborts the update thread
        private void StopUpdate()
        {
            updateThread.CancelAsync();
        }

        public void leaveRoomWorked()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () { leaveRoomWorked(); }));
            }
            else
            {
                StopUpdate();
                this.Hide();
                RoomListMenu roomListMenu = new RoomListMenu();
                roomListMenu.ShowDialog();
                this.Close();
            }
        }


        public void showErrorBox(String errorToShow)
        {
            Action action = () => this.errorTextBox.Text = errorToShow;
            errorTextBox.Invoke(action);

            action = () => this.errorTextBox.Visible = true;
            errorTextBox.Invoke(action);

        }
        private void errorTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (timeForQuestion != -1)
            {
                StopUpdate();
                RequestHandler.StartGame(this);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            StopUpdate();
            RequestHandler.CloseRoom(this);
        }
        public void Admin()
        {
            this.admin_box.Text = "Admin";
            this.admin_box.Visible = true;
            this.LeaveButton .Visible= false;
            this.CloseButton.Visible = true;
            this.StartButton.Visible = true;
            this.panel7.Visible = true;
        }
        public void Member()
        {
            this.admin_box.Visible = false;
            this.LeaveButton.Visible = true;
            this.CloseButton.Visible = false;
            this.StartButton.Visible = false;
        }
        public void addPlayers(List<string> list)
        {

            Action action = () => PlayerList.Items.Clear();
            PlayerList.Invoke(action);
            foreach (string roomName in list)
            {

                action = () => PlayerList.Items.Add(roomName);
                PlayerList.Invoke(action);
            }

        }
        public void SetParameters(string time)
        {
            this.timeForQuestion = int.Parse(time);
            Action action = () => this.answerTimeout.Text = "Time for each question: " + time;
            this.answerTimeout.Invoke(action);

            action = () => this.questionCount.Text = "Number of questions : 10";
            this.answerTimeout.Invoke(action);

        }

        private void LeaveButton_Click(object sender, EventArgs e)
        {

            StopUpdate();
            RequestHandler.LeaveRoom(this);

        }

        private void RoomMenu_Load(object sender, EventArgs e)
        {

        }

        private void PlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void admin_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void StartGameWorked()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () { StartGameWorked(); }));
            }
            else 
            {
                StopUpdate();
                this.Hide();
                GameMenu gameMenu = new GameMenu(10, timeForQuestion);
                gameMenu.ShowDialog();
                this.Close();
            }
        }
    }
}
