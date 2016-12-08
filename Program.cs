using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA {
    class Program {
        static List<Echiquier> solutions;
        static Stopwatch sw1;
        static Stopwatch sw2;
        static int rollBack;
        static List<long> temps;

        static void Main(string[] args) {

           // Console.WriteLine("Taille d'échiquier");
            //int nb = Convert.ToInt32(Console.ReadLine());
            temps = new List<long>();
            solutions = new List<Echiquier>();
            sw1 = new Stopwatch();
            sw2 = new Stopwatch();
            rollBack = 0;
            Echiquier e;

            /* sw1.Start();
             sw2.Start();
             bruteForce(0, e);
             sw2.Stop();*/
            /*for (int i=7;i<1000;i++){
                sw1.Start();
                e = recuitSimule(i);
                sw1.Stop();
                Console.WriteLine(i + " " + sw1.ElapsedMilliseconds + " ms");
                sw1.Reset();
            }*/

            /* Echiquier e = new Echiquier(nb);
             bruteForce(0, e);
             Console.WriteLine("Une solution : " + sw1.ElapsedMilliseconds + "ms. Toutes les solutions : " + sw2.ElapsedMilliseconds + " ms. Nombre de rollbacks : " + rollBack);
             rollBack = 0;
             premierHeuristique(0, e);
             foreach(Echiquier eq in solutions) {
                 Console.WriteLine(eq);
             }
             Console.WriteLine(solutions.Count);
             Console.WriteLine("Une solution : " + sw1.ElapsedMilliseconds + "ms. Toutes les solutions : " + sw2.ElapsedMilliseconds + " ms. Nombre de rollbacks : " + rollBack);*/


            /* sw1.Start();
             Echiquier soluce = recuitSimule(nb);
             sw1.Stop();
             Console.WriteLine(soluce + "\n en : " + sw1.ElapsedMilliseconds + " ms");*/
            Console.WriteLine("Minimum de fonction : " + recuitSimule2(4.0));
            Console.ReadLine();

        }

        static bool bruteForce(int ligne, Echiquier e) {
            bool ok = false;
            int i = 0;
            while (i < e.taille && ok == false) {
                Reine r = new Reine(ligne, i);
                if (r.posValid(e)) {
                    e.reines.Add(r);
                    if (r.x == e.taille - 1) {
                        if (sw1.IsRunning)
                            sw1.Stop();
                        ok = true;
                        solutions.Add(new Echiquier(e));
                    } else {
                        ok = bruteForce(ligne + 1, e);
                    }
                }
                if (ok == false) {
                    e.reines.Remove(r);
                    rollBack++;
                }
                i++;
            }
            return ok;
        }

        static bool premierHeuristique(int ligne, Echiquier e) {
            bool ok = false;
            int i = 0;
            int j = 0;
            int cpt = 0;
            Dictionary<int, List<int>> notes = new Dictionary<int, List<int>>();

            //Calcul des notes
            while (i < e.taille) {
                Reine r = new Reine(ligne, i);
                e.reines.Add(r);
                while (j < e.taille) {
                    Reine r2 = new Reine(ligne + 1, j);
                    if (r2.posValid(e)) {
                        cpt++;
                    }
                    if (notes.Keys.Contains(cpt)) {
                        notes[cpt].Add(j);
                    } else {
                        notes.Add(cpt, new List<int> { j });
                    }
                    e.reines.Remove(r2);
                    j++;
                }
                i++;
                e.reines.Remove(r);
            }

            List<int> sorted = notes.Keys.ToList();
            sorted.Sort();
            sorted.Reverse();

            //Positionnement
            i = 0;
            while (i < sorted.Count && ok == false) {
                foreach (int note in sorted) {
                    foreach (int pos in notes[note]) {
                        Reine r = new Reine(ligne, pos);
                        if (r.posValid(e)) {
                            e.reines.Add(r);
                            if (r.x == e.taille - 2) {
                                ok = bruteForce(ligne + 1, e);
                            } else {
                                ok = premierHeuristique(ligne + 1, e);
                            }
                        }
                        if (ok == false) {
                            e.reines.Remove(r);
                            rollBack++;
                        }
                        if (ok == true)
                            return ok;
                    }
                }
                i++;
            }
            return ok;
        }

        static Echiquier recuitSimule(int taille) {

            Echiquier e = new Echiquier(taille);
            List<int> possible = new List<int>();
            Random r = new Random();
            var temperature = 10.0;
            Reine re;
            int y;
            int tire;
            int r1;
            int r2;
            int rTemp;
            bool a = true;

            for (int i = 0; i < taille; i++) {
                possible.Add(i);
            }
            for (int i = 0; i < taille; i++) {
                tire = r.Next(0, possible.Count);
                y = possible[tire];
                possible.Remove(possible[tire]);
                re = new Reine(i, y);
                e.reines.Add(re);
            }

            while (e.getEnergie() != 0) {

                Echiquier eTemp = new Echiquier(e);

                r1 = r.Next(0, e.taille);
                r2 = r.Next(0, e.taille);
                rTemp = e.reines[r1].y;
                eTemp.reines[r1].y = eTemp.reines[r2].y;
                eTemp.reines[r2].y = rTemp;

                /* if (eTemp.getEnergie() < e.getEnergie())
                     e = eTemp;*/
                a = e.accepte(eTemp.getEnergie() - e.getEnergie(), temperature);
                if ( a) {
                    e = eTemp;
                }
                temperature *= 0.5;
                
            }
            Debug.WriteLine("e final "+e.getEnergie());
            return e;
        }

        static double recuitSimule2 (double X0) {
            Random r = new Random();
            double X = X0;
            double T = 100.0;
            double Rt = 0.5;
            double Nt = 100;/* iterations à chaque pas de temperature. */
            double Y = 0.0;
            while (T > 0.87) {
                for (int m = 0 ; m < Nt ; m++) {
                    if (r.Next() > 0.5) {
                        Y = X + r.Next();
                    }
                    else {
                        Y = X - r.Next();
                    }
                    double dF = F(Y) - F(X);
                    if (accepte(dF,T)) {
                        X = Y;
                    }
                }
                T = Rt * T;
            }
            return X;
        }
        static bool accepte (double dF,double T) {
            Random r = new Random();
            if (dF >= 0) {
                double A = Math.Exp(-dF / T);
                if (r.NextDouble() >= A) {
                    return false;
                }
            }
            return true;
        }

        static double F (double x) {
            return 10 * Math.Sin((0.3 * x) * Math.Sin(1.3 * Math.Pow(x,2) + 0.00001 * Math.Pow(x,4) + 0.2 * x + 80));
        }
    }

}
