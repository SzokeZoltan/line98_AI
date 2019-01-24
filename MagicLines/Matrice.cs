using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicLines
{
    class Matrice
    {
        private int[,] mat, mat2;
        public int[,] pred;
        public int[,,,] pred_all;
        public int[,,] pred_colors;
        public int[] pred_all_coordinates;
        public float[] bonus_before;
        public float[,,,] bonus_after;
        public int[,,,] weight_array;
        public int[,] weight_actual;
        public bool[] layer_activated;
        public int casute_libere;
        Random r;
        Queue<int> Q;

        /// <summary>
        /// Plaseaza (maxim) 4 bile fictive //Helyezzen (maximum) 4 fiktív labdát
        /// </summary>
        public void PuneBileFictive()
        {
            int i, j, tip, adaugat;
            if (casute_libere <= 4)
                adaugat = casute_libere;
            else
                adaugat = 4;
            while (adaugat > 0)
            {
                i = r.Next(0, 9);
                j = r.Next(0, 9);
                while (mat[i, j] != 0)
                {
                    i = r.Next(0, 9);
                    j = r.Next(0, 9);
                }
                tip = r.Next(1, 8);
                mat[i, j] = -tip;
                adaugat--;
            }
        }

        /// <summary>
        /// Plaseaza 3 bile initiale la inceputul unui joc nou   //Helyezzen 3 golyót egy új játék elejére
        /// </summary>
        private void PuneBileInitiale()
        {
            int i, j, tip; // pozitia bilei si tipul ei   //golyós helyzet és típus
            while (casute_libere>78)
            {
                i = r.Next(0, 9);
                j = r.Next(0, 9);
                if (mat[i, j] == 0)
                {
                    tip = r.Next(1, 8);
                    mat[i, j] = tip;
                    casute_libere--;
                }
            }
            PuneBileFictive();
        }

        /// <summary>
        /// Reseteaza matricea pentru un joc nou //Állítsa vissza a mátrixot egy új játékhoz
        /// </summary>
        public void Initializare()
        {
            casute_libere = 81;
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    mat[i, j] = 0;
                    mat2[i, j] = 0;
                }
            }
            PuneBileInitiale();
        }

        /// <summary>
        /// Constructor pentru o matrice de joc //Építõ játékmátrixhoz
        /// </summary>
        public Matrice()
        {
            mat = new int[9, 9];
            mat2 = new int[9, 9];
            pred = new int[9, 9];
            pred_all = new int[9, 9, 9, 9];
            pred_all_coordinates = new int[4];
            pred_colors = new int[8, 9, 9];
            bonus_before = new float[1];         //a kiinduló bónusz értéket mentjük le ide.
            bonus_after = new float[9, 9, 9, 9]; //az összes lehetséges lépéshez le tudunk menteni összesített bónusz értéket.
            weight_array = new int[9, 9, 9, 9];
            weight_actual = new int[9, 9];
            layer_activated = new bool[10];
            layer_activated[0] = true;
            layer_activated[1] = true;
            layer_activated[2] = true;
            layer_activated[3] = true;
            layer_activated[4] = true;


            r = new Random();
            Q = new Queue<int>();
            Initializare();
        }

        public int this[int i, int j]
        {
            get
            {
                return mat[i, j];
            }
            set
            {
                mat[i, j] = value;
            }
        }

        /// <summary>
        /// Cauta daca exista drum liber intre (xstart, ystart) si (xfinal, yfinal) //Keresse meg, hogy van-e szabad út (xstart, ystart) és (xfinal, yfinal) között
        /// </summary>
        /// <param name="xstart"></param>
        /// <param name="ystart"></param>
        /// <param name="xfinal"></param>
        /// <param name="yfinal"></param>
        /// <returns></returns>
        public bool CautaDrum(int xstart, int ystart, int xfinal, int yfinal)
        {
            int i, j, tag, aux;
            aux = mat[xstart, ystart];  //start koordinátán lévő szín mentése
            mat[xstart, ystart] = 0;    //start koordináta érték 0-ba állítása
            for (i = 0; i < 9; ++i)     //csinálunk egy pred mátrixot és ott mindent inicializálunk -1-el
            {
                for (j = 0; j < 9; ++j)
                {
                    pred[i, j] = -1;
                }
            }
            Q.Clear();
            Q.Enqueue(xfinal * 10 + yfinal);  //x és y koordináta lementése fura módon..
            pred[xfinal, yfinal] = -2;        //a cél koordinátánál az értéket -2-re állítjuk
            while (Q.Count > 0)
            {
                tag = Q.Dequeue();
                i = tag / 10;
                j = tag % 10;  //x és y visszakonvertálása
                // sus //fel
                if (i - 1 >= 0 && mat[i - 1, j] <= 0 && pred[i - 1, j] == -1) //ha még tudunk felfelé menni, és a mátrix értéke negatív (fiktív labda) vagy nulla (üres mező), és a pred mátrix init értékben van. akkor:
                {
                    pred[i - 1, j] = tag; //akkor a fenti érték legyen az a koordináta ahonnan jöttünk!
                    Q.Enqueue((i - 1) * 10 + j); //a kiindulási értéket eltoljuk egyel fentebb, és eltároljuk ebben a listában..
                }
                // jos //le
                if (i + 1 < 9 && mat[i + 1, j] <= 0 && pred[i + 1, j] == -1)
                {
                    pred[i + 1, j] = tag;
                    Q.Enqueue((i + 1) * 10 + j);
                }
                // dreapta //jobbra
                if (j + 1 < 9 && mat[i, j + 1] <= 0 && pred[i, j + 1] == -1)
                {
                    pred[i, j + 1] = tag;
                    Q.Enqueue(i * 10 + j + 1);
                }
                // stanga //balra
                if (j - 1 >= 0 && mat[i, j - 1] <= 0 && pred[i, j - 1] == -1)
                {
                    pred[i, j - 1] = tag;
                    Q.Enqueue(i * 10 + j - 1);
                }
            }
            mat[xstart, ystart] = aux;
            if (pred[xstart, ystart] == -1)  //ha maradt az init érték akkor nem lehet átjárni, egyébként igen.
                return false;
            return true;
        }

        /// <summary>
        /// //Keresse meg, hogy van-e szabad út ...
        /// </summary>
        /// <returns></returns>
        public bool CautaDrum2()
        {
             //lehetséges lépések kiszámítása
            int xstart, ystart, xfinal, yfinal;
            int i, j, tag, aux;
            for (int ia = 0; ia < 9; ++ia)
            {
                for (int ja = 0; ja < 9; ++ja)
                {
                    if (mat[ia, ja] > 0)
                    { 
                        xstart = ia;
                        ystart = ja;
                        xfinal = xstart;
                        yfinal = ystart;
                        //int i, j, tag, aux;
                        aux = mat[xstart, ystart];  //start koordinátán lévő szín mentése
                        mat[xstart, ystart] = 0;    //start koordináta érték 0-ba állítása
                        for (i = 0; i < 9; ++i)     //csinálunk egy pred mátrixot és ott mindent inicializálunk -1-el
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                pred_all[ia, ja, i, j] = -1;
                            }
                        }
                        Q.Clear();
                        Q.Enqueue(xfinal * 10 + yfinal);  //x és y koordináta lementése fura módon..
                        pred_all[ia, ja, xfinal, yfinal] = -2;        //a cél koordinátánál az értéket -2-re állítjuk
                        while (Q.Count > 0)
                        {
                            tag = Q.Dequeue();
                            i = tag / 10;
                            j = tag % 10;  //x és y visszakonvertálása
                            // sus //fel
                            if (i - 1 >= 0 && mat[i - 1, j] <= 0 && pred_all[ia, ja, i - 1, j] == -1) //ha még tudunk felfelé menni, és a mátrix értéke negatív (fiktív labda) vagy nulla (üres mező), és a pred mátrix init értékben van. akkor:
                            {
                                pred_all[ia, ja, i - 1, j] = 100; //akkor a fenti érték legyen az a koordináta ahonnan jöttünk!
                                Q.Enqueue((i - 1) * 10 + j); //a kiindulási értéket eltoljuk egyel fentebb, és eltároljuk ebben a listában..
                            }
                            // jos //le
                            if (i + 1 < 9 && mat[i + 1, j] <= 0 && pred_all[ia, ja, i + 1, j] == -1)
                            {
                                pred_all[ia, ja, i + 1, j] = 100;
                                Q.Enqueue((i + 1) * 10 + j);
                            }
                            // dreapta //jobbra
                            if (j + 1 < 9 && mat[i, j + 1] <= 0 && pred_all[ia, ja, i, j + 1] == -1)
                            {
                                pred_all[ia, ja, i, j + 1] = 100;
                                Q.Enqueue(i * 10 + j + 1);
                            }
                            // stanga //balra
                            if (j - 1 >= 0 && mat[i, j - 1] <= 0 && pred_all[ia, ja, i, j - 1] == -1)
                            {
                                pred_all[ia, ja, i, j - 1] = 100;
                                Q.Enqueue(i * 10 + j - 1);
                            }
                        }
                        mat[xstart, ystart] = aux;
                    }
                    else
                    {
                        for (i = 0; i < 9; ++i)     //csinálunk egy pred mátrixot és ott mindent inicializálunk -1-el
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                pred_all[ia, ja, i, j] = -1;
                            }
                        }
                    }
                }
            }


            //init array
            Array.Clear(bonus_after, 0, bonus_after.Length);

            //számoljuk meg hány üres cella van kezdetben!  bonus before generálása
            if (true)
            {
                bonus_before[0] = 0;
                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        if (mat[ia, ja] <= 0)
                        {
                            bonus_before[0] += 1;
                        }
                    }
                }
            }


            //végigjátszuk az összes lehetséges lépést és kiszámoljuk hozzá, hogy hány üres cella lenne az adott lépés kiértékelése után,
            //az erenymént tömbben ( bonus_after[] ) tároljuk, ez a réteg csak azokat a lépéseket értékeli pozitívan amelyek végrehajtása után üres cellák keletkeznek, tehát minimum 4db egymás melletti szín esetén..
            if (layer_activated[0] == true)
            {
                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                if (pred_all[ia, ja, i, j] > 0)  //soron következő lehetséges lépés
                                {
                                    Array.Copy(mat, 0, mat2, 0, mat.Length);

                                    mat2[i, j] = mat2[ia, ja];
                                    mat2[ia, ja] = 0;

                                    Sunt5InLinie4(i, j);

                                    //bonus_after[ia, ja, i, j] = 0-bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)
                                    for (int ib = 0; ib < 9; ++ib)
                                    {
                                        for (int jb = 0; jb < 9; ++jb)
                                        {
                                            if (mat2[ib, jb] <= 0)
                                            {
                                                bonus_after[ia, ja, i, j] += 1;
                                            }
                                        }
                                    }
                                    bonus_after[ia, ja, i, j] -= bonus_before[0];

                                }
                            }
                        }
                    }
                }
            }




            if(false) //félkész kódrész.. törölhető..
            {
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        if (mat2[i, j] > 0)
                        {
                            //mat2[i, j] = -mat2[i, j];
                            //P[i, j].BackgroundImage = img[M[i, j]];
                            //P[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                            //M.casute_libere--;
                            if (Sunt5InLinie(i, j)) // s-au facut macar 5 in linie cu noua bila // legalább 5 az új labdával összhangban készült
                            {
                                //Explodeaza();
                                int i2; //contor = 0;
                                for (i2 = 0; i2 < 9; ++i2)
                                {
                                    for (j = 0; j < 9; ++j)
                                    {
                                        if (mat2[i2, j] == -1000)
                                        {
                                            //contor++;
                                            mat2[i2, j] = 0;
                                            //P[i, j].BackgroundImage = null;
                                            //mat2.casute_libere++;
                                        }
                                    }
                                }
                                //textBoxScor.Text = scor.ToString();
                            }
                        }
                    }
                }
            }

            //végigmegyünk az összes lehetséges lépésen, minden lépés start x,y-ban lévő golyót ellenőrizzük 3db színre 
            // (törli a metódus a matrixból az elemeket) és ezután összeszámoljuk hány üres cella van.
            //(kivonjuk az eredeti mennyiségből így megkapjuk a különbséget)
            //Ha ebben az esetben üres cellák keletkeztek akkor büntető bónuszt kell hozzáadni! 
            //(mivel ezzel a lépéssel egy 3 egymás mellett lévő sorozat egyik elemét vennénk ki)
            if (layer_activated[1] == true)
            {
                //számoljuk meg hány üres cella van.  bonus before generálása
                if (true)
                {
                    Array.Copy(mat, 0, mat2, 0, mat.Length); //lemásoljuk a mat mátrixot, hogy dolgozhassunk rajta..
                    bonus_before[0] = 0;
                    for (int ia = 0; ia < 9; ++ia)
                    {
                        for (int ja = 0; ja < 9; ++ja)
                        {
                            if (mat2[ia, ja] <= 0)
                            {
                                bonus_before[0] += 0.3f;
                            }
                        }
                    }
                }

                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                if (pred_all[ia, ja, i, j] > 0)  //soron következő lehetséges lépés
                                {
                                    Array.Copy(mat, 0, mat2, 0, mat.Length); //mat2 := mat

                                    //mat2[i, j] = mat2[ia, ja];
                                    //mat2[ia, ja] = 0;

                                    Sunt5InLinie3(ia, ja);

                                    //bonus_after[ia, ja, i, j] = 0 - bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)
                                    for (int ib = 0; ib < 9; ++ib)
                                    {
                                        for (int jb = 0; jb < 9; ++jb)
                                        {
                                            if (mat2[ib, jb] <= 0)
                                            {
                                                bonus_after[ia, ja, i, j] -= 0.3f;
                                            }
                                        }
                                    }

                                    bonus_after[ia, ja, i, j] += bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)

                                }
                            }
                        }
                    }
                }
            }


            //végigmegyünk az összes lehetséges lépésen, minden lépés target x,y-ban lévő golyót ellenőrizzük 3db színre 
            // (törli a metódus a matrixból az elemeket) és ezután összeszámoljuk hány üres cella van.
            //(kivonjuk az eredeti mennyiségből így megkapjuk a különbséget)
            //Ha ebben az esetben üres cellák keletkeztek akkor bónuszt kell hozzáadni! 
            //
            if (layer_activated[2] == true)
            {
                //számoljuk meg hány üres cella van.  bonus before generálása
                if (true)
                {
                    Array.Copy(mat, 0, mat2, 0, mat.Length); //lemásoljuk a mat mátrixot, hogy dolgozhassunk rajta..
                    bonus_before[0] = 0;
                    for (int ia = 0; ia < 9; ++ia)
                    {
                        for (int ja = 0; ja < 9; ++ja)
                        {
                            if (mat2[ia, ja] <= 0)
                            {
                                bonus_before[0] += 0.3f;
                            }
                        }
                    }
                }

                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                if (pred_all[ia, ja, i, j] > 0)  //soron következő lehetséges lépés
                                {
                                    Array.Copy(mat, 0, mat2, 0, mat.Length); //mat2 := mat

                                    mat2[i, j] = mat2[ia, ja];
                                    mat2[ia, ja] = 0;

                                    Sunt5InLinie3(i, j);

                                    //bonus_after[ia, ja, i, j] = 0 - bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)
                                    for (int ib = 0; ib < 9; ++ib)
                                    {
                                        for (int jb = 0; jb < 9; ++jb)
                                        {
                                            if (mat2[ib, jb] <= 0)
                                            {
                                                bonus_after[ia, ja, i, j] += 0.3f;
                                            }
                                        }
                                    }

                                    bonus_after[ia, ja, i, j] -= bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)

                                }
                            }
                        }
                    }
                }
            }

            //ugyan ez csak 2 elemmel:
            //végigmegyünk az összes lehetséges lépésen, minden lépés start x,y-ban lévő golyót ellenőrizzük 3db színre 
            // (törli a metódus a matrixból az elemeket) és ezután összeszámoljuk hány üres cella van.
            //(kivonjuk az eredeti mennyiségből így megkapjuk a különbséget)
            //Ha ebben az esetben üres cellák keletkeztek akkor büntető bónuszt kell hozzáadni! 
            //(mivel ezzel a lépéssel egy 3 egymás mellett lévő sorozat egyik elemét vesszük ki)
            if (layer_activated[3] == true)
            {
                //számoljuk meg hány üres cella van.  bonus before generálása
                if (true)
                {
                    Array.Copy(mat, 0, mat2, 0, mat.Length); //lemásoljuk a mat mátrixot, hogy dolgozhassunk rajta..
                    bonus_before[0] = 0;
                    for (int ia = 0; ia < 9; ++ia)
                    {
                        for (int ja = 0; ja < 9; ++ja)
                        {
                            if (mat2[ia, ja] <= 0)
                            {
                                bonus_before[0] += 0.1f;
                            }
                        }
                    }
                }

                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                if (pred_all[ia, ja, i, j] > 0)  //soron következő lehetséges lépés
                                {
                                    Array.Copy(mat, 0, mat2, 0, mat.Length); //mat2 := mat

                                    //mat2[i, j] = mat2[ia, ja];
                                    //mat2[ia, ja] = 0;

                                    Sunt5InLinie2(ia, ja);

                                    //bonus_after[ia, ja, i, j] = 0 - bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)
                                    for (int ib = 0; ib < 9; ++ib)
                                    {
                                        for (int jb = 0; jb < 9; ++jb)
                                        {
                                            if (mat2[ib, jb] <= 0)
                                            {
                                                bonus_after[ia, ja, i, j] -= 0.1f;
                                            }
                                        }
                                    }

                                    bonus_after[ia, ja, i, j] += bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)

                                }
                            }
                        }
                    }
                }
            }

            //ugyan ez csak 2 elemmel:
            //végigmegyünk az összes lehetséges lépésen, minden lépés target x,y-ban lévő golyót ellenőrizzük 3db színre 
            // (törli a metódus a matrixból az elemeket) és ezután összeszámoljuk hány üres cella van.
            //(kivonjuk az eredeti mennyiségből így megkapjuk a különbséget)
            //Ha ebben az esetben üres cellák keletkeztek akkor bónuszt kell hozzáadni! 
            //
            if (layer_activated[4] == true)
            {
                //számoljuk meg hány üres cella van.  bonus before generálása
                if (true)
                {
                    Array.Copy(mat, 0, mat2, 0, mat.Length); //lemásoljuk a mat mátrixot, hogy dolgozhassunk rajta..
                    bonus_before[0] = 0;
                    for (int ia = 0; ia < 9; ++ia)
                    {
                        for (int ja = 0; ja < 9; ++ja)
                        {
                            if (mat2[ia, ja] <= 0)
                            {
                                bonus_before[0] += 0.1f;
                            }
                        }
                    }
                }

                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                if (pred_all[ia, ja, i, j] > 0)  //soron következő lehetséges lépés
                                {
                                    Array.Copy(mat, 0, mat2, 0, mat.Length); //mat2 := mat

                                    mat2[i, j] = mat2[ia, ja];
                                    mat2[ia, ja] = 0;

                                    Sunt5InLinie2(i, j);

                                    //bonus_after[ia, ja, i, j] = 0 - bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)
                                    for (int ib = 0; ib < 9; ++ib)
                                    {
                                        for (int jb = 0; jb < 9; ++jb)
                                        {
                                            if (mat2[ib, jb] <= 0)
                                            {
                                                bonus_after[ia, ja, i, j] += 0.1f;
                                            }
                                        }
                                    }

                                    bonus_after[ia, ja, i, j] -= bonus_before[0]; //azért vonom ki a bonus_before értéket, hogy ne kapjak csak azért több bónuszt mert a pályán még kezdetben sok az üres mező. (ezzel elnyomnám a többi, nem pálya telítettség alapú bónusz pontokat) egyféle standardizálás (0 várható értékünk van amennyiben nem kapunk bónuszt.)

                                }
                            }
                        }
                    }
                }
            }


            if (layer_activated[5] == true) //weight array test
            {
                //init array
                Array.Clear(weight_array, 0, weight_array.Length);
                int[,] testCsv = new int[200, 9];
                testCsv = LoadCsv135Row(@"c:\temp\test.csv"); //betölti csv fájlból a 9x9-es mátrixok elemeit
                int number_of_matrix = 0;
                int matrix_offset = 9;

                for (int g = 0; g < 5; ++g)
                {
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            weight_array[0, g, i, j] = testCsv[i + matrix_offset * number_of_matrix, j];
                        }
                    }
                    number_of_matrix++;
                }

                for (int g = 1; g < 5; ++g)
                {
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            weight_array[1, g, i, j] = testCsv[i + matrix_offset * number_of_matrix, j];
                        }
                    }
                    number_of_matrix++;
                }

                for (int g = 2; g < 5; ++g)
                {
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            weight_array[2, g, i, j] = testCsv[i + matrix_offset * number_of_matrix, j];
                        }
                    }
                    number_of_matrix++;
                }

                for (int g = 3; g < 5; ++g)
                {
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            weight_array[3, g, i, j] = testCsv[i + matrix_offset * number_of_matrix, j];
                        }
                    }
                    number_of_matrix++;
                }
                for (int g = 4; g < 5; ++g)
                {
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            weight_array[4, g, i, j] = testCsv[i + matrix_offset * number_of_matrix, j];
                        }
                    }
                    number_of_matrix++;
                }

                int[,] source_array = new int[9, 9];
                int[,] tranformed_array = new int[9, 9];

                //////// TRANSPONE all /////////////
                //create source array  1,0 = transpose(0,1)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[0, 1, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[1, 0, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  2,0 = transpose(0,2)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[0, 2, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[2, 0, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  3,0 = transpose(0,3)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[0, 3, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[3, 0, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  4,0 = transpose(0,4)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[0, 4, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[4, 0, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  2,1 = transpose(1,2)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[1, 2, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[2, 1, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  3,1 = transpose(1,3)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[1, 3, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[3, 1, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  4,1 = transpose(1,4)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[1, 4, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[4, 1, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  3,2 = transpose(2,3)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[2, 3, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[3, 2, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  4,2 = transpose(2,4)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[2, 4, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[4, 2, i, j] = tranformed_array[i, j];
                    }
                }

                //create source array  4,3 = transpose(3,4)
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        source_array[i, j] = weight_array[3, 4, i, j];
                    }
                }
                //make transformation
                tranformed_array = transpose(source_array);
                //save to weight array
                for (i = 0; i < 9; ++i)
                {
                    for (j = 0; j < 9; ++j)
                    {
                        weight_array[4, 3, i, j] = tranformed_array[i, j];
                    }
                }

                //////// VERTICAL MIRROR all /////////////
                //create source array  0,8 = transpose(0,0)
                for (int vmirrorR = 0; vmirrorR < 5; ++vmirrorR)
                {
                    for (int vmirrorC = 0; vmirrorC < 4; ++vmirrorC)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                source_array[i, j] = weight_array[vmirrorR, vmirrorC, i, j];
                            }
                        }
                        //make transformation
                        tranformed_array = MirrorHorisontal(source_array);
                        //save to weight array
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                weight_array[vmirrorR, 8 - vmirrorC, i, j] = tranformed_array[i, j];
                            }
                        }
                    }
                }

                //////// HORISONTAL MIRROR all /////////////
                //create source array  0,8 = MirrorHorisontal(0,0)
                for (int vmirrorR = 0; vmirrorR < 4; ++vmirrorR)
                {
                    for (int vmirrorC = 0; vmirrorC < 9; ++vmirrorC)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                source_array[i, j] = weight_array[vmirrorR, vmirrorC, i, j];
                            }
                        }
                        //make transformation
                        tranformed_array = MirrorVertical(source_array);
                        //save to weight array
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                weight_array[8 - vmirrorR, vmirrorC, i, j] = tranformed_array[i, j];
                            }
                        }
                    }
                }


                using (StreamWriter outfile = new StreamWriter(@"C:\Temp\weight_matrixs.csv"))
                {
                    for (int row = 0; row < 9; row++)
                    {
                        for (int col = 0; col < 9; col++) //következő matrix
                        {
                            for (int x = 0; x < 9; x++) //sor
                            {
                                string content = "";
                                for (int y = 0; y < 9; y++) //elem
                                {
                                    if (y == 8)
                                    {
                                        content += weight_array[row, col, x, y].ToString();
                                    }
                                    else
                                    {
                                        content += weight_array[row, col, x, y].ToString() + "\t";
                                    }
                                }
                                outfile.WriteLine(content);
                            }
                            outfile.WriteLine("");
                        }
                        //outfile.WriteLine("");
                    }
                }



                if (false) // OLD test code
                {
                    int[,] test = new int[9, 9];
                    int[,] testtranspose = new int[9, 9];
                    int[,] testMirrorHorisontal = new int[9, 9];
                    int[,] testMirrorVertical = new int[9, 9];
                    //int[,] testCsv = new int[200, 9];

                    testtranspose = transpose(test);
                    testMirrorHorisontal = MirrorHorisontal(test);
                    testMirrorVertical = MirrorVertical(test);

                    using (StreamWriter outfile = new StreamWriter(@"C:\Temp\testtranspose.csv"))
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            string content = "";
                            for (int y = 0; y < 9; y++)
                            {
                                if (y == 8)
                                {
                                    content += testtranspose[x, y].ToString();
                                }
                                else
                                {
                                    content += testtranspose[x, y].ToString() + "\t";
                                }
                            }
                            outfile.WriteLine(content);
                        }
                    }

                    using (StreamWriter outfile = new StreamWriter(@"C:\Temp\testMirrorHorisontal.csv"))
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            string content = "";
                            for (int y = 0; y < 9; y++)
                            {
                                if (y == 8)
                                {
                                    content += testMirrorHorisontal[x, y].ToString();
                                }
                                else
                                {
                                    content += testMirrorHorisontal[x, y].ToString() + "\t";
                                }
                            }
                            outfile.WriteLine(content);
                        }
                    }

                    using (StreamWriter outfile = new StreamWriter(@"C:\Temp\testMirrorVertical.csv"))
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            string content = "";
                            for (int y = 0; y < 9; y++)
                            {
                                if (y == 8)
                                {
                                    content += testMirrorVertical[x, y].ToString();
                                }
                                else
                                {
                                    content += testMirrorVertical[x, y].ToString() + "\t";
                                }

                            }
                            outfile.WriteLine(content);
                        }
                    }



                    testCsv = LoadCsv135Row(@"c:\temp\test.csv"); //betölti csv fájlból a 9x9-es mátrix elemeit
                }

            }

            //use weight array

            if (true)
            {
                Array.Clear(weight_actual, 0, weight_actual.Length);
                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                if (pred_all[ia, ja, i, j] == 100)
                                {
                                    ;
                                }
                            }
                        }
                    }
                }
            }


            //az utolsó valós lépés lekérdezése
            for (int ia = 0; ia < 9; ++ia)
            {
                for (int ja = 0; ja < 9; ++ja)
                {
                    for (i = 0; i < 9; ++i)
                    {
                        for (j = 0; j < 9; ++j)
                        {
                            if (pred_all[ia, ja, i, j] >= 0)
                            {
                                pred_all_coordinates[0] = ia;
                                pred_all_coordinates[1] = ja;
                                pred_all_coordinates[2] = i;
                                pred_all_coordinates[3] = j;
                            }
                        }
                    }
                }
            }


            //search maximum bonus after value and save coordinates..
            if (true)
            {
                float maximum;
                float maximum_last;
                maximum_last = 0;
                maximum = 0;
                for (int ia = 0; ia < 9; ++ia)
                {
                    for (int ja = 0; ja < 9; ++ja)
                    {
                        for (i = 0; i < 9; ++i)
                        {
                            for (j = 0; j < 9; ++j)
                            {
                                maximum = bonus_after[ia, ja, i, j];
                                if (maximum > maximum_last )
                                {
                                    maximum_last = maximum;
                                    pred_all_coordinates[0] = ia;
                                    pred_all_coordinates[1] = ja;
                                    pred_all_coordinates[2] = i;
                                    pred_all_coordinates[3] = j;
                                }
                            }
                        }
                    }
                }
             }




            return true;
        }

        // Load a CSV file into an array of rows and columns.
        // Assume there may be blank lines but every line has
        // the same number of fields.
        private int[,] LoadCsv(string filename)
        {
            // Get the file's text.
            string whole_file = System.IO.File.ReadAllText(filename);

            // Split into lines.
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split('\t').Length;

            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split('\t');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }

            //convert to number
            int[,] numbers = new int[9, 9];
            for (int r = 0; r < num_rows; r++)
            {
                for (int c = 0; c < num_cols; c++)
                {
                    numbers[r, c] = Convert.ToInt16(values[r, c]);
                }
            }


            // Return the values.
            return numbers;
        }

        // Load a CSV file into an array of rows and columns.
        // Assume there may be blank lines but every line has
        // the same number of fields.
        private int[,] LoadCsv135Row(string filename)
        {
            // Get the file's text.
            string whole_file = System.IO.File.ReadAllText(filename);

            // Split into lines.
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split('\t').Length;

            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split('\t');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }

            //convert to number
            int[,] numbers = new int[num_rows, num_cols];
            for (int r = 0; r < num_rows; r++)
            {
                for (int c = 0; c < num_cols; c++)
                {
                    numbers[r, c] = Convert.ToInt16(values[r, c]);
                }
            }


            // Return the values.
            return numbers;
        }


        public int[,] transpose(int[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            int[,] result = new int[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        public int[,] MirrorHorisontal(int[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            int[,] result = new int[h, w];

            for (int i = 0; i<w; i++)
            {
                for (int j = 0; j<h; j++)
                {
                    result[i, j] = matrix[i, h - 1 - j];
                }
        }

            return result;
        }

        public int[,] MirrorVertical(int[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            int[,] result = new int[h, w];

            for (int i = 0; i<w; i++)
            {
                for (int j = 0; j<h; j++)
                {
                    result[i, j] = matrix[w - 1 - i, j];
                }
            }

            return result;
        }

        public bool Sunt5InLinie(int x, int y)
        {
            int i, contor, tip;
            bool gasit = false;
            tip = mat[x, y];  //elmenti a mátrix aktuális értékét mert, szín kereséskor az azonos színeket (4db legalább) átírja -1000-re de mivel minden irányban külön van az ellenőrzés ezért ez gondot okozna. Ezért minden irányban amikor végez akkor ezt visszaírja a mátrixba.. a végén pedig ha volt egyezést azt a mátix elemet is -1000-rel helyettesíti
            // 1
            i = 0; contor = -1;
            while (x + i < 9 && y + i < 9 && mat[x + i, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y - i >= 0 && mat[x - i, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y - i && y - i < 9 && mat[x - i, y - i] == tip)
                {
                    mat[x - i, y - i] = -1000;
                    i--;
                }
                mat[x, y] = tip;
                gasit = true;
            }
            // 2
            i = 0; contor = -1;
            while (x + i < 9 && y - i >= 0 && mat[x + i, y - i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y + i < 9 && mat[x - i, y + i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y + i && y + i < 9 && mat[x - i, y + i] == tip)
                {
                    mat[x - i, y + i] = -1000;
                    i--;
                }
                mat[x, y] = tip;
                gasit = true;
            }
            // 3
            i = 0; contor = -1;
            while (x + i < 9 && mat[x + i, y] == tip)
            {
                contor++;
                i++;
            }
            i = 0;;
            while (x - i >= 0 && mat[x - i, y] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= x - i && x - i < 9 && mat[x - i, y] == tip)
                {
                    mat[x - i, y] = -1000;
                    i--;
                }
                mat[x, y] = tip;
                gasit = true;
            }
            // 4
            i = 0; contor = -1;
            while (y + i < 9 && mat[x, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (y - i >= 0 && mat[x, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= y - i && y - i < 9 && mat[x, y - i] == tip)
                {
                    mat[x, y - i] = -1000;
                    i--;
                }
                mat[x, y] = tip;
                gasit = true;
            }
            if (gasit)
                mat[x, y] = -1000;
            return gasit;
        }

        public bool Sunt5InLinie4(int x, int y)
        {
            int i, contor, tip;
            bool gasit = false;
            tip = mat2[x, y];  //elmenti a mátrix aktuális értékét mert, szín kereséskor az azonos színeket (4db legalább) átírja -1000-re de mivel minden irányban külön van az ellenőrzés ezért ez gondot okozna. Ezért minden irányban amikor végez akkor ezt visszaírja a mátrixba.. a végén pedig ha volt egyezést azt a mátix elemet is -1000-rel helyettesíti
            // 1
            i = 0; contor = -1;
            while (x + i < 9 && y + i < 9 && mat2[x + i, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y - i >= 0 && mat2[x - i, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y - i && y - i < 9 && mat2[x - i, y - i] == tip)
                {
                    mat2[x - i, y - i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 2
            i = 0; contor = -1;
            while (x + i < 9 && y - i >= 0 && mat2[x + i, y - i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y + i < 9 && mat2[x - i, y + i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y + i && y + i < 9 && mat2[x - i, y + i] == tip)
                {
                    mat2[x - i, y + i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 3
            i = 0; contor = -1;
            while (x + i < 9 && mat2[x + i, y] == tip)
            {
                contor++;
                i++;
            }
            i = 0; ;
            while (x - i >= 0 && mat2[x - i, y] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= x - i && x - i < 9 && mat2[x - i, y] == tip)
                {
                    mat2[x - i, y] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 4
            i = 0; contor = -1;
            while (y + i < 9 && mat2[x, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (y - i >= 0 && mat2[x, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 4)
            {
                i--;
                while (0 <= y - i && y - i < 9 && mat2[x, y - i] == tip)
                {
                    mat2[x, y - i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            if (gasit)
                mat2[x, y] = -1000;

            int i2, j; //contor = 0;
            for (i2 = 0; i2 < 9; ++i2)
            {
                for (j = 0; j < 9; ++j)
                {
                    if (mat2[i2, j] == -1000)
                    {
                        //contor++;
                        mat2[i2, j] = 0;
                        //P[i, j].BackgroundImage = null;
                        //mat2.casute_libere++;
                    }
                }
            }

            return gasit;
        }

        public bool Sunt5InLinie3(int x, int y)
        {
            int i, contor, tip;
            bool gasit = false;
            tip = mat2[x, y];  //elmenti a mátrix aktuális értékét mert, szín kereséskor az azonos színeket (4db legalább) átírja -1000-re de mivel minden irányban külön van az ellenőrzés ezért ez gondot okozna. Ezért minden irányban amikor végez akkor ezt visszaírja a mátrixba.. a végén pedig ha volt egyezést azt a mátix elemet is -1000-rel helyettesíti
            // 1
            i = 0; contor = -1;
            while (x + i < 9 && y + i < 9 && mat2[x + i, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y - i >= 0 && mat2[x - i, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 3)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y - i && y - i < 9 && mat2[x - i, y - i] == tip)
                {
                    mat2[x - i, y - i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 2
            i = 0; contor = -1;
            while (x + i < 9 && y - i >= 0 && mat2[x + i, y - i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y + i < 9 && mat2[x - i, y + i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 3)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y + i && y + i < 9 && mat2[x - i, y + i] == tip)
                {
                    mat2[x - i, y + i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 3
            i = 0; contor = -1;
            while (x + i < 9 && mat2[x + i, y] == tip)
            {
                contor++;
                i++;
            }
            i = 0; ;
            while (x - i >= 0 && mat2[x - i, y] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 3)
            {
                i--;
                while (0 <= x - i && x - i < 9 && mat2[x - i, y] == tip)
                {
                    mat2[x - i, y] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 4
            i = 0; contor = -1;
            while (y + i < 9 && mat2[x, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (y - i >= 0 && mat2[x, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 3)
            {
                i--;
                while (0 <= y - i && y - i < 9 && mat2[x, y - i] == tip)
                {
                    mat2[x, y - i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            if (gasit)
                mat2[x, y] = -1000;

            int i2, j; //contor = 0;
            for (i2 = 0; i2 < 9; ++i2)
            {
                for (j = 0; j < 9; ++j)
                {
                    if (mat2[i2, j] == -1000)
                    {
                        //contor++;
                        mat2[i2, j] = 0;
                        //P[i, j].BackgroundImage = null;
                        //mat2.casute_libere++;
                    }
                }
            }

            return gasit;
        }

        public bool Sunt5InLinie2(int x, int y)
        {
            int i, contor, tip;
            bool gasit = false;
            tip = mat2[x, y];  //elmenti a mátrix aktuális értékét mert, szín kereséskor az azonos színeket (4db legalább) átírja -1000-re de mivel minden irányban külön van az ellenőrzés ezért ez gondot okozna. Ezért minden irányban amikor végez akkor ezt visszaírja a mátrixba.. a végén pedig ha volt egyezést azt a mátix elemet is -1000-rel helyettesíti
            // 1
            i = 0; contor = -1;
            while (x + i < 9 && y + i < 9 && mat2[x + i, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y - i >= 0 && mat2[x - i, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 2)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y - i && y - i < 9 && mat2[x - i, y - i] == tip)
                {
                    mat2[x - i, y - i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 2
            i = 0; contor = -1;
            while (x + i < 9 && y - i >= 0 && mat2[x + i, y - i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (x - i >= 0 && y + i < 9 && mat2[x - i, y + i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 2)
            {
                i--;
                while (0 <= x - i && x - i < 9 && 0 <= y + i && y + i < 9 && mat2[x - i, y + i] == tip)
                {
                    mat2[x - i, y + i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 3
            i = 0; contor = -1;
            while (x + i < 9 && mat2[x + i, y] == tip)
            {
                contor++;
                i++;
            }
            i = 0; ;
            while (x - i >= 0 && mat2[x - i, y] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 2)
            {
                i--;
                while (0 <= x - i && x - i < 9 && mat2[x - i, y] == tip)
                {
                    mat2[x - i, y] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            // 4
            i = 0; contor = -1;
            while (y + i < 9 && mat2[x, y + i] == tip)
            {
                contor++;
                i++;
            }
            i = 0;
            while (y - i >= 0 && mat2[x, y - i] == tip)
            {
                contor++;
                i++;
            }
            if (contor >= 2)
            {
                i--;
                while (0 <= y - i && y - i < 9 && mat2[x, y - i] == tip)
                {
                    mat2[x, y - i] = -1000;
                    i--;
                }
                mat2[x, y] = tip;
                gasit = true;
            }
            if (gasit)
                mat2[x, y] = -1000;

            int i2, j; //contor = 0;
            for (i2 = 0; i2 < 9; ++i2)
            {
                for (j = 0; j < 9; ++j)
                {
                    if (mat2[i2, j] == -1000)
                    {
                        //contor++;
                        mat2[i2, j] = 0;
                        //P[i, j].BackgroundImage = null;
                        //mat2.casute_libere++;
                    }
                }
            }

            return gasit;
        }

    }
}
