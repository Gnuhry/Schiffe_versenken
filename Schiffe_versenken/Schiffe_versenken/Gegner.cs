using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schiffe_versenken
{
    class Gegner
    {
        private TableLayoutPanel Spielbrett;
        private List<int> erg;
        private int[][] Schiffe, Spieler;
        private int[][] Geschossen;//0-KA; 1-Wasser; 2...-Treffer; -1-Versenkt

        public Gegner(TableLayoutPanel Spielbrett, int[][] Spieler)
        {
            this.Spieler = Spieler;
            this.Spielbrett = Spielbrett;
            Geschossen = new int[10][];
            for (int f = 0; f < Geschossen.Length; f++)
            {
                Geschossen[f] = new int[10];
                for (int g = 0; g < Geschossen[f].Length; g++)
                {
                    Geschossen[f][g] = 0;
                }
            }
            erg = new List<int>();
            //Schiffe erstellen
            Schiffe = Form1.RandomS();
        }

        public int[][] Versuch(int f, int g, int[][] Versuche)
        {
            Versuche[f][g] = Schiffe[f][g];
            return Versuche;
        }
        public int Schiessen()
        {
            int Zahl = -1; bool aa; int richtung = 0; int z = -1, y = -1; List<int> Versuch = new List<int>(), dSchiff = new List<int>();
            Random x = new Random();
            for (int f = 0; f < Geschossen.Length; f++)
            {
                for (int g = 0; g < Geschossen[f].Length; g++)
                {
                    if (Geschossen[f][g] > 1)
                    {
                        try { if (Geschossen[f][g + 1] == Geschossen[f][g]) { richtung = 1; } } catch (Exception) { }
                        try { if (Geschossen[f][g - 1] == Geschossen[f][g]) { richtung = 1; } } catch (Exception) { }
                        try { if (Geschossen[f + 1][g] == Geschossen[f][g]) { richtung = 2; } } catch (Exception) { }
                        try { if (Geschossen[f - 1][g] == Geschossen[f][g]) { richtung = 2; } } catch (Exception) { }
                        y = f; z = g;
                        break;
                    }
                }
            }
            if (y == -1)
            {
                do
                {
                    aa = true;
                    Zahl = x.Next(100);
                    if (Geschossen[Zahl / 10][Zahl % 10] == 0)
                        aa = false;
                }
                while (aa);
            }
            else
            {
                dSchiff.Add(y * 10 + z);
                switch (richtung)
                {
                    case 0:
                        try { if (Geschossen[y][z + 1] == 0) { Versuch.Add(y * 10 + z + 1); } } catch (Exception) { }
                        try { if (Geschossen[y][z - 1] == 0) { Versuch.Add(y * 10 + z - 1); } } catch (Exception) { }
                        try { if (Geschossen[y + 1][z] == 0) { Versuch.Add((y + 1) * 10 + z); } } catch (Exception) { }
                        try { if (Geschossen[y - 1][z] == 0) { Versuch.Add((y - 1) * 10 + z); } } catch (Exception) { }
                        break;
                    case 1:
                        int zV = z;
                        do
                        {
                            aa = false; try
                            {
                                if (Geschossen[y][z + 1] == 0) { Versuch.Add(y * 10 + z + 1); }
                                else if (Geschossen[y][z + 1] == Geschossen[y][z]) { aa = true; dSchiff.Add(y * 10 + z + 1); }
                            }
                            catch (Exception) { }
                            z++;
                        } while (aa);
                        z = zV;
                        do
                        {
                            aa = false; try
                            {
                                if (Geschossen[y][z - 1] == 0) { Versuch.Add(y * 10 + z - 1); }
                                else if (Geschossen[y][z - 1] == Geschossen[y][z]) { aa = true; dSchiff.Add(y * 10 + z - 1); }
                            }
                            catch (Exception) { }
                            z--;
                        } while (aa);
                        z = zV;
                        break;
                    case 2:
                        int yV = y;
                        do
                        {
                            aa = false; try
                            {
                                if (Geschossen[y + 1][z] == 0) { Versuch.Add((y + 1) * 10 + z); }
                                else if (Geschossen[y + 1][z] == Geschossen[y][z]) { aa = true; dSchiff.Add((y + 1) * 10 + z); }
                            }
                            catch (Exception) { }
                            y++;
                        } while (aa);
                        y = yV;
                        do
                        {
                            aa = false; try
                            {
                                if (Geschossen[y - 1][z] == 0) { Versuch.Add((y - 1) * 10 + z); }
                                else if (Geschossen[y - 1][z] == Geschossen[y][z]) { aa = true; dSchiff.Add((y - 1) * 10 + z); }
                            }
                            catch (Exception) { }
                            y--;
                        } while (aa);
                        y = yV;
                        break;
                }
                if (Versuch.Count == 0)
                {
                    Zahl = DreierX(dSchiff, richtung == 2);
                }
                else
                    Zahl = Versuch[x.Next(0, Versuch.Count)];
            }
            Geschossen[Zahl / 10][Zahl % 10] = Spieler[Zahl / 10][Zahl % 10];
            /* if (Spieler[Zahl / 10][Zahl % 10] > 1) dSchiff.Add(Zahl);
             switch (Geschossen[Zahl / 10][Zahl % 10]) {
                 case 2: if (dSchiff.Count == 2) for (int f = 0; f < dSchiff.Count; f++) Geschossen[dSchiff[f] / 10][dSchiff[f] % 10] = -1; break;
                 case 3: if (dSchiff.Count == 3) for (int f = 0; f < dSchiff.Count; f++) Geschossen[dSchiff[f] / 10][dSchiff[f] % 10] = -1; break;
                 case 4: if (dSchiff.Count == 3) for (int f = 0; f < dSchiff.Count; f++) Geschossen[dSchiff[f] / 10][dSchiff[f] % 10] = -1; break;
                 case 5: if (dSchiff.Count == 4) for (int f = 0; f < dSchiff.Count; f++) Geschossen[dSchiff[f] / 10][dSchiff[f] % 10] = -1; break;
                 case 6: if (dSchiff.Count == 5) { for (int f = 0; f < dSchiff.Count; f++) Geschossen[dSchiff[f] / 10][dSchiff[f] % 10] = -1; MessageBox.Show("juhe"); } break;
                 case 7: if (dSchiff.Count == 4) for (int f = 0; f < dSchiff.Count; f++) Geschossen[dSchiff[f] / 10][dSchiff[f] % 10] = -1; break;
            } */
            Untergegangen(Geschossen[Zahl / 10][Zahl % 10]);

            return Zahl;
        }

        private void Untergegangen(int v)
        {
            if (v == 1) return;
            int[] erg = new int[5]; int ergC = 0; bool unter = false;
            for (int f = 0; f < erg.Length; f++) { erg[f] = -1; }
            for (int f = 0; f < Geschossen.Length; f++)
            {
                for (int g = 0; g < Geschossen[f].Length; g++)
                {
                    if (Geschossen[f][g] == v) { erg[ergC++] = f * 10 + g; }
                }
            }
            switch (v)
            {
                case 2: if (erg[1] != -1) unter = true; break;
                case 3: if (erg[2] != -1) unter = true; break;
                case 4: if (erg[2] != -1) unter = true; break;
                case 5: if (erg[3] != -1) unter = true; break;
                case 6: if (erg[4] != -1) unter = true; break;
                case 7: if (erg[3] != -1) unter = true; break;
            }
            if (unter)
                for (int f = 0; f < erg.Length; f++)
                {
                    if (erg[f] != -1) { Geschossen[erg[f] / 10][erg[f] % 10] = -1; }
                }
        }
        private int DreierX(List<int> dSchiff, bool richtung)
        {
            int number;
            //MessageBox.Show("Drier");
            if (dSchiff.Count == 3)
            {
                //MessageBox.Show(dSchiff[0]+","+dSchiff[1]+","+dSchiff[2]);
                if ((dSchiff[0] > dSchiff[1] && dSchiff[1] > dSchiff[2]) || (dSchiff[2] > dSchiff[1] && dSchiff[1] > dSchiff[0]))
                {
                    //Mitte ist 1
                    number = dSchiff[1];
                }
                else if ((dSchiff[1] > dSchiff[0] && dSchiff[0] > dSchiff[2]) || (dSchiff[2] > dSchiff[0] && dSchiff[0] > dSchiff[1]))
                {
                    //Mitte ist 0
                    number = dSchiff[0];
                }
                else
                {
                    //Mitte ist 2
                    number = dSchiff[2];
                }
                // MessageBox.Show(number + "");
                /* if (richtung) try { if (Geschossen[number/ 10][(number% 10) + 1] == 0) Versuche.Add(number + 1); } catch (IndexOutOfRangeException) { }
                 else try { if (Geschossen[(number/ 10) + 1][number % 10] == 0) Versuche.Add(number + 10); } catch (IndexOutOfRangeException) { }*/
                if (richtung) { return number + 1; }
                else return (number + 10);
            }
            else if (dSchiff.Count == 2)
            {
                if (dSchiff[0] > dSchiff[1]) { Geschossen[dSchiff[0] / 10][dSchiff[0] % 10] = -1; }
                else { Geschossen[dSchiff[1] / 10][dSchiff[1] % 10] = -1; }
                return Schiessen();
            }
            bool aa; int Zahl; Random x = new Random();
            do
            {
                aa = true;
                Zahl = x.Next(100);
                if (Geschossen[Zahl / 10][Zahl % 10] == 0)
                    aa = false;
            }
            while (aa);
            return Zahl;
        }

        /* public List<int> Schiessen(int[][] Schiffe)
         {//0-99 Wasser getroffen; 100-199 Treffer
             Random x = new Random(); int Schuss = -1; bool aa = true; ;
             for (int f = 0; f < Geschoßen.Length; f++)
                 for (int g = 0; g < Geschoßen[f].Length; g++)
                     if (Geschoßen[f][g] == 2)
                     {
                         aa = false;
                         break;
                     }
             if (aa)
             {
                 erg = new List<int>();
                 do
                 {
                     aa = false;
                     Schuss = x.Next(0, 99);
                     for (int f = 0; f < Geschoßen.Length; f++)
                     {
                         for (int g = 0; g < Geschoßen[f].Length; g++)
                         {
                             if (Geschoßen[f][g] != 0)
                                 aa = true;
                         }
                     }
                 }
                 while (aa);
             }
             else {
                 bool ac = true;
                 //Ueberprufung Schiff versenkt
                 int[][] test = new int[7][];
                 test[0] = new int[4];//Doppelte wegen f und g
                 test[1] = new int[6];
                 test[2] = new int[6];
                 test[3] = new int[8];
                 test[4] = new int[10];
                 test[5] = new int[8];
                 test[6] = new int[6];//Counter
                 for (int f = 0; f < test[0].Length; f++) test[0][f] = -1;
                 for (int f = 0; f < test[1].Length; f++) test[1][f] = -1;
                 for (int f = 0; f < test[2].Length; f++) test[2][f] = -1;
                 for (int f = 0; f < test[3].Length; f++) test[3][f] = -1;
                 for (int f = 0; f < test[4].Length; f++) test[4][f] = -1;
                 for (int f = 0; f < test[5].Length; f++) test[5][f] = -1;
                 for (int f = 0; f < test[6].Length; f++) test[6][f] = 0;

                 for (int f = 0; f < Geschoßen.Length; f++)
                 {
                     for (int g = 0; g < Geschoßen[f].Length; f++)
                     {
                         switch (Geschoßen[f][g] - 1)
                         {
                             case 2: test[0][test[6][0]++] = f; test[0][test[6][0]++] = g; break;
                             case 3: test[1][test[6][1]++] = f; test[0][test[6][1]++] = g; break;
                             case 4: test[2][test[6][2]++] = f; test[0][test[6][2]++] = g; break;
                             case 5: test[3][test[6][3]++] = f; test[0][test[6][3]++] = g; break;
                             case 6: test[4][test[6][4]++] = f; test[0][test[6][4]++] = g; break;
                             case 7: test[5][test[6][5]++] = f; test[0][test[6][5]++] = g; break;
                         }
                     }
                 }
                 for (int g = 0; g < test.Length - 1; g++) {

                     if (test[g][test[g].Length - 1] != -1)
                     {
                         ac = false;
                         for (int f = 0; f < test[g].Length; f++)
                         {
                             Geschoßen[test[g][f++]][test[g][f]] = -1;
                         }
                     }
                 }
                 if (ac)
                 {//Treffer, weiter Schiffsteile suchen
                     bool y = true;
                     for (int f = 0; f < Geschoßen.Length && y; f++)
                     {
                         for (int g = 0; g < Geschoßen[f].Length && y; g++)
                         {
                             if (Geschoßen[f][g] > 1)
                             {
                                 List<int> Ver = new List<int>();
                                 bool v = true;
                                 try { if (Geschoßen[f][g + 1] == Geschoßen[f][g]) v = false; } catch (Exception) { }
                                 try { if (Geschoßen[f][g - 1] == Geschoßen[f][g]) v = false; } catch (Exception) { }
                                 if (v) Ver = hoch(f, g, Ver);
                                 v = true;
                                 try { if (Geschoßen[f + 1][g] == Geschoßen[f][g]) v = false; } catch (Exception) { }
                                 try { if (Geschoßen[f - 1][g] == Geschoßen[f][g]) v = false; } catch (Exception) { }
                                 if (v) Ver = rechts(f, g, Ver);
                                 Schuss = x.Next(0, Ver.Count);
                                 y = false;
                             }
                         }
                     }
                 }
                 else
                 {
                     do
                     {
                         aa = false;
                         Schuss = x.Next(0, 99);
                         for (int f = 0; f < Geschoßen.Length; f++)
                         {
                             for (int g = 0; g < Geschoßen[f].Length; g++)
                             {
                                 if (Geschoßen[f][g] != 0)
                                     aa = true;
                             }
                         }
                     }
                     while (aa);
                 }
             }
             if (Schiffe[Schuss / 10][Schuss % 10] != 0) {
                 erg.Add(Schuss + 100);
                 Geschoßen[Schuss / 10][Schuss % 10] = Schiffe[Schuss / 10][Schuss % 10] + 1;
                 Schiessen(Schiffe);
             }
             else { erg.Add(Schuss); Geschoßen[Schuss / 10][Schuss % 10] = 1; }
             return erg;
         }
         private List<int> rechts(int f, int g, List<int> Ver)
         {
             try
             {
                 if (Geschoßen[f][g + 1] == Geschoßen[f][g])
                 {
                     try { if (Geschoßen[f][g - 1] == Geschoßen[f][g]) { Ver = links(f, g - 1, Ver); } } catch (Exception) { }
                     try { if (Geschoßen[f][g + 2] == Geschoßen[f][g]) { Ver = rechts(f, g + 1, Ver); } } catch (Exception) { }
                 }
                 else if (Geschoßen[f][g + 1] == 0) { Ver.Add(f * 10 + g + 1); }
             }
             catch (Exception) { }
             return Ver;
         }
         private List<int> links(int f, int g, List<int> Ver)
         {
             try
             {
                 if (Geschoßen[f][g - 1] == Geschoßen[f][g])
                 {
                     try { if (Geschoßen[f][g + 1] == Geschoßen[f][g]) { Ver = rechts(f, g + 1, Ver); } } catch (Exception) { }
                     try { if (Geschoßen[f][g - 2] == Geschoßen[f][g]) { Ver = links(f, g - 1, Ver); } } catch (Exception) { }
                 }
                 else if (Geschoßen[f][g - 1] == 0) { Ver.Add(f * 10 + g - 1); }
             }
             catch (Exception) { }
             return Ver;
         }
         private List<int> hoch(int f, int g, List<int> Ver)
         {
             try
             {
                 if (Geschoßen[f + 1][g] == Geschoßen[f][g])
                 {
                     try { if (Geschoßen[f - 1][g] == Geschoßen[f][g]) { Ver = runter(f - 1, g, Ver); } } catch (Exception) { }
                     try { if (Geschoßen[f + 2][g] == Geschoßen[f][g]) { Ver = hoch(f + 1, g, Ver); } } catch (Exception) { }
                 }
                 else if (Geschoßen[f + 1][g] == 0) { Ver.Add((f + 1) * 10 + g); }
             }
             catch (Exception) { }
             return Ver;
         }
         private List<int> runter(int f, int g, List<int> Ver)
         {
             try
             {
                 if (Geschoßen[f - 1][g] == Geschoßen[f][g])
                 {
                     try { if (Geschoßen[f + 1][g] == Geschoßen[f][g]) { Ver = hoch(f + 1, g, Ver); } } catch (Exception) { }
                     try { if (Geschoßen[f - 2][g] == Geschoßen[f][g]) { Ver = runter(f - 1, g, Ver); } } catch (Exception) { }
                 }
                 else if (Geschoßen[f - 1][g] == 0) { Ver.Add((f - 1) * 10 + g); }
             }
             catch (Exception) { }
             return Ver;
         }*/
    }
}
