using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersPF
{
    class Pieza
    {
        // get; set;
        //Define una propiedad de lectura y escritura
        public string Tipo { get; set; }
        public bool EsVacia { get; set; }
        public bool EsBlanca { get; set; }
        public bool EsNegra { get; set; }
        public int PosY { get; set; }
        public int PosX { get; set; }
        public bool EsReina { get; set; }

        public Pieza(string tipo, System.Windows.Forms.Button button)
        {
            Tipo = tipo;
            if (Tipo == "Blanca")
            {
                EsNegra = false;
                EsBlanca = true;
                EsVacia = false;
            }
            else if (Tipo == "Negra")
            {
                EsNegra = true;
                EsBlanca = false;
                EsVacia = false;
            }
            else if (Tipo == "Vacia")
            {
                EsNegra = false;
                EsBlanca = false;
                EsVacia = true;
            }
            // Posición gráfica (matriz y,x) -> filas, columnas
            // filas primero ya que el tablero se maneja por filas y no columnas
            // cada casilla
            // se divide entre 100 para obtener la fila y columna, y por ende la casilla
            // en la cual se encuentra la pieza.
            // ejemplo pos(150, 250) == (1, 2)
            PosY = button.Location.Y / 100;
            PosX = button.Location.X / 100;
            EsReina = false; // No empieza como reina
        }
    }    
}
