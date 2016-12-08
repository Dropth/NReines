using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA {
    class Echiquier {
        public int taille { get; set; }
        public List<Reine> reines { get; set; }

        public Echiquier(int taille) {
            this.taille = taille;
            reines = new List<Reine>();
        }

        public Echiquier(Echiquier e) {
            this.taille = e.taille;
            reines = new List<Reine>();
            foreach(Reine r in e.reines) {
                reines.Add(new Reine(r.x, r.y));
            }
        }

        public override String ToString() {
            int[,] pos = new int[taille, taille];
            int i = 0;
            int j = 0;
            foreach(Reine r in reines) {
                pos[r.x, r.y] = 1;
            }

            String ret = "";
            for(int k = 0; k < taille; k++)
                ret += "*---";
            ret += "*\n";
            while(i < taille) {
                while(j < taille) {
                    ret += (pos[i, j] == 1) ? "* X " : "*   ";
                    j++;
                }
                j = 0;
                ret += "* \n";
                for(int k = 0; k < taille; k++)
                    ret += "*---";
                ret += "* \n";
                i++;
            }

            return ret;
        }

        public Echiquier symetrieV() {
            Echiquier ret = new Echiquier(taille);
            foreach(Reine r in reines) {
                ret.reines.Add(new Reine(taille - 1 - r.x, r.y));
            }
            return ret;
        }

        public int getEnergie() {
            int energie = 0;
            int[] pos = new int[taille];
            foreach(Reine r in reines) {
                pos[r.x] = r.y;
            }
            foreach (Reine r in reines) {

                //bas droite
                int tempX = r.x;
                int tempY = r.y;
                bool temp = true;
                if (r.x == taille - 1 || r.y == taille - 1) {
                    temp = false;
                }
                while (temp == true) {
                        tempX++;
                        tempY++;
                        if (pos[tempX] == tempY) {
                            energie++;
                        }
                        if (tempX == taille - 1 || tempY == taille - 1)
                            temp = false;
                    }

                //bas gauche
                tempX = r.x;
                tempY = r.y;
                temp = true;
                if (r.x == taille - 1 || r.y == 0) {
                    temp = false;
                }
                while (temp == true) {
                        tempX++;
                        tempY--;
                        if (pos[tempX] == tempY) {
                            energie++;
                        }
                        if (tempX == taille - 1 || tempY == 0)
                            temp = false;
                    }

                //haut droit
                tempX = r.x;
                tempY = r.y;
                temp = true;
                if (r.x == 0 || r.y == taille - 1) {
                    temp = false;
                }
                    while (temp == true) {
                        tempX--;
                        tempY++;
                        if (pos[tempX] == tempY) {
                            energie++;
                        }
                        if (tempX == 0 || tempY == taille-1)
                            temp = false;
                    }

                //haut gauche
                tempX = r.x;
                tempY = r.y;
                temp = true;
                if (r.x == 0 || r.y == 0) {
                    temp = false;
                }
                while (temp == true) {
                        tempX--;
                        tempY--;
                        if (pos[tempX] == tempY) {
                            energie++;
                        }
                        if (tempX == 0 || tempY == 0)
                            temp = false;
                    }
                }

            return energie;
        }

        public bool accepte(double dF, double T) {
            Random r = new Random();
            if (dF >= 0) {
                double A = Math.Exp(-dF / T);
                bool a = r.Next(0, 1) >= A;
                if (r.NextDouble() >= A) {
                    return false;
                }
            }
            return true;
        }
    }
}
