﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


using ClassLibrary1;

namespace Winform
{
    public partial class Form_stats : Form
    {
        Hashtable hashtable;
        Panel pn1, pn2, pn3, pn4, pn5, title;
        ListView lv;
        PrivateFontCollection ft1, ft2;
        Font font1, font2;
        Label title_lb;
        ComboBox cb1;
        Chart chart1, chart2;
        //ImageList il = new ImageList();
        private MYsql db = new MYsql();
        Commons cmm = new Commons();
        private string sql = "select  count(DATEDIFF(mEnd, now())) from member where sex = '남성' and DATEDIFF(mEnd, now())>0;";
        private string sql2 = "select  count(DATEDIFF(mEnd, now())) from member where sex = '여성' and DATEDIFF(mEnd, now())>0;";
        private string sql3 = "select DATE_FORMAT(mStart, '%Y년 %m월') as '일자',sum(cost) as '월 매출' from member where mstart Like '2018%' group by DATE_FORMAT(mStart, '%Y%m') order by 1;";
        private string cbyear = "select DATE_FORMAT(mStart, '%Y')as '년' from member group by DATE_FORMAT(mStart, '%Y');";

        public Form_stats()
        {
            InitializeComponent();
            Load += Form_stats_Load;
        }
        //1461,633
        private void Form_stats_Load(object sender, EventArgs e)
        {
            fonts();
            /* 전체 패널 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(1461, 633));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN4");
            pn4 = cmm.getPanel2(hashtable, this);

            /* 왼쪽 패널 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(318, 613));
            hashtable.Add("point", new Point(10, 10));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN5");
            pn5 = cmm.getPanel(hashtable, pn4);
            pn5.BorderStyle = BorderStyle.FixedSingle;

            /* 왼쪽 타이틀 패널 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(317, 53));
            hashtable.Add("point", new Point(0, 0));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "title");
            title = cmm.getPanel(hashtable, pn5);
            title.BorderStyle = BorderStyle.FixedSingle;

            /* 타이틀 라벨 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(300, 40));
            hashtable.Add("point", new Point(10, 5));
            hashtable.Add("color", Color.Black);
            hashtable.Add("name", "title_lb");
            hashtable.Add("text", "매출 정보 및 남여 구성 비율");
            title_lb = cmm.getLabel(hashtable, title);
            title_lb.ForeColor = Color.Black;
            title_lb.BackColor = Color.Transparent;
            title_lb.Font = font2;

            /* 원형 차트 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(220, 180));
            hashtable.Add("point", new Point(89, 423));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN1");
            pn1 = cmm.getPanel(hashtable, pn5);
            pn1.BorderStyle = BorderStyle.FixedSingle;

            /* 통계 차트 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(220, 356));
            hashtable.Add("point", new Point(89, 60));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN2");
            pn2 = cmm.getPanel(hashtable, pn5);

            /* 막대 차트 패널 */
            hashtable = new Hashtable();
            hashtable.Add("size", new Size(1114, 613));
            hashtable.Add("point", new Point(336, 10));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "BackgroundPN3");
            pn3 = cmm.getPanel(hashtable, pn4);
            pn3.BorderStyle = BorderStyle.FixedSingle;

            hashtable = new Hashtable();
            hashtable.Add("width", "70");
            hashtable.Add("point", new Point(10, 60));
            hashtable.Add("color", Color.White);
            hashtable.Add("name", "선택");
            hashtable.Add("text", "이름");
            hashtable.Add("value", "이름");
            hashtable.Add("key", "1");
            cb1 = cmm.getComboBox(hashtable, pn5);
            cb_year(cbyear);
            cb1.DropDownStyle = ComboBoxStyle.DropDownList;
            cb1.SelectedIndexChanged += Cb1_SelectedIndexChanged;
            cb1.Font = font1;

            chart1 = new Chart();
            ChartArea chartArea1 = new ChartArea();
            Series series1 = new Series();

            chartArea1.Name = "ChartArea1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = SeriesChartType.Doughnut;
            series1.Name = "Series1";

            chart1.Name = "chart1";
            chart1.Dock = DockStyle.Fill;
            chart1.Text = "chart1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Titles.Add("현 성별 구성 비율");
            chart1.Series.Add(series1);
            chart1.Series["Series1"].IsValueShownAsLabel = false;
            pn1.Controls.Add(chart1);
            value_search(sql);
            value_search2(sql2);
            //===============================================================
            chart2 = new Chart();
            ChartArea chartArea2 = new ChartArea();
            Legend legend2 = new Legend();
            Series series2 = new Series();

            chartArea2.Name = "ChartArea2";
            legend2.Name = "Legend2";
            series2.ChartArea = "ChartArea2";
            series2.ChartType = SeriesChartType.Column;
            series2.Legend = "Legend2";
            series2.Name = "매출액";

            chart2.Name = "chart2";
            chart2.Dock = DockStyle.Fill;
            chart2.Text = "chart2";
            chart2.BackColor = Color.White;
            chart2.ChartAreas.Add(chartArea2);

            chart2.Legends.Add(legend2);
            chart2.Series.Add(series2);

