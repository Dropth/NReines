using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA {
    class Reine {
        public int x{get; set;}
        public int y{get; set;}

        public Reine(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public bool posValid(Echiquier e) {
            int[,] pos = new int[e.taille,e.taille];
            bool ok = true;
            foreach(Reine r in e.reines) {
                pos[r.x,r.y] = 1;
                if(r.x == x || r.y == y)
                    ok = false;
            }

            
            bool temp = true;
            int tempX=this.x;
            int tempY=this.y;
            
                //Diagonales
                if(ok == true) {
                    
                    //bas droite
                    if(this.x == e.taille-1 || this.y == e.taille-1)
                        temp = false;
                    while(temp == true && ok==true) {
                        tempX++;
                        tempY++;
                        if(pos[tempX,tempY]==1) {
                            ok = false;
                        }
                        if(tempX == e.taille-1 || tempY == e.taille-1)
                            temp = false;
                    }

                    //bas gauche
                    tempX = this.x;
                    tempY = this.y;
                    temp = true;
                    if(this.x == e.taille-1 || this.y == 0)
                        temp = false;
                    while(temp == true && ok==true) {
                        tempX++;
                        tempY--;
                        if(pos[tempX,tempY]==1) {
                            ok = false;
                        }
                        if(tempX == e.taille-1 || tempY == 0)
                            temp = false;
                    }

                    //haut droit
                    tempX = this.x;
                    tempY = this.y;
                    temp = true;
                    if(this.x == 0 || this.y == e.taille-1)
                        temp = false;
                    while(temp == true && ok==true) {
                        tempX--;
                        tempY++;
                        if(pos[tempX,tempY]==1) {
                            ok = false;
                        }
                        if(tempX == 0 || tempY == e.taille-1)
                            temp = false;
                    }

                    //haut gauche
                    tempX = this.x;
                    tempY = this.y;
                    temp = true;
                    if(this.x == 0 || this.y == 0)
                        temp = false;
                    while(temp == true && ok==true) {
                        tempX--;
                        tempY--;
                        if(pos[tempX,tempY]==1) {
                            
                            ok = false;
                        }
                        if(tempX == 0 || tempY == 0)
                            temp = false;
                    }
                }
                //Console.WriteLine(string.Join(",", e.reines));
            return ok;
        }


        public override String ToString() {
            return "Reine{" +
                    "x=" + x +
                    ", y=" + y +
                    '}';
        }
    }
}
