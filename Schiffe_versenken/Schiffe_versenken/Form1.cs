using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schiffe_versenken
{
    public partial class Form1 : Form
    {
        private int[][] Spieler = new int[10][], Versuche = new int[10][], Untergehen = new int[7][];
        private PictureBox[] Schiffsteile;
        private int SchiffsteileC = 0;
        private Control activeControl;
        private Point previousPosition;
        private TableLayoutPanel[] Schiffe, SchiffeHo;
        private Gegner KI;
        private static Color[,] bgColors;
        private int WinC, winC;

        private static Image bug_unten = Properties.Resources.Bug_unten;
        private static Image bug_oben = Properties.Resources.Bug_oben;
        private static Image bug_links = Properties.Resources.Bug_links;
        private static Image bug_rechts = Properties.Resources.Bug_rechts;
        private static Image mitte = Properties.Resources.mitte;
        private static Image mitte_horizontal = Properties.Resources.mitte_horizontal;
        private static Image dreier_x = Properties.Resources.dreier_x;
        private static Image dreier_x2 = Properties.Resources.dreier_x2;
        private static Image wasser = Properties.Resources.Wasser;
        private static Image treffer = Properties.Resources.Treffer;

        public Form1()
        {
            bgColors = new Color[10, 10] {
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control},
    { SystemColors.Control, SystemColors.Control , SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control}
};
            InitializeComponent();
            Untergehen[0] = new int[2];
            Untergehen[1] = new int[3];
            Untergehen[2] = new int[3];
            Untergehen[3] = new int[4];
            Untergehen[4] = new int[5];
            Untergehen[5] = new int[4];
            Untergehen[6] = new int[6];
            for (int f = 0; f < Untergehen.Length; f++)
            {
                for (int g = 0; g < Untergehen[f].Length; g++)
                {
                    Untergehen[f][g] = -1;
                }
            }

            WinC = winC = 0;
            Schiffsteile = new PictureBox[42];
            for (int f = 0; f < Schiffsteile.Length; f++)
            {
                Schiffsteile[f] = new PictureBox();
                Schiffsteile[f].Size = new Size(20, 20);
                Schiffsteile[f].Name = "btn";
                Schiffsteile[f].MouseUp += Schiffe_MouseUp;
                Schiffsteile[f].MouseDown += Schiffe_MouseDown;
                Schiffsteile[f].MouseMove += Schiffe_MouseMove;
            }

            Schiffe = new TableLayoutPanel[6];
            SchiffeHo = new TableLayoutPanel[6];


            Schiffe[0] = new TableLayoutPanel();
            Schiffe[0].ColumnCount = 3;
            Schiffe[0].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[0].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[0].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[0].RowCount = 1;
            Schiffe[0].Size = new System.Drawing.Size(65, 25);
            Schiffe[0].Location = new System.Drawing.Point(0, 40);
            Schiffe[0].Name = "Zweier";
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            Schiffe[0].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 0);
            Schiffsteile[SchiffsteileC].Image = bug_links;
            Schiffe[0].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);

            SchiffeHo[0] = new TableLayoutPanel();
            SchiffeHo[0].ColumnCount = 1;
            SchiffeHo[0].RowCount = 3;
            SchiffeHo[0].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[0].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[0].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[0].Size = new System.Drawing.Size(25, 65);
            SchiffeHo[0].Visible = false;
            SchiffeHo[0].Name = "Zweier";
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            SchiffeHo[0].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 2);
            Schiffsteile[SchiffsteileC].Image = bug_oben;
            SchiffeHo[0].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);

            Schiffe[1] = new TableLayoutPanel();
            Schiffe[1].ColumnCount = 5;
            Schiffe[1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[1].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[1].RowCount = 1;
            Schiffe[1].Size = new System.Drawing.Size(105, 25);
            Schiffe[1].Location = new System.Drawing.Point(0, 80);
            Schiffe[1].Name = "Dreier";
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[1].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 0);
            Schiffsteile[SchiffsteileC].Image = bug_links;
            Schiffe[1].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            Schiffe[1].Controls.Add(Schiffsteile[SchiffsteileC++], 4, 0);

            SchiffeHo[1] = new TableLayoutPanel();
            SchiffeHo[1].ColumnCount = 1;
            SchiffeHo[1].RowCount = 5;
            SchiffeHo[1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[1].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[1].Size = new System.Drawing.Size(25, 105);
            SchiffeHo[1].Visible = false;
            SchiffeHo[1].Name = "Dreier";
            Schiffsteile[SchiffsteileC].Image = bug_oben;
            SchiffeHo[1].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[1].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 2);
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            SchiffeHo[1].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 4);

            Schiffe[2] = new TableLayoutPanel();
            Schiffe[2].ColumnCount = 5;
            Schiffe[2].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[2].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[2].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[2].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[2].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[2].RowCount = 1;
            Schiffe[2].Size = new System.Drawing.Size(105, 25);
            Schiffe[2].Location = new System.Drawing.Point(0, 120);
            Schiffe[2].Name = "Dreier2";
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[2].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 0);
            Schiffsteile[SchiffsteileC].Image = bug_links;
            Schiffe[2].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            Schiffe[2].Controls.Add(Schiffsteile[SchiffsteileC++], 4, 0);

            SchiffeHo[2] = new TableLayoutPanel();
            SchiffeHo[2].ColumnCount = 1;
            SchiffeHo[2].RowCount = 5;
            SchiffeHo[2].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[2].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[2].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[2].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[2].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[2].Size = new System.Drawing.Size(25, 105);
            SchiffeHo[2].Visible = false;
            SchiffeHo[2].Name = "Dreier2";
            Schiffsteile[SchiffsteileC].Image = bug_oben;
            SchiffeHo[2].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[2].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 2);
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            SchiffeHo[2].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 4);

            Schiffe[3] = new TableLayoutPanel();
            Schiffe[3].ColumnCount = 7;
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[3].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[3].RowCount = 1;
            Schiffe[3].Size = new System.Drawing.Size(145, 25);
            Schiffe[3].Location = new System.Drawing.Point(0, 160);
            Schiffe[3].Name = "Vierer";
            Schiffsteile[SchiffsteileC].Image = bug_links;
            Schiffe[3].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[3].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 0);
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[3].Controls.Add(Schiffsteile[SchiffsteileC++], 4, 0);
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            Schiffe[3].Controls.Add(Schiffsteile[SchiffsteileC++], 6, 0);

            SchiffeHo[3] = new TableLayoutPanel();
            SchiffeHo[3].ColumnCount = 1;
            SchiffeHo[3].RowCount = 7;
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[3].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[3].Size = new System.Drawing.Size(25, 145);
            SchiffeHo[3].Visible = false;
            SchiffeHo[3].Name = "Vierer";
            Schiffsteile[SchiffsteileC].Image = bug_oben;
            SchiffeHo[3].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[3].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 2);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[3].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 4);
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            SchiffeHo[3].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 6);

            Schiffe[4] = new TableLayoutPanel();
            Schiffe[4].ColumnCount = 9;
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15));
            Schiffe[4].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[4].RowCount = 1;
            Schiffe[4].Size = new System.Drawing.Size(180, 25);
            Schiffe[4].Location = new System.Drawing.Point(0, 200);
            Schiffe[4].Name = "Fuenfer";
            Schiffsteile[SchiffsteileC].Image = bug_links;
            Schiffe[4].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[4].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 0);
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[4].Controls.Add(Schiffsteile[SchiffsteileC++], 4, 0);
            Schiffsteile[SchiffsteileC].Image = mitte_horizontal;
            Schiffe[4].Controls.Add(Schiffsteile[SchiffsteileC++], 6, 0);
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            Schiffe[4].Controls.Add(Schiffsteile[SchiffsteileC++], 8, 0);

            SchiffeHo[4] = new TableLayoutPanel();
            SchiffeHo[4].ColumnCount = 1;
            SchiffeHo[4].RowCount = 9;
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15));
            SchiffeHo[4].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[4].Size = new System.Drawing.Size(25, 180);
            SchiffeHo[4].Visible = false;
            SchiffeHo[4].Name = "Fuenfer";
            Schiffsteile[SchiffsteileC].Image = bug_oben;
            SchiffeHo[4].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[4].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 2);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[4].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 4);
            Schiffsteile[SchiffsteileC].Image = mitte;
            SchiffeHo[4].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 6);
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            SchiffeHo[4].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 8);

            Schiffe[5] = new TableLayoutPanel();
            Schiffe[5].ColumnCount = 5;
            Schiffe[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[5].RowCount = 3;
            Schiffe[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            Schiffe[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            Schiffe[5].Size = new System.Drawing.Size(105, 60);
            Schiffe[5].Location = new System.Drawing.Point(0, 240);
            Schiffe[5].Name = "DreierX";
            Schiffsteile[SchiffsteileC].Image = bug_links;
            Schiffe[5].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = dreier_x;
            Schiffe[5].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 0);
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            Schiffe[5].Controls.Add(Schiffsteile[SchiffsteileC++], 5, 0);
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            Schiffe[5].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 2);

            SchiffeHo[5] = new TableLayoutPanel();
            SchiffeHo[5].ColumnCount = 3;
            SchiffeHo[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[5].ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[5].RowCount = 5;
            SchiffeHo[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13));
            SchiffeHo[5].RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 20));
            SchiffeHo[5].Size = new System.Drawing.Size(60, 105);
            SchiffeHo[5].Visible = false;
            SchiffeHo[5].Name = "DreierX";
            Schiffsteile[SchiffsteileC].Image = bug_oben;
            SchiffeHo[5].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 0);
            Schiffsteile[SchiffsteileC].Image = dreier_x2;
            SchiffeHo[5].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 2);
            Schiffsteile[SchiffsteileC].Image = bug_unten;
            SchiffeHo[5].Controls.Add(Schiffsteile[SchiffsteileC++], 0, 5);
            Schiffsteile[SchiffsteileC].Image = bug_rechts;
            SchiffeHo[5].Controls.Add(Schiffsteile[SchiffsteileC++], 2, 2);

            for (int f = 0; f < Schiffe.Length; f++)
            {
                Schiffe[f].MouseUp += Schiffe_MouseUp;
                Schiffe[f].MouseDown += Schiffe_MouseDown;
                Schiffe[f].MouseMove += Schiffe_MouseMove;
                pnSpielbereit.Controls.Add(Schiffe[f]);
                SchiffeHo[f].MouseUp += Schiffe_MouseUp;
                SchiffeHo[f].MouseDown += Schiffe_MouseDown;
                SchiffeHo[f].MouseMove += Schiffe_MouseMove;
                pnSpielbereit.Controls.Add(SchiffeHo[f]);
            }



        }

        private void Schiffe_MouseDown(object sender, MouseEventArgs e)
        {
            //sender = Tabel(sender);
            activeControl = sender as Control;
            previousPosition = e.Location;
            Cursor = Cursors.Hand;
        }
        private void Schiffe_MouseMove(object sender, MouseEventArgs e)
        {

            Randuerberprufung(sender);

            if (activeControl == null || activeControl != sender)
                return;
            else
            {
                sender = Tabel(sender);
                Point location = (sender as Control).Location;
                location.Offset(e.Location.X - previousPosition.X, e.Location.Y - previousPosition.Y);
                (sender as Control).Location = location;
                // activeControl.Location = location;
            }
        }
        private void Schiffe_MouseUp(object sender, MouseEventArgs e)
        {
            Randuerberprufung(sender);
            sender = Tabel(sender);

            try
            {
                TableLayoutPanel x = sender as TableLayoutPanel;
                if (x.Location.X % 40 > 19 && x.Location.Y % 40 > 19) x.Location = new Point(x.Location.X + (40 - (x.Location.X % 40)), x.Location.Y + (40 - (x.Location.Y % 40)));
                else if (x.Location.X % 40 > 19 && x.Location.Y % 40 < 20) x.Location = new Point(x.Location.X + (40 - (x.Location.X % 40)), x.Location.Y - (x.Location.Y % 40));
                else if (x.Location.X % 40 < 21 && x.Location.Y % 40 > 19) x.Location = new Point(x.Location.X - (x.Location.X % 40), x.Location.Y + (40 - (x.Location.Y % 40)));
                else x.Location = new Point(x.Location.X - (x.Location.X % 40), x.Location.Y - (x.Location.Y % 40));
                if (e.Button == MouseButtons.Right)
                {
                    for (int f = 0; f < Schiffe.Length; f++)
                    {
                        if (x.Equals(Schiffe[f]))
                        {
                            SchiffeHo[f].Location = Schiffe[f].Location;
                            Schiffe[f].Visible = false;
                            SchiffeHo[f].Visible = true;
                            Kollisionsabfrage(SchiffeHo[f]);
                        }
                        else if (x.Equals(SchiffeHo[f]))
                        {
                            Schiffe[f].Location = SchiffeHo[f].Location;
                            SchiffeHo[f].Visible = false;
                            Schiffe[f].Visible = true;
                            Kollisionsabfrage(Schiffe[f]);
                        }
                    }
                }
                else
                    Kollisionsabfrage(sender);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            activeControl = null;
            Cursor = Cursors.Default;


        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label21.Visible = true;
            label22.Visible = true;
            label23.Visible = true;
            label24.Visible = true;
            label25.Visible = true;
            label26.Visible = true;
            label27.Visible = true;
            label28.Visible = true;
            label29.Visible = true;
            label30.Visible = true;
            btnStart.Enabled = false;
            btnRandom.Enabled = false;
            for (int f = 0; f < Schiffsteile.Length; f++)
            {
                Schiffsteile[f].MouseUp -= Schiffe_MouseUp;
                Schiffsteile[f].MouseDown -= Schiffe_MouseDown;
                Schiffsteile[f].MouseMove -= Schiffe_MouseMove;
            }
            for (int f = 0; f < Schiffe.Length; f++)
            {
                Schiffe[f].MouseUp -= Schiffe_MouseUp;
                Schiffe[f].MouseDown -= Schiffe_MouseDown;
                Schiffe[f].MouseMove -= Schiffe_MouseMove;
                SchiffeHo[f].MouseUp -= Schiffe_MouseUp;
                SchiffeHo[f].MouseDown -= Schiffe_MouseDown;
                SchiffeHo[f].MouseMove -= Schiffe_MouseMove;
            }
            pnSpielbrett.Visible = true;
            for (int f = 0; f < Spieler.Length; f++)
            {
                Spieler[f] = new int[10];
                Versuche[f] = new int[10];
            }
            for (int f = 0; f < Spieler.Length; f++)
            {
                for (int g = 0; g < Spieler[f].Length; g++)
                {
                    Versuche[f][g] = 0;
                    Spieler[f][g] = 1;
                    for (int h = 0; h < Schiffsteile.Length; h++)
                    {
                        if (Schiffsteile[h].Parent.Visible)
                        {
                            if (Schiffsteile[h].Location.X + Schiffsteile[h].Parent.Location.X >= 40 * g && Schiffsteile[h].Location.X + Schiffsteile[h].Parent.Location.X < 40 * g + 40 &&
                                Schiffsteile[h].Location.Y + Schiffsteile[h].Parent.Location.Y >= 40 * f && Schiffsteile[h].Location.Y + Schiffsteile[h].Parent.Location.Y < 40 * f + 40)
                            {
                                switch (Schiffsteile[h].Parent.Name)
                                {
                                    case "Zweier": Spieler[f][g] = 2; break;
                                    case "Dreier": Spieler[f][g] = 3; break;
                                    case "Dreier2": Spieler[f][g] = 4; break;
                                    case "Vierer": Spieler[f][g] = 5; break;
                                    case "Fuenfer": Spieler[f][g] = 6; break;
                                    case "DreierX": Spieler[f][g] = 7; break;
                                }
                            }
                            //      MessageBox.Show(h+": "+Spieler[f][g] + "," + Schiffsteile[h].Location.ToString()+";X: "+(40*g)+"-"+(40*g+40) + ";Y: " + (40 * f) + "-" + (40 * f + 40));
                        }
                        if (Spieler[f][g] != 1)
                            break;
                    }
                    //MessageBox.Show(f + ":" + g + "=" + Spieler[f][g]);
                }
            }
            KI = new Gegner(pnSpielbrett, Spieler);

        }
        private void btnRandom_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Noch in Arbeit");
            Spieler = RandomS();
            bool[] einmal = new bool[6];
            for (int f = 0; f < einmal.Length; f++)
            {
                einmal[f] = true;
            }
            for (int f = 0; f < Spieler.Length; f++)
            {
                for (int g = 0; g < Spieler[f].Length; g++)
                {
                    switch (Spieler[f][g])
                    {
                        case 2: if (einmal[0]) try { if (Spieler[f][g + 1] == Spieler[f][g]) { SchiffeHo[0].Visible = false; Schiffe[0].Location = new Point(g * 40, f * 40); Schiffe[0].Visible = true; } else { throw new Exception(); } } catch (Exception) { Schiffe[0].Visible = false; SchiffeHo[0].Location = new Point(g * 40, f * 40); SchiffeHo[0].Visible = true; } einmal[0] = false; break;
                        case 3: if (einmal[1]) try { if (Spieler[f][g + 1] == Spieler[f][g]) { SchiffeHo[1].Visible = false; Schiffe[1].Location = new Point(g * 40, f * 40); Schiffe[1].Visible = true; } else { throw new Exception(); } } catch (Exception) { Schiffe[1].Visible = false; SchiffeHo[1].Location = new Point(g * 40, f * 40); SchiffeHo[1].Visible = true; } einmal[1] = false; break;
                        case 4: if (einmal[2]) try { if (Spieler[f][g + 1] == Spieler[f][g]) { SchiffeHo[2].Visible = false; Schiffe[2].Location = new Point(g * 40, f * 40); Schiffe[2].Visible = true; } else { throw new Exception(); } } catch (Exception) { Schiffe[2].Visible = false; SchiffeHo[2].Location = new Point(g * 40, f * 40); SchiffeHo[2].Visible = true; } einmal[2] = false; break;
                        case 5: if (einmal[3]) try { if (Spieler[f][g + 1] == Spieler[f][g]) { SchiffeHo[3].Visible = false; Schiffe[3].Location = new Point(g * 40, f * 40); Schiffe[3].Visible = true; } else { throw new Exception(); } } catch (Exception) { Schiffe[3].Visible = false; SchiffeHo[3].Location = new Point(g * 40, f * 40); SchiffeHo[3].Visible = true; } einmal[3] = false; break;
                        case 6: if (einmal[4]) try { if (Spieler[f][g + 1] == Spieler[f][g]) { SchiffeHo[4].Visible = false; Schiffe[4].Location = new Point(g * 40, f * 40); Schiffe[4].Visible = true; } else { throw new Exception(); } } catch (Exception) { Schiffe[4].Visible = false; SchiffeHo[4].Location = new Point(g * 40, f * 40); SchiffeHo[4].Visible = true; } einmal[4] = false; break;
                        case 7: if (einmal[5]) try { if (Spieler[f][g + 1] == Spieler[f][g]) { SchiffeHo[5].Visible = false; Schiffe[5].Location = new Point(g * 40, f * 40); Schiffe[5].Visible = true; } else { throw new Exception(); } } catch (Exception) { Schiffe[5].Visible = false; SchiffeHo[5].Location = new Point(g * 40, f * 40); SchiffeHo[5].Visible = true; } einmal[5] = false; break;
                    }
                }
            }
        }
        private void pnSpielbrett_MouseClick(object sender, MouseEventArgs e)
        {
            if (pnSpielbrett.Enabled)
            {
                // MessageBox.Show(e.X+","+e.Y);
                //MessageBox.Show(e.X / 40 + ", " + (e.Y / 40));
                if (Versuche[e.Y / 40][e.X / 40] == 0)
                {
                    Versuche = KI.Versuch(e.Y / 40, e.X / 40, Versuche);
                    if (Versuche[e.Y / 40][e.X / 40] == 1)
                    {
                        bgColors[e.X / 40, e.Y / 40] = Color.Blue;
                        pnSpielbrett.Refresh();
                        pnSpielbrett.Enabled = false;
                    }
                    else
                    {
                        Untergehen[Versuche[e.Y / 40][e.X / 40] - 2][++Untergehen[6][Versuche[e.Y / 40][e.X / 40] - 2]] = (e.Y / 40) * 10 + (e.X / 40);
                        bgColors[e.X / 40, e.Y / 40] = Color.Red;

                        if (Untergehen[0][1] != -1)
                        {
                            for (int f = 0; f < Untergehen[0].Length; f++)
                            {
                                bgColors[Untergehen[0][f] % 10, Untergehen[0][f] / 10] = Color.Orange;
                            }
                            Untergehen[0][1] = -1;

                        }
                        if (Untergehen[1][2] != -1)
                        {
                            for (int f = 0; f < Untergehen[1].Length; f++)
                            {
                                bgColors[Untergehen[1][f] % 10, Untergehen[1][f] / 10] = Color.Orange;
                            }
                            Untergehen[1][2] = -1;

                        }
                        if (Untergehen[2][2] != -1)
                        {
                            for (int f = 0; f < Untergehen[2].Length; f++)
                            {
                                bgColors[Untergehen[2][f] % 10, Untergehen[2][f] / 10] = Color.Orange;
                            }
                            Untergehen[2][2] = -1;
                        }
                        if (Untergehen[3][3] != -1)
                        {
                            for (int f = 0; f < Untergehen[3].Length; f++)
                            {
                                bgColors[Untergehen[3][f] % 10, Untergehen[3][f] / 10] = Color.Orange;
                            }
                            Untergehen[3][3] = -1;
                        }
                        if (Untergehen[4][4] != -1)
                        {
                            for (int f = 0; f < Untergehen[4].Length; f++)
                            {
                                bgColors[Untergehen[4][f] % 10, Untergehen[4][f] / 10] = Color.Orange;
                            }
                            Untergehen[4][4] = -1;
                        }
                        if (Untergehen[5][3] != -1)
                        {
                            for (int f = 0; f < Untergehen[5].Length; f++)
                            {
                                bgColors[Untergehen[5][f] % 10, Untergehen[5][f] / 10] = Color.Orange;
                            }
                            Untergehen[5][3] = -1;
                        }
                        pnSpielbrett.Refresh();
                        if (++WinC == Schiffsteile.Length / 2) Gewonnen(true);
                    }

                }


            }
        }
        private void pnSpielbrett_EnabledChanged(object sender, EventArgs e)
        {
            if (!pnSpielbrett.Enabled)
            {
                int platz = KI.Schiessen();
                switch (platz % 10)
                {
                    case 0: MessageBox.Show("A" + (platz / 10 + 1)); break;
                    case 1: MessageBox.Show("B" + (platz / 10 + 1)); break;
                    case 2: MessageBox.Show("C" + (platz / 10 + 1)); break;
                    case 3: MessageBox.Show("D" + (platz / 10 + 1)); break;
                    case 4: MessageBox.Show("E" + (platz / 10 + 1)); break;
                    case 5: MessageBox.Show("F" + (platz / 10 + 1)); break;
                    case 6: MessageBox.Show("G" + (platz / 10 + 1)); break;
                    case 7: MessageBox.Show("H" + (platz / 10 + 1)); break;
                    case 8: MessageBox.Show("I" + (platz / 10 + 1)); break;
                    case 9: MessageBox.Show("J" + (platz / 10 + 1)); break;
                }

                if (Spieler[platz / 10][platz % 10] <= 1)
                {
                    PictureBox picture = new PictureBox();
                    picture.Image = wasser;
                    picture.Size = new Size(40, 40);
                    picture.Location = new Point(platz % 10 * 40, platz / 10 * 40);
                    pnSpielbereit.Controls.Add(picture);
                }
                else
                {
                    for (int h = 0; h < Schiffsteile.Length; h++)
                    {
                        if (Schiffsteile[h].Parent.Visible)
                        {
                            if (Schiffsteile[h].Location.X + Schiffsteile[h].Parent.Location.X >= 40 * (platz % 10) && Schiffsteile[h].Location.X + Schiffsteile[h].Parent.Location.X < 40 * (platz % 10) + 40 &&
                                Schiffsteile[h].Location.Y + Schiffsteile[h].Parent.Location.Y >= 40 * (platz / 10) && Schiffsteile[h].Location.Y + Schiffsteile[h].Parent.Location.Y < 40 * (platz / 10) + 40)
                            {
                                Schiffsteile[h].Image = treffer;
                            }
                            //      MessageBox.Show(h+": "+Spieler[f][g] + "," + Schiffsteile[h].Location.ToString()+";X: "+(40*g)+"-"+(40*g+40) + ";Y: " + (40 * f) + "-" + (40 * f + 40));
                        }
                    }
                    //pnSpielbrett.Controls.RemoveAt();
                }
                if (Spieler[platz / 10][platz % 10] > 1) { if (winC++ == Schiffsteile.Length / 2) Gewonnen(false); else pnSpielbrett_EnabledChanged(sender, e); }
                pnSpielbrett.Enabled = true;
            }
            // else //MessageBox.Show("Spieler ist dran"); 
            //pnSpielbrett.Enabled = false;

        }
        private void pnSpielbrett_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            using (var b = new SolidBrush(bgColors[e.Column, e.Row]))
            {
                e.Graphics.FillRectangle(b, e.CellBounds);
            }
        }

        private void Kollisionsabfrage(object sender)
        {
            btnStart.Enabled = true;
            sender = Tabel(sender);
            TableLayoutPanel x = sender as TableLayoutPanel;
            Control[] c = x.Controls.Find("btn", true);
            // MessageBox.Show(c.Length + "");
            for (int f = 0; f < c.Length; f++)
            {

                for (int g = 0; g < Schiffsteile.Length; g++)
                {
                    if (Schiffsteile[g] != c[f] && Schiffsteile[g].Parent.Visible)
                    {
                        // MessageBox.Show(g+":"+(c[f] as Button).Location.X + "<=" + Schiffsteile[g].Location.X + "&&" + ((c[f] as Button).Location.X + (c[f] as Button).Size.Width )+ ">=" + Schiffsteile[g].Location.X + "&&" +
                        // (c[f] as Button).Location.Y + "<=" + Schiffsteile[g].Location.Y + "&&" + ((c[f] as Button).Location.X + (c[f] as Button).Size.Height) + ">=" + Schiffsteile[g].Location.Y);
                        if (((c[f] as PictureBox).Location.X + x.Location.X <= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X && (c[f] as PictureBox).Location.X + (c[f] as PictureBox).Size.Width + x.Location.X >= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X &&
                        (c[f] as PictureBox).Location.Y + x.Location.Y <= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y && (c[f] as PictureBox).Location.Y + (c[f] as PictureBox).Size.Height + x.Location.Y >= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y) ||

                        ((c[f] as PictureBox).Location.X + x.Location.X <= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X + Schiffsteile[g].Size.Width && (c[f] as PictureBox).Location.X + (c[f] as PictureBox).Size.Width + x.Location.X >= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X + Schiffsteile[g].Size.Width &&
                        (c[f] as PictureBox).Location.Y + x.Location.Y <= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y + Schiffsteile[g].Size.Height && (c[f] as PictureBox).Location.Y + (c[f] as PictureBox).Size.Height + x.Location.Y >= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y + Schiffsteile[g].Size.Height) ||

                        ((c[f] as PictureBox).Location.X + x.Location.X <= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X && (c[f] as PictureBox).Location.X + (c[f] as PictureBox).Size.Width + x.Location.X >= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X &&
                        (c[f] as PictureBox).Location.Y + x.Location.Y <= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y + Schiffsteile[g].Size.Height && (c[f] as PictureBox).Location.Y + (c[f] as PictureBox).Size.Height + x.Location.Y >= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y + Schiffsteile[g].Size.Height) ||

                        ((c[f] as PictureBox).Location.X + x.Location.X <= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X + Schiffsteile[g].Size.Width && (c[f] as PictureBox).Location.X + (c[f] as PictureBox).Size.Width + x.Location.X >= Schiffsteile[g].Location.X + Schiffsteile[g].Parent.Location.X + Schiffsteile[g].Size.Width &&
                        (c[f] as PictureBox).Location.Y + x.Location.Y <= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y && (c[f] as PictureBox).Location.Y + (c[f] as PictureBox).Size.Height + x.Location.Y >= Schiffsteile[g].Location.Y + Schiffsteile[g].Parent.Location.Y)
                        )
                        {
                            //MessageBox.Show("Kollision mit " + g);
                            btnStart.Enabled = false;
                        }
                    }
                }
            }
        }
        private object Tabel(object btnO)
        {
            PictureBox btn = btnO as PictureBox;
            if (btn != null)
            {
                if (btn.Parent != null)
                    return btn.Parent;
                else
                    return btnO;
            }
            /*if(btnO.Equals(Schiffsteile[0] as object)&& btn == Schiffsteile[1])
            {
                MessageBox.Show("hey");
                return Schiffe[0] as object;
            }
            else if (btn == Schiffsteile[2] && btn == Schiffsteile[3])
            {
                return SchiffeHo[0] as object;
            }
            else if (btn == Schiffsteile[4] && btn == Schiffsteile[5] && btn == Schiffsteile[6])
            {
                return Schiffe[1] as object;
            }
            else if (btn == Schiffsteile[7] && btn == Schiffsteile[8] && btn == Schiffsteile[9])
            {
                return SchiffeHo[1] as object;
            }
            else if (btn == Schiffsteile[10] && btn == Schiffsteile[11] && btn == Schiffsteile[12])
            {
                return Schiffe[2] as object;
            }
            else if (btn == Schiffsteile[13] && btn == Schiffsteile[14] && btn == Schiffsteile[15])
            {
                return SchiffeHo[2] as object;
            }
            else if (btn == Schiffsteile[16] && btn == Schiffsteile[17] && btn == Schiffsteile[18] && btn == Schiffsteile[19])
            {
                return Schiffe[3] as object;
            }
            else if (btn == Schiffsteile[20] && btn == Schiffsteile[21] && btn == Schiffsteile[22] && btn == Schiffsteile[23])
            {
                return SchiffeHo[3] as object;
            }
            else if (btn == Schiffsteile[24] && btn == Schiffsteile[25] && btn == Schiffsteile[26] && btn == Schiffsteile[27])
            {
                return Schiffe[4] as object;
            }
            else if (btn == Schiffsteile[28] && btn == Schiffsteile[29] && btn == Schiffsteile[30] && btn == Schiffsteile[31])
            {
                return SchiffeHo[4] as object;
            }
            else if (btn == Schiffsteile[32] && btn == Schiffsteile[33] && btn == Schiffsteile[34] && btn == Schiffsteile[35] && btn == Schiffsteile[36])
            {
                return Schiffe[5] as object;
            }
            else if (btn == Schiffsteile[37] && btn == Schiffsteile[38] && btn == Schiffsteile[39] && btn == Schiffsteile[40] && btn == Schiffsteile[41])
            {
                return SchiffeHo[5] as object;
            }*/
            else
            {
                return btnO;
            }
        }
        private void Gewonnen(bool v)
        {
            if (v) MessageBox.Show("Spieler hat gewonnen");
            else MessageBox.Show("KI hat gewonnen");
            Close();
        }
        private void Randuerberprufung(object sender)
        {
            while ((Tabel(sender) as TableLayoutPanel).Location.X < 0)
                (Tabel(sender) as TableLayoutPanel).Location = new Point((Tabel(sender) as TableLayoutPanel).Location.X + 10, (Tabel(sender) as TableLayoutPanel).Location.Y);
            while ((Tabel(sender) as TableLayoutPanel).Location.X + (Tabel(sender) as TableLayoutPanel).Width > 400)
                (Tabel(sender) as TableLayoutPanel).Location = new Point((Tabel(sender) as TableLayoutPanel).Location.X - 10, (Tabel(sender) as TableLayoutPanel).Location.Y);
            while ((Tabel(sender) as TableLayoutPanel).Location.Y < 0)
                (Tabel(sender) as TableLayoutPanel).Location = new Point((Tabel(sender) as TableLayoutPanel).Location.X, (Tabel(sender) as TableLayoutPanel).Location.Y + 10);
            while ((Tabel(sender) as TableLayoutPanel).Location.Y + (Tabel(sender) as TableLayoutPanel).Height > 400)
                (Tabel(sender) as TableLayoutPanel).Location = new Point((Tabel(sender) as TableLayoutPanel).Location.X, (Tabel(sender) as TableLayoutPanel).Location.Y - 10);
        }

        public static int[][] RandomS()
        {
            int[][] erg = new int[10][];
            for (int f = 0; f < erg.Length; f++)
            {
                erg[f] = new int[10];
                for (int g = 0; g < erg[f].Length; g++)
                {
                    erg[f][g] = 1;
                }
            }
            Random rnd = new Random();
            int platz;
            do
            {
                platz = rnd.Next(100);
                try
                {
                    if (rnd.Next() % 2 == 0) { erg[platz / 10][platz % 10 + 1] = 2; erg[platz / 10][platz % 10] = 2; }
                    else { erg[platz / 10 + 1][platz % 10] = 2; erg[platz / 10][platz % 10] = 2; }
                }
                catch (IndexOutOfRangeException) { }
            }
            while (erg[platz / 10][platz % 10] != 2);

            do
            {
                platz = rnd.Next(100);
                try
                {
                    if (rnd.Next() % 2 == 0)
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10][platz % 10 + 1] == 1 && erg[platz / 10][platz % 10 + 2] == 1)
                        {
                            erg[platz / 10][platz % 10] = 3; erg[platz / 10][platz % 10 + 1] = 3; erg[platz / 10][platz % 10 + 2] = 3;
                        }
                    }
                    else
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10 + 1][platz % 10] == 1 && erg[platz / 10 + 2][platz % 10] == 1)
                        {
                            erg[platz / 10][platz % 10] = 3; erg[platz / 10 + 1][platz % 10] = 3; erg[platz / 10 + 2][platz % 10] = 3;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            while (erg[platz / 10][platz % 10] != 3);

            do
            {
                platz = rnd.Next(100);
                try
                {
                    if (rnd.Next() % 2 == 0)
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10][platz % 10 + 1] == 1 && erg[platz / 10][platz % 10 + 2] == 1)
                        {
                            erg[platz / 10][platz % 10] = 4; erg[platz / 10][platz % 10 + 1] = 4; erg[platz / 10][platz % 10 + 2] = 4;
                        }
                    }
                    else
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10 + 1][platz % 10] == 1 && erg[platz / 10 + 2][platz % 10] == 1)
                        {
                            erg[platz / 10][platz % 10] = 4; erg[platz / 10 + 1][platz % 10] = 4; erg[platz / 10 + 2][platz % 10] = 4;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            while (erg[platz / 10][platz % 10] != 4);


            do
            {
                platz = rnd.Next(100);
                try
                {
                    if (rnd.Next() % 2 == 0)
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10][platz % 10 + 1] == 1 && erg[platz / 10][platz % 10 + 2] == 1 && erg[platz / 10][platz % 10 + 3] == 1)
                        {
                            erg[platz / 10][platz % 10] = 5; erg[platz / 10][platz % 10 + 1] = 5; erg[platz / 10][platz % 10 + 2] = 5; erg[platz / 10][platz % 10 + 3] = 5;
                        }
                    }
                    else
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10 + 1][platz % 10] == 1 && erg[platz / 10 + 2][platz % 10] == 1 && erg[platz / 10 + 3][platz % 10] == 1)
                        {
                            erg[platz / 10][platz % 10] = 5; erg[platz / 10 + 1][platz % 10] = 5; erg[platz / 10 + 2][platz % 10] = 5; erg[platz / 10 + 3][platz % 10] = 5;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            while (erg[platz / 10][platz % 10] != 5);

            do
            {
                platz = rnd.Next(100);
                try
                {
                    if (rnd.Next() % 2 == 0)
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10][platz % 10 + 1] == 1 && erg[platz / 10][platz % 10 + 2] == 1 && erg[platz / 10][platz % 10 + 3] == 1 && erg[platz / 10][platz % 10 + 4] == 1)
                        {
                            erg[platz / 10][platz % 10] = 6; erg[platz / 10][platz % 10 + 1] = 6; erg[platz / 10][platz % 10 + 2] = 6; erg[platz / 10][platz % 10 + 3] = 6; erg[platz / 10][platz % 10 + 4] = 6;
                        }
                    }
                    else
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10 + 1][platz % 10] == 1 && erg[platz / 10 + 2][platz % 10] == 1 && erg[platz / 10 + 3][platz % 10] == 1 && erg[platz / 10 + 4][platz % 10] == 1)
                        {
                            erg[platz / 10][platz % 10] = 6; erg[platz / 10 + 1][platz % 10] = 6; erg[platz / 10 + 2][platz % 10] = 6; erg[platz / 10 + 3][platz % 10] = 6; erg[platz / 10 + 4][platz % 10] = 6;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            while (erg[platz / 10][platz % 10] != 6);

            do
            {
                platz = rnd.Next(100);
                try
                {
                    if (rnd.Next() % 2 == 0)
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10][platz % 10 + 1] == 1 && erg[platz / 10][platz % 10 + 2] == 1 && erg[platz / 10 + 1][platz % 10 + 1] == 1)
                        {
                            erg[platz / 10][platz % 10] = 7; erg[platz / 10][platz % 10 + 1] = 7; erg[platz / 10][platz % 10 + 2] = 7; erg[platz / 10 + 1][platz % 10 + 1] = 7;
                        }
                    }
                    else
                    {
                        if (erg[platz / 10][platz % 10] == 1 && erg[platz / 10 + 1][platz % 10] == 1 && erg[platz / 10 + 2][platz % 10] == 1 && erg[platz / 10 + 1][platz % 10 + 1] == 1)
                        {
                            erg[platz / 10][platz % 10] = 7; erg[platz / 10 + 1][platz % 10] = 7; erg[platz / 10 + 2][platz % 10] = 7; erg[platz / 10 + 1][platz % 10 + 1] = 7;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            while (erg[platz / 10][platz % 10] != 7);            //throw new NotImplementedException();

            /* for (int f = 0; f < erg.Length; f++)
             {
                 erg[f] = new int[10];
             }
             erg[0][0] = 2;
             erg[0][1] = 2;
             erg[0][2] = 3;
             erg[0][3] = 3;
             erg[0][4] = 3;
             erg[0][5] = 4;
             erg[0][6] = 4;
             erg[0][7] = 4;
             erg[0][8] = 1;
             erg[0][9] = 1;

             erg[1][0] = 5;
             erg[1][1] = 5;
             erg[1][2] = 5;
             erg[1][3] = 5;
             erg[1][4] = 6;
             erg[1][5] = 6;
             erg[1][6] = 6;
             erg[1][7] = 6;
             erg[1][8] = 6;
             erg[1][9] = 1;

             erg[2][0] = 7;
             erg[2][1] = 7;
             erg[2][2] = 7;
             erg[2][3] = 1;
             erg[2][4] = 1;
             erg[2][5] = 1;
             erg[2][6] = 1;
             erg[2][7] = 1;
             erg[2][8] = 1;
             erg[2][9] = 1;

             erg[3][0] = 1;
             erg[3][1] = 7;
             erg[3][2] = 1;
             erg[3][3] = 1;
             erg[3][4] = 1;
             erg[3][5] = 1;
             erg[3][6] = 1;
             erg[3][7] = 1;
             erg[3][8] = 1;
             erg[3][9] = 1;

             erg[4][0] = 1;
             erg[4][1] = 1;
             erg[4][2] = 1;
             erg[4][3] = 1;
             erg[4][4] = 1;
             erg[4][5] = 1;
             erg[4][6] = 1;
             erg[4][7] = 1;
             erg[4][8] = 1;
             erg[4][9] = 1;

             erg[5][0] = 1;
             erg[5][1] = 1;
             erg[5][2] = 1;
             erg[5][3] = 1;
             erg[5][4] = 1;
             erg[5][5] = 1;
             erg[5][6] = 1;
             erg[5][7] = 1;
             erg[5][8] = 1;
             erg[5][9] = 1;

             erg[6][0] = 1;
             erg[6][1] = 1;
             erg[6][2] = 1;
             erg[6][3] = 1;
             erg[6][4] = 1;
             erg[6][5] = 1;
             erg[6][6] = 1;
             erg[6][7] = 1;
             erg[6][8] = 1;
             erg[6][9] = 1;

             erg[7][0] = 1;
             erg[7][1] = 1;
             erg[7][2] = 1;
             erg[7][3] = 1;
             erg[7][4] = 1;
             erg[7][5] = 1;
             erg[7][6] = 1;
             erg[7][7] = 1;
             erg[7][8] = 1;
             erg[7][9] = 1;

             erg[8][0] = 1;
             erg[8][1] = 1;
             erg[8][2] = 1;
             erg[8][3] = 1;
             erg[8][4] = 1;
             erg[8][5] = 1;
             erg[8][6] = 1;
             erg[8][7] = 1;
             erg[8][8] = 1;
             erg[8][9] = 1;

             erg[9][0] = 1;
             erg[9][1] = 1;
             erg[9][2] = 1;
             erg[9][3] = 1;
             erg[9][4] = 1;
             erg[9][5] = 1;
             erg[9][6] = 1;
             erg[9][7] = 1;
             erg[9][8] = 1;
             erg[9][9] = 1;*/
            return erg;
        }
    }
}
