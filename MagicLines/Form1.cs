using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicLines
{
    public partial class Form1 : Form
    {
        Matrice M;
        Image[] img, imgmic;
        PictureBox[,] P;
        Timer t;
        Timer auto_trigger;
        Random r;
        int xclicked, yclicked, xstart, ystart, xfinal, yfinal, xrestant, yrestant, tiprestant;
        int scor;
        bool disableclick, sunet;
        //System.Media.SoundPlayer playerexplozie, playergameover, playernewgame, playeraplauze;

        public Form1()
        {
            InitializeComponent();
            M = new Matrice();

            img = new Image[8];
            imgmic = new Image[8];
            P = new PictureBox[9, 9];
            r = new Random();

            t = new Timer();
            t.Interval = 100; //100
            t.Tick += t_Tick;

            auto_trigger = new Timer();
            auto_trigger.Interval = 100;
            auto_trigger.Tick += auto_trigger_Tick;


            xclicked = yclicked = -1;
            scor = 0;
            xrestant = yrestant = tiprestant = -1;
            disableclick = true;

            //playerexplozie = new System.Media.SoundPlayer("explosion.wav");
            //playergameover = new System.Media.SoundPlayer("gameover.wav");
            //playernewgame = new System.Media.SoundPlayer("gametime.wav");
            //playeraplauze = new System.Media.SoundPlayer("applause.wav");
            //sunet = true;

            InitializarePanel();
            IncarcaPoze();
            PunePozeInitiale();
            //playernewgame.Play();
        }

        /// <summary>
        /// Consruieste panelul de joc //A játéktér inicializálása
        /// </summary>
        private void InitializarePanel()
        {
            panelJoc.Controls.Clear();
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    PictureBox p = new PictureBox();
                    p.Size = new Size(40, 40);
                    p.Location = new Point(i * 41, j * 41);
                    p.Parent = panelJoc;
                    p.BackColor = Color.Transparent;
                    p.Tag = i * 10 + j;
                    p.Click += p_Click;
                    panelJoc.Controls.Add(p);
                    P[i, j] = p;
                }
            }
        }

        /// <summary>
        /// Muta o bila
        /// </summary>
        private void MutaBila()
        {

            if (false) //zoli: disable animation
            {
                // M[xfinal, yfinal] = M[xstart, ystart];
                P[xfinal, yfinal].BackgroundImage = P[xstart, ystart].BackgroundImage;
                P[xstart, ystart].BackgroundImage = null;
                VerificaExplozie();
            }
            else
            {
                t.Start();
                auto_trigger.Start();
                disableclick = false;
            }


        }

        private void auto_trigger_Tick(object sender, EventArgs e)
        {
            if ( checkBox_autotrigger.Checked == true )
            {
                button1.PerformClick();
            }
        }

        /// <summary>
        /// La fiecare interval de timp bila se muta cu o pozitie, pana cand ajunge la destinatie
        /// Minden egyes időintervallumban a labda egy pozícióval mozog, amíg el nem éri az úticélt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t_Tick(object sender, EventArgs e)
        {
            Timer T = sender as Timer;
            t.Enabled = false;
            if (M.pred[xstart, ystart] == -2)
            {
                T.Stop();
                if(xrestant != -1)
                {
                    M[xrestant, yrestant] = tiprestant;
                    P[xrestant, yrestant].BackgroundImage = imgmic[-tiprestant];
                    P[xrestant, yrestant].BackgroundImageLayout = ImageLayout.Center;
                    xrestant = yrestant = tiprestant = -1;
                }
                VerificaExplozie();
                disableclick = true;
                return;
            }
            int i, j;
            i = M.pred[xstart, ystart] / 10;
            j = M.pred[xstart, ystart] % 10;
            P[i, j].BackgroundImage = P[xstart, ystart].BackgroundImage;
            P[i, j].BackgroundImageLayout = ImageLayout.Stretch;
            if (M[xstart, ystart] == 0)
                P[xstart, ystart].BackgroundImage = null;
            else
            {
                P[xstart, ystart].BackgroundImage = imgmic[-M[xstart, ystart]];
                P[xstart, ystart].BackgroundImageLayout = ImageLayout.Center;
            }
            xstart = i;
            ystart = j;
            t.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PictureBox p = sender as PictureBox;
            bool ok = M.CautaDrum2();
            

            p_Click(P[M.pred_all_coordinates[0], M.pred_all_coordinates[1]], null);
            p_Click(P[M.pred_all_coordinates[2], M.pred_all_coordinates[3]], null);

        }

        private void checkLayer1_CheckedChanged(object sender, EventArgs e)
        {
            M.layer_activated[0] = checkLayer1.Checked;
        }

        private void checkLayer2_CheckedChanged(object sender, EventArgs e)
        {
            M.layer_activated[1] = checkLayer2.Checked;
        }

        private void checkLayer3_CheckedChanged(object sender, EventArgs e)
        {
            M.layer_activated[2] = checkLayer3.Checked;
        }

        private void checkLayer4_CheckedChanged(object sender, EventArgs e)
        {
            M.layer_activated[3] = checkLayer4.Checked;
        }

        private void checkLayer5_CheckedChanged(object sender, EventArgs e)
        {
            M.layer_activated[4] = checkLayer5.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          t.Interval = Convert.ToInt16(comboBox1.Text);
        }

        private void checkBox_autotrigger_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Se face click pe o casuta //Kattintson egy dobozra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void p_Click(object sender, EventArgs e)
        {
            if (!disableclick)
                return;
            PictureBox p = sender as PictureBox;
            int i, j;
            i = (int)p.Tag / 10;
            j = (int)p.Tag % 10;
            if (M[i, j] > 0) // Kiválasztom a labdát
            {
                if (xclicked != -1)
                    P[xclicked, yclicked].BackColor = Color.Transparent;
                xclicked = i;
                yclicked = j;
                p.BackColor = Color.Gold;
               // bool ok = M.CautaDrum2(xclicked, yclicked, i, j);
            }
            else
            {
                if (xclicked == -1) // Nem volt választott golyó
                    return;
                // A labdát választottam, ezért megpróbálom mozgatni
                bool ok = M.CautaDrum(xclicked, yclicked, i, j);
                if (ok == false) // itt nincs lehetőség a kiválasztott golyóra
                    return;
                // mut bila selectata aici  //az itt kiválasztott golyó
                xstart = xclicked;
                ystart = yclicked;
                xfinal = i;
                yfinal = j;
                if (M[xfinal, yfinal] < 0 && M[xfinal, yfinal] != -1000) // aveam o bila fictiva acolo, deci trebuie sa o mut //Van egy kitalált labdám, ezért mozgatni kell
                {
                    int tip = M[xfinal, yfinal];
                    M[xfinal, yfinal] = M[xstart, ystart];
                    M[xstart, ystart] = 0;
                    P[xfinal, yfinal].BackgroundImage = null;
                    if (M.casute_libere <= 3) // atunci bila fictiva o mut in locul bilei pe care o mut acum //akkor a fiktív labda mozdítja őt ahelyett, hogy a labda most mozog
                    {
                        xrestant = xstart;
                        yrestant = ystart;
                        tiprestant = tip;
                    }
                    else
                    {
                        i = r.Next(0, 9);
                        j = r.Next(0, 9);
                        while (M[i, j] != 0 || (i == xstart && j == ystart))
                        {
                            i = r.Next(0, 9);
                            j = r.Next(0, 9);
                        }
                        M[i, j] = tip;
                        P[i, j].BackgroundImage = imgmic[-M[i, j]];
                        P[i, j].BackgroundImageLayout = ImageLayout.Center;
                    }
                }
                else
                {
                    M[xfinal, yfinal] = M[xstart, ystart];
                    M[xstart, ystart] = 0;
                }
                xclicked = yclicked = -1;
                P[xstart, ystart].BackColor = Color.Transparent;

                if (false) //zoli:disable animation
                {
                    P[xfinal, yfinal].BackgroundImage = P[xstart, ystart].BackgroundImage;
                    P[xstart, ystart].BackgroundImage = null;
                    VerificaExplozie();
                }
                else
                {
                    MutaBila();
                }
            }
        }

        /// <summary>
        /// Verifica dupa ce am pus bila noua daca se intampla ceva //Nézd meg, miután felhoztam az új labdát, ha valami történik
        /// </summary>
        private void VerificaExplozie()
        {
            int i, j;
            i = xfinal;
            j = yfinal;
            if (M.Sunt5InLinie(i, j)) // sunt cel putin 5 in linie //legalább 5 sor
            {
                Explodeaza();
                textBoxScor.Text = scor.ToString();
            }
            else // altfel mai adaug cele (maxim) 3 bile //egyébként hozzáadom a (maximum) 3 golyót
            {
                for(i = 0; i <9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        if (M[i, j] < 0 && M[i, j] != -1000)
                        {
                            M[i, j] = -M[i, j];
                            P[i, j].BackgroundImage = img[M[i, j]];
                            P[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                            M.casute_libere--;
                            if (M.Sunt5InLinie(i, j)) // s-au facut macar 5 in linie cu noua bila // legalább 5 az új labdával összhangban készült
                            {
                                Explodeaza();
                                textBoxScor.Text = scor.ToString();
                            }
                        }
                    }
                }
                if (M.casute_libere == 0) // tabla e plina, deci gata jocul //a tábla tele van, így a játék készen áll
                {
                    //if (sunet)
                    //    playergameover.Play();
                    this.Enabled = false;
                    Form2 f2 = new Form2(scor);
                    f2.ShowDialog();
                    this.Enabled = true;
                    buttonHighScores.PerformClick(); // afisez topul scorurilor dupa ce s-a salvat noul scor //Az új pontszám megmentése után megjelenítem a legmagasabb pontszámot
                    return;
                }
                else
                {
                    M.PuneBileFictive(); // mai adaug (maxim) 3 bile fictive  //add (maximum) 3 fiktív labdát
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            if (M[i, j] < 0)
                            {
                                P[i, j].BackgroundImage = imgmic[-M[i, j]];
                                P[i, j].BackgroundImageLayout = ImageLayout.Center;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sterge bile si actualizeaza scor //A golyók törlése és a pontszám frissítése
        /// </summary>
        private void Explodeaza()
        {
            //if (sunet)
            //    playerexplozie.Play();
            int i, j, contor = 0;
            for (i = 0; i < 9; ++i)
            {
                for (j = 0; j < 9; ++j)
                {
                    if (M[i, j] == -1000)
                    {
                        contor++;
                        M[i, j] = 0;
                        P[i, j].BackgroundImage = null;
                        M.casute_libere++;
                    }
                }
            }
            //scor += (contor - 3) * 5;
            if (contor == 4)
                scor += 12;
            if (contor == 5)
                scor += 20;
            if (contor == 6)
                scor += 30;
            if (contor == 7)
                scor += 42;
            /*
               Line of 4 balls scores 12 points -- 3 points for each deleted ball
               Line of 5 balls scores 20 points -- 4 points for each deleted ball
               Line of 6 balls scores 30 points -- 5 points for each deleted ball
               Line of 7 balls scores 42 points -- 6 points for each deleted ball
            */
        }


        /// <summary>
        /// Incarca pozele bilelor //Feltölti a labdákat (labdák képeit)
        /// </summary>
        private void IncarcaPoze()
        {
            for (int i = 1; i < 8; ++i)
            {
                img[i] = Image.FromFile("minge" + i + ".png");
                imgmic[i] = Image.FromFile("mingemica" + i + ".png");
            }
        }

        /// <summary>
        /// Seteaza pozele celor 3 bile initiale //Állítson be három képet az eredeti golyókról
        /// </summary>
        private void PunePozeInitiale()
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if(M[i,j] > 0) // bila reala //valódi labda
                    {
                        P[i, j].BackgroundImage = img[M[i, j]];
                        P[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    if (M[i, j] < 0) // bila fictiva //fiktív labdát
                    {
                        P[i, j].BackgroundImage = imgmic[-M[i, j]];
                        P[i, j].BackgroundImageLayout = ImageLayout.Center;
                    }
                }
            }
        }

        /// <summary>
        /// Porneste un joc nou //Indítson el egy új játékot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            M.Initializare();
            InitializarePanel();
            PunePozeInitiale();
            textBoxScor.Text = "0";
            scor = 0;
            //if (sunet)
            //    playernewgame.Play();
        }

        /// <summary>
        /// Inchide aplicatia //Zárja be az alkalmazást
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Afiseaza topul scorurilor //A legmagasabb pontszámot mutatja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHighScores_Click(object sender, EventArgs e)
        {
            //if (sunet)
            //    playeraplauze.Play();
            this.Enabled = false;
            Form3 f3 = new Form3();
            f3.ShowDialog();
            this.Enabled = true;
            //if (sunet)
            //    playeraplauze.Stop();
        }

        /// <summary>
        /// Afiseaza informatii despre joc //A játékinformációk megjelenítése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Form4 f4 = new Form4();
            f4.ShowDialog();
            this.Enabled = true;
        }

        /// <summary>
        /// Sunetul se poate dezactiva si activa //A hang kikapcsolható és bekapcsolható
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSunet_CheckedChanged(object sender, EventArgs e)
        {
            if (sunet)
                sunet = false;
            else
                sunet = true;
        }
    }
}
