﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;
namespace Winform
{
    public partial class Form_main : Form
    {
        ob_Set os = new ob_Set();
        ArrayList arr = new ArrayList();
        ArrayList btn_array = new ArrayList(); // 클릭한 버튼 색깔 바꾸기위해
        Panel main_pnl, menu_pnl, mdi_pnl, time_pnl, time_pnl2;
        Button logout, menu_btn;
        PictureBox logo, home;
        Label date, time;
        Timer mTimer;
        Form close;
        PrivateFontCollection ft1, ft2, ft3;
        Font font1, font2, font3, font4;
        Form_login fl;
        MYsql db = new MYsql();

        public Form_main()
        {
            InitializeComponent();
            Load += Form_main_Load;
            ClientSize = new Size(1500, 772);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Beethoven Management System ver 1.04";
            this.IsMdiContainer = true;
            Control_Init();
        }

        public Form_main(Form form)
        {
            fl = (Form_login)form;
            InitializeComponent();
            Load += Form_main_Load;
            ClientSize = new Size(1500, 772);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Beethoven Management System ver 1.04";
            this.IsMdiContainer = true;
            Control_Init();
        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            fonts();

            logo = new PictureBox();
            logo.Image = (Bitmap)Winform.Properties.Resources.ResourceManager.GetObject("goodee_logo3");
            logo.SizeMode = PictureBoxSizeMode.StretchImage;
            logo.Size = new Size(290, 100);
            logo.Location = new Point(0, 0);

            home = new PictureBox();
            home.Image = BackgroundImage = (Bitmap)Winform.Properties.Resources.ResourceManager.GetObject("home");
            home.SizeMode = PictureBoxSizeMode.StretchImage;
            home.Size = new Size(1460, 632);
            home.Location = new Point(0, 0);

            arr.Add(new ob_Pnl(this, "", "", 1500, 100, 0, 0));
            arr.Add(new ob_Pnl(this, "", "", 1460, 632, 20, 25));
            arr.Add(new ob_Pnl(this, "", "", 200, 200, 1096, 0)); // 시간 패널
            arr.Add(new ob_Pnl(this, "", "", 190, 90, 5, 5));
            arr.Add(new ob_Lbl(this, "", "", 200, 200, 5, 5)); // 년월
            arr.Add(new ob_Lbl(this, "", "", 200, 200, -3, 35)); // 시간
            arr.Add(new ob_Btn(this, "logout", "로그아웃", 204, 100, 1296, 0));
            arr.Add(new ob_Pnl(this, "", "", 1500, 677, 0, 95));

            menu_pnl = os.Pnl((ob_Pnl)arr[0]);
            mdi_pnl = os.Pnl((ob_Pnl)arr[1]);
            time_pnl = os.Pnl((ob_Pnl)arr[2]);
            time_pnl2 = os.Pnl((ob_Pnl)arr[3]);
            date = os.Lbl((ob_Lbl)arr[4]);
            time = os.Lbl((ob_Lbl)arr[5]);
            logout = os.Btn((ob_Btn)arr[6]);
            logout.Click += Logout_Click;
            logout.Cursor = Cursors.Hand;
            main_pnl = os.Pnl((ob_Pnl)arr[7]);

            Controls.Add(menu_pnl);
            menu_pnl.Controls.Add(logo);
            menu_pnl.Controls.Add(time_pnl);
            time_pnl.Controls.Add(time_pnl2);
            time_pnl2.Controls.Add(time);
            time_pnl2.Controls.Add(date);
            menu_pnl.Controls.Add(logout);
            Controls.Add(main_pnl);
            main_pnl.Controls.Add(mdi_pnl);
            mdi_pnl.Controls.Add(home);


            for (int i = 0; i < 4; i++)
            {
                menu_btn = new Button();
                menu_btn.Size = new Size(200, 100);
                menu_btn.Location = new Point((202 * i) + 290, 0);
                menu_btn.BackColor = Color.Black;
                menu_btn.ForeColor = Color.White;
                menu_btn.Font = font3;
                menu_btn.Name = string.Format("btn{0}", i + 1);
                menu_btn.TabStop = false; // 탭방지
                menu_btn.FlatStyle = FlatStyle.Popup; // 테두리 제거
                menu_pnl.Controls.Add(menu_btn);
                menu_btn.Click += Menu_btn_Click;
                menu_btn.Cursor = Cursors.Hand;
                menu_btn.Font = font3;
                btn_array.Add(menu_btn);

                if (menu_btn.Name == "btn1")
                {
                    menu_btn.Text = "회원 목록";
                }
                else if (menu_btn.Name == "btn2")
                {
                    menu_btn.Text = "회원 등록";
                }
                else if (menu_btn.Name == "btn3")
                {
                    menu_btn.Text = "기구 관리";
                }
                else if (menu_btn.Name == "btn4")
                {
                    menu_btn.Text = "통계 정보";
                }
            }
            option();
        }