            chart2.Series["매출액"].IsValueShownAsLabel = false;
            month_cost(string.Format("select DATE_FORMAT(mStart, '%Y년 %m월') as '일자',sum(cost) as '월 매출' from member where mstart Like '{0}%' group by DATE_FORMAT(mStart, '%Y%m') order by 1;", cb1.Text));
            pn3.Controls.Add(chart2);

            hashtable = new Hashtable();
            hashtable.Add("color", Color.WhiteSmoke);
            hashtable.Add("name", "listView");
            lv = cmm.getListView(hashtable, pn2);
            month_cost2(sql3);
            lv.Columns.Add("기간", 110, HorizontalAlignment.Center);
            lv.Columns.Add("매출액", 106, HorizontalAlignment.Center);
            lv.Font = font1;
            lv.ColumnWidthChanging += Lv_ColumnWidthChanging;
            lv.ColumnClick += Lv_ColumnClick;
        }

        private void Lv_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e) // 리스트뷰 크기 고정
        {
            e.NewWidth = lv.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void Lv_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.lv.Sorting == SortOrder.Ascending || lv.Sorting == SortOrder.None)
            {
                this.lv.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                lv.Sorting = SortOrder.Descending;
            }
            else
            {
                this.lv.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                lv.Sorting = SortOrder.Ascending;

            }
            lv.Sort();
        }

        private void Cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            month_cost2(string.Format("select DATE_FORMAT(mStart, '%Y년 %m월') as '일자',sum(cost) as '월 매출' from member where mstart Like '{0}%' group by DATE_FORMAT(mStart, '%Y%m') order by 1;", cb1.Text));
            month_cost(string.Format("select DATE_FORMAT(mStart, '%Y년 %m월') as '일자',sum(cost) as '월 매출' from member where mstart Like '{0}%' group by DATE_FORMAT(mStart, '%Y%m') order by 1;", cb1.Text));
        }//콤보박스 클릭시 그래프와 리스트 초기화
        private void cb_year(string cbyear)
        {
            MySqlDataReader sdr = db.Reader(cbyear);
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                }
                cb1.Items.Add(string.Format("{0}", arr));
            }
            db.ReaderClose(sdr);
        }//콤보박스 년도 자동 생성

        private void month_cost2(string sql3)
        {
            MySqlDataReader sdr = db.Reader(sql3);
            lv.Items.Clear();
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                }
                lv.Items.Add(new ListViewItem(arr));
            }
            db.ReaderClose(sdr);
        }//월별 데이터 리스트뷰

        private void month_cost(string sql3)
        {
            MySqlDataReader sdr = db.Reader(sql3);
            chart2.Series["매출액"].Points.Clear();
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                }
                chart2.Series["매출액"].Points.AddXY(arr[0], arr[1]);
            }
            db.ReaderClose(sdr);
        }// 월별데이터 그래프
        private void value_search(string sql)
        {
            MySqlDataReader sdr = db.Reader(sql);
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                    chart1.Series["Series1"].Points.AddXY(string.Format("남성 {0}", arr[i]), arr[i]);
                    chart1.Series["Series1"].Points[0].Color = Color.FromArgb(45, 35, 135);
                    chart1.Series["Series1"].Points[0].LabelForeColor = Color.White;
                }
            }
            db.ReaderClose(sdr);
        }//남자

        private void value_search2(string sql2)
        {
            MySqlDataReader sdr = db.Reader(sql2);
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                    chart1.Series["Series1"].Points.AddXY(string.Format("여성 {0}", arr[i]), arr[i]);
                    chart1.Series["Series1"].Points[1].Color = Color.OrangeRed;
                    chart1.Series["Series1"].Points[1].LabelForeColor = Color.White;
                }
            }
            db.ReaderClose(sdr);
        }//여자

        private void fonts()
        {
            ft1 = new PrivateFontCollection();
            ft2 = new PrivateFontCollection();

            ft1.AddFontFile("Font\\HANYGO230.ttf");
            ft2.AddFontFile("Font\\HANYGO250.ttf");

            font1 = new Font(ft1.Families[0], 12);
            font2 = new Font(ft2.Families[0], 18);
        }

        internal class ListViewItemComparer : IComparer
        {
            private int column;
            private string sort = "asc";

            public ListViewItemComparer()
            {
                column = 0;
            }
            public ListViewItemComparer(int column, string sort)
            {
                this.column = column;
                this.sort = sort;
            }
            public int Compare(object x, object y)
            {
                int chk = 1;
                try
                {
                    if (sort == "asc")
                        chk = 1;
                    else
                        chk = -1;
                    if (Convert.ToInt32(((ListViewItem)x).SubItems[column].Text) > Convert.ToInt32(((ListViewItem)y).SubItems[column].Text))
                        return chk;
                    else
                        return -chk;
                }
                catch (Exception)
                {
                    if (sort == "asc")
                        return String.Compare(((ListViewItem)x).SubItems[column].Text, ((ListViewItem)y).SubItems[column].Text);
                    else
                        return String.Compare(((ListViewItem)y).SubItems[column].Text, ((ListViewItem)x).SubItems[column].Text);
                }
            }
        }
    }
}