        private void fonts()
        {
            ft1 = new PrivateFontCollection();
            ft2 = new PrivateFontCollection();
            ft3 = new PrivateFontCollection();
            ft1.AddFontFile("Font\\DS-DIGIB.ttf");
            ft2.AddFontFile("Font\\HANYGO230.ttf");
            ft3.AddFontFile("Font\\HANYGO240.ttf");
            font1 = new Font(ft1.Families[0], 38); // 시간
            font2 = new Font(ft1.Families[0], 20); // 년/월
            font3 = new Font(ft2.Families[0], 30); // 메뉴
            font4 = new Font(ft3.Families[0], 30); // 로그아웃
        }

        private void option()
        {
            menu_pnl.BackColor = Color.FromArgb(45, 35, 155);
            mdi_pnl.BackColor = Color.White;
            logout.FlatStyle = FlatStyle.Flat; // 테두리 제거
            logout.FlatAppearance.BorderSize = 0; // 테두리 제거
            logout.BackColor = Color.FromArgb(30, 30, 30);
            logout.ForeColor = Color.White;
            logout.TabStop = false; // 탭방지
            logout.FlatStyle = FlatStyle.Popup; // 테두리 제거
            logout.Font = font4;
            date.Font = font2;
            time.Font = font1;
            time_pnl.BackColor = Color.FromArgb(30, 30, 30);
            time_pnl2.BackColor = Color.FromArgb(40, 40, 40);
            main_pnl.BackColor = Color.FromArgb(45, 35, 155);
        }

        private void Control_Init()
        {
            mTimer = new Timer();
            mTimer.Tick += MTimer_Tick;
            mTimer.Start();
        }

        private void MTimer_Tick(object sender, EventArgs e)
        {
            date.Text = string.Format("{0:yyyy.MM.dd dddd}", DateTime.Now);
            date.ForeColor = Color.White;
            date.BackColor = Color.Transparent;
            time.Text = string.Format("{0:HH:mm:ss}", DateTime.Now);
            time.ForeColor = Color.White;
            time.BackColor = Color.Transparent;
        }

        private void Menu_btn_Click(object sender, EventArgs e)
        {
            if (close != null) close.Dispose();

            menu_btn = (Button)sender;
            for (int i = 0; i < btn_array.Count; i++)
            {
                Button clear = (Button)btn_array[i];
                if (clear.BackColor != Color.Black) clear.BackColor = Color.Black;
            }
            menu_btn.BackColor = Color.Red;

            switch (menu_btn.Name)
            {
                case "btn1":
                    Form_member fm = new Form_member();
                    close = new Form_member(this, mdi_pnl);
                    close.WindowState = FormWindowState.Maximized;
                    close.FormBorderStyle = FormBorderStyle.None;
                    close.MdiParent = this;
                    close.Dock = DockStyle.Fill;
                    home.Dispose();
                    mdi_pnl.Controls.Add(close);
                    close.Show();
                    break;
                case "btn2":
                    close = new Form_register();
                    close.WindowState = FormWindowState.Maximized;
                    close.FormBorderStyle = FormBorderStyle.None;
                    close.MdiParent = this;
                    close.Dock = DockStyle.Fill;
                    home.Dispose();
                    mdi_pnl.Controls.Add(close);
                    close.Show();
                    break;
                case "btn3":
                    close = new Form_infoAdd();
                    close.WindowState = FormWindowState.Maximized;
                    close.FormBorderStyle = FormBorderStyle.None;
                    close.MdiParent = this;
                    close.Dock = DockStyle.Fill;
                    home.Dispose();
                    mdi_pnl.Controls.Add(close);
                    close.Show();
                    break;
                case "btn4":
                    close = new Form_stats();
                    close.WindowState = FormWindowState.Maximized;
                    close.FormBorderStyle = FormBorderStyle.None;
                    close.MdiParent = this;
                    close.Dock = DockStyle.Fill;
                    home.Dispose();
                    mdi_pnl.Controls.Add(close);
                    close.Show();
                    break;
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("로그아웃 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                fl.Visible = true;
                this.Close();
            }
        }
    }
}