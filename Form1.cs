using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersPF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        Pieza[,] Tablero = new Pieza[8, 8]; // tablero 8x8 (en matriz)
        Pieza pieza1; // blanca
        Pieza pieza2; // negra
        Button boton1; // nuevo juego
        Button boton2; // salir
        bool TurnoBlancas = true;
        bool UltimaJugada = false; // detect ultima jugada
        int Score_White=0, Score_Black=0; // contadores puntuaciones

        // reiniciar app
        private void Restart()
        {
            Application.Restart();
        }       
        private void Form1_Load(object sender, EventArgs e)
        {
            // mostrar splashcreen
            Form2 form2 = new Form2();
            form2.ShowDialog();
            // blancas inicia primero
            ScoreBlancas.BackColor = Color.Wheat; // indicar mediante color que es el turno de blancas
            // para cada boton en el form
            foreach (Button boton in Controls.OfType<Button>())
            {
                // si el boton esta en una de las 3 filas superiores es una ficha blanca
                if (boton.Location.Y == 0 || boton.Location.Y == 100 || boton.Location.Y == 200)
                {
                    Pieza pieza =  new Pieza("Blanca", boton); // asignar tipo
                    Tablero[pieza.PosY, pieza.PosX] = pieza; // añadir a tablero
                }
                // si esta en las 2 filas del centro, la casilla va vacia
                if (boton.Location.Y == 300 || boton.Location.Y == 400)
                {
                    Pieza pieza = new Pieza("Vacia", boton); // asignar tipo
                    Tablero[pieza.PosY, pieza.PosX] = pieza; // añadir a tablero
                }
                // si se encuentra en una de las 3 filas inferiores, en una ficha negra
                if(boton.Location.Y == 500 || boton.Location.Y == 600 || boton.Location.Y == 700)
                {
                    Pieza pieza = new Pieza("Negra", boton); // asignar tipo
                    Tablero[pieza.PosY, pieza.PosX] = pieza; // añadir a tablero
                }
            }
        }
        private void Comer(Pieza pieza)
        {
            string tipo = pieza.Tipo;
            // por cada casilla del tablero
            foreach(Button boton in Controls.OfType<Button>())
            {
                // si la pieza esta en la casilla seleecionada
                if (boton.Location.Y/100 == pieza.PosY && boton.Location.X/100 == pieza.PosX)
                {
                    // cambiar imagen
                    boton.Image = Properties.Resources.Checker_Wheat;
                }
                // marcar como vacia
                Tablero[pieza.PosY, pieza.PosX].EsVacia = true;
                Tablero[pieza.PosY, pieza.PosX].EsNegra = false;
                Tablero[pieza.PosY, pieza.PosX].EsBlanca = false;
                Tablero[pieza.PosY, pieza.PosX].Tipo = "Vacia";
            }
            // sumar puntos
            if (tipo == "Negra")
            {
                Score_White++;
                // actualizar scoreboard
                ScoreBlancas.Text = "Blancas: " + Score_White;
            }
            if (tipo == "Blanca")
            {
                Score_Black++;
                // actualizar scoreboard
                ScoreNegras.Text = "Negras: " + Score_Black;
            }
        }
        private void CoronarReina()
        {   
            // si la pieza negra se encuentra en la primera fila superior y no es reina
            if(pieza1.EsNegra && pieza1.PosY==0 && pieza1.EsReina == false)
            {
                // cambiar imagen
                boton2.Image = Properties.Resources.Black_Piece2_Queen;
                // marcar como riena
                pieza1.EsReina = true;
                // obtener ubicacion y actualizar tablero
                Tablero[pieza1.PosY, pieza1.PosX] = pieza1;
                // se hizo una jugada
                UltimaJugada = false;
            }
            // si la pieza blanca esta en la primera fila inferior y no es reina
            if(pieza1.EsBlanca && pieza1.PosY==7 && pieza1.EsReina == false)
            {
                // cambiar imagen
                boton2.Image = Properties.Resources.White_Piece_Queen;
                // marcar como reina
                pieza1.EsReina = true;
                // obtener ubicacion y actualizar tablero
                Tablero[pieza1.PosY, pieza1.PosX] = pieza1;
                // se hizo una jugada
                UltimaJugada = false;
            }
        }
        private void MoverPieza(string tipo)
        {
            // si la ficha es blanca y no es reina
            if(tipo == "Blanca" && pieza1.EsReina == false)
            {
                // cambiar imagen de la casilla anterior
                boton1.Image = Properties.Resources.Checker_Wheat;
                // cambiar imagen de la nueva casilla
                boton2.Image = Properties.Resources.White_Piece;
                // posiciones orginales
                int orx = pieza1.PosX;
                int ory = pieza1.PosY;
                // intercambiar posiciones
                pieza1.PosY = pieza2.PosY;
                pieza1.PosX = pieza2.PosX;
                pieza2.PosY = ory;
                pieza2.PosX = orx;
                // actualizar tablero
                Tablero[pieza2.PosY, pieza2.PosX] = pieza2;
                Tablero[pieza1.PosY, pieza1.PosX] = pieza1;
                // verificar si se puede convertir en reina
                CoronarReina();
            }
            else if (tipo == "Negra" && pieza1.EsReina == false)
            {
                boton1.Image = Properties.Resources.Checker_Wheat;
                boton2.Image = Properties.Resources.Black_Piece2;
                // posiciones orginales
                int orx = pieza1.PosX;
                int ory = pieza1.PosY;
                // intercambiar posiciones
                pieza1.PosY = pieza2.PosY;
                pieza1.PosX = pieza2.PosX;
                pieza2.PosY = ory;
                pieza2.PosX = orx;
                // actualizar tablero
                Tablero[pieza2.PosY, pieza2.PosX] = pieza2;
                Tablero[pieza1.PosY, pieza1.PosX] = pieza1;
                // verificar si se puede convertir en reina
                CoronarReina();
            }
            // si es blanca y reina
            else if (tipo == "Blanca" && pieza1.EsReina == true)
            {
                // cambiar imagen de la casilla anterior
                boton1.Image = Properties.Resources.Checker_Wheat;
                // cambiar imagen de la nueva casilla
                boton2.Image = Properties.Resources.White_Piece_Queen;
                // posiciones orginales
                int orx = pieza1.PosX;
                int ory = pieza1.PosY;
                // intercambiar posiciones
                pieza1.PosY = pieza2.PosY;
                pieza1.PosX = pieza2.PosX;
                pieza2.PosY = ory;
                pieza2.PosX = orx;
                // actualizar tablero
                Tablero[pieza2.PosY, pieza2.PosX] = pieza2;
                Tablero[pieza1.PosY, pieza1.PosX] = pieza1;
            }
            // si es negra y reina
            else if (tipo == "Negra" && pieza1.EsReina == true)
            {
                // cambiar imagen de la casilla anterior
                boton1.Image = Properties.Resources.Checker_Wheat;
                // cambiar imagen de la nueva casilla
                boton2.Image = Properties.Resources.Black_Piece2_Queen;
                // posiciones orginales
                int orx = pieza1.PosX;
                int ory = pieza1.PosY;
                // intercambiar posiciones
                pieza1.PosY = pieza2.PosY;
                pieza1.PosX = pieza2.PosX;
                pieza2.PosY = ory;
                pieza2.PosX = orx;
                // actualizar tablero
                Tablero[pieza2.PosY, pieza2.PosX] = pieza2;
                Tablero[pieza1.PosY, pieza1.PosX] = pieza1;
            }
        }
        private bool PuedeComer()
        {
            bool puede_comer = false;
            foreach (Pieza pieza in Tablero)
            {
                // si la pieza esta vacia
                if (pieza == null)
                {
                    // nada
                }
                // si no es el turno de las blancas
                else if (TurnoBlancas)
                {
                    // si la pieza es reina
                    if (pieza.EsReina)
                    {
                        if (pieza.EsBlanca)
                        {
                            // comer en diagonal hacia abajo
                            /*
                             * LOGICA:
                             * 
                             * La reina puede desplazarse a traves del tablero completo.
                             * 
                             * 7 - pieza.PosY representa la cantidad de filas que quedan por debajo de la pieza en el tablero, es
                             * decir, la distancia que hay desde la pieza hacia el borde inferior del tablero. 
                             * 7 - pieza.PosX representa la cantidad de columnas que quedan por la derecha de la pieza en el
                             * tablero, es decir, la distancia que hay desde la pieza hacia el borde derecho del tablero.
                             * 
                             * La condición 7 - pieza.PosY > 7 - pieza.PosX verifica si la pieza se encuentra más lejos del borde inferior que 
                             * del borde derecho, lo que significa que su posición en el tablero permite comer en diagonal hacia abajo.
                             * 
                             * Las condiciones adicionales pieza.PosX != 6, pieza.PosX != 7, pieza.PosY != 6 y pieza.PosY != 7 se aseguran de que la
                             * pieza no se encuentre en las últimas filas o columnas del tablero, ya que en ese caso no podría realizar el movimiento diagonal hacia abajo.
                             */
                            // derecha
                            if (7 - pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                bool imposible = false;
                                bool posible = false;
                                // mientras la distancia sea menor o igual a 7
                                for (int i = 1; i <= 7 - pieza.PosX; i++)
                                {

                                    if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                    {
                                        // la casilla esta vacia y no se puede comer
                                    }
                                    // si hay una pieza blanca
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca)
                                    {
                                        imposible = true; // no se puede comer
                                    }
                                    // si hay una negra
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible == false)
                                    {
                                        posible = true; // es posible comer 
                                    }
                                    // si hay otra negra en el camino
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible)
                                    {
                                        imposible = true; // no es posible comer mas alla de la primera
                                    }
                                    // si es posible comer y hay una casilla vacia al otro lado de la ficha a comer
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true; // se puede comer
                                    }
                                }
                            }
                            // izquierda
                            else if (7 - pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                bool imposible = false;
                                bool posible = false;
                                // mientras la distancia sea menor o igual a 7
                                for (int i = 1; i <= 7 - pieza.PosY; i++)
                                {
                                    if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                    {
                                        // la casilla esta vacia y no se puede comer
                                    }
                                    // si hay una pieza blanca
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca)
                                    {
                                        imposible = true;  // no se puede comer
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible == false)
                                    {
                                        posible = true; // es posible comer 
                                    }
                                    // si hay otra negra en el camino
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible)
                                    {
                                        imposible = true; // no es posible comer mas alla de la primera
                                    }
                                    // si es posible comer y hay una casilla vacia al otro lado de la ficha a comer
                                    else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true; // se puede comer
                                    }
                                }
                            }
                            // comer en diagonal hacia arriba
                            // izquierda
                            if (pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= 7 - pieza.PosX; i++)
                                {
                                    if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true;
                                    }
                                }
                            }
                            // derecha
                            else if (pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= pieza.PosY; i++)
                                {
                                    if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true;
                                    }
                                }
                            }
                            // comer en diagonal hacia la izquierda
                            if (7 - pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= pieza.PosX; i++)
                                {
                                    if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true;
                                    }
                                }
                            }
                            // derecha
                            else if (7 - pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= 7 - pieza.PosY; i++)
                                {
                                    if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true;
                                    }
                                }
                            }
                            // diagonal superior izquierda
                            if (pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= pieza.PosX; i++)
                                {
                                    if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true;
                                    }
                                }
                            }
                            else if (pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= pieza.PosY; i++)
                                {
                                    if (Tablero[pieza.PosY - +i, pieza.PosX - i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer= true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // es blanca pero no reina
                        if (pieza.EsBlanca)
                        {
                            // solo se pude mover 1 casilla si no esta obstruida
                            if ((pieza.PosX == 0 || pieza.PosX == 1) && pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                // si se puede saltar diagonalmente sobre la negra
                                if (Tablero[pieza.PosY + 1, pieza.PosX + 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX + 2].EsVacia)
                                {
                                    puede_comer = true; // se puede comer
                                }
                                    
                            }
                            else if ((pieza.PosX == 7 || pieza.PosX == 6) && pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                if (Tablero[pieza.PosY + 1, pieza.PosX - 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX - 2].EsVacia)
                                {
                                    puede_comer = true;
                                }
                            }
                            // si esta en la columna 6 y 7 y no en la fila 7
                            else if (pieza.PosY != 6 && pieza.PosY != 7)
                            {
                                // si hay 2 casillas libres
                                if (Tablero[pieza.PosY + 1, pieza.PosX - 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX - 2].EsVacia)
                                {
                                    puede_comer = true; // se puede comer
                                }
                                // si solo hay 1
                                else if (Tablero[pieza.PosY + 1, pieza.PosX + 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX + 2].EsVacia)
                                {
                                    puede_comer = true; // se puede comer
                                }
                            }
                        }
                    }
                }
                else
                {
                    // lo mismo pero en las negras
                    if (pieza.EsReina)
                    {
                        if (pieza.EsNegra)
                        {
                                // comer en diagonal hacia abajo
                                if (7 - pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                                {
                                    bool imposible = false;
                                    bool posible = false;
                                    for (int i = 1; i <= 7 - pieza.PosX; i++)
                                    {
                                        if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                        {

                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible == false)
                                        {
                                            posible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                        {
                                            puede_comer= true;
                                        }
                                    }
                                }
                                else if (7 - pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                                {
                                    bool imposible = false;
                                    bool posible = false;
                                    for (int i = 1; i <= 7 - pieza.PosY; i++)
                                    {
                                        if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                        {

                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible == false)
                                        {
                                            posible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                        {
                                            puede_comer = true;
                                        }
                                    }
                                }

                                // comer en diagonal hacia arriba
                                if (pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                                {
                                    bool imposible = false;
                                    bool posible = false;
                                    for (int i = 1; i <= 7 - pieza.PosX; i++)
                                    {
                                        if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                        {

                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible == false)
                                        {
                                            posible = true;
                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                        {
                                            puede_comer= true;
                                        }
                                    }
                                }
                                else if (pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                                {
                                    bool imposible = false;
                                    bool posible = false;
                                    for (int i = 1; i <= pieza.PosY; i++)
                                    {
                                        if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                        {

                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible == false)
                                        {
                                            posible = true;
                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                        {
                                            puede_comer= true;
                                        }
                                    }
                                }

                                // comer en diagonal hacia la izquierda
                                if (7 - pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                                {
                                    bool imposible = false;
                                    bool posible = false;
                                    for (int i = 1; i <= pieza.PosX; i++)
                                    {
                                        if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                        {

                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible == false)
                                        {
                                            posible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                        {
                                            puede_comer= true;
                                        }
                                    }
                                }
                                else if (7 - pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                                {
                                    bool imposible = false;
                                    bool posible = false;
                                    for (int i = 1; i <= 7 - pieza.PosY; i++)
                                    {
                                        if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                        {

                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible == false)
                                        {
                                            posible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible)
                                        {
                                            imposible = true;
                                        }
                                        else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                        {
                                            puede_comer = true;
                                        }
                                    }
                                }

                            // diagonal superior izquierda
                            if (pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= pieza.PosX; i++)
                                {
                                    if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer = true;
                                    }
                                }
                            }
                            else if (pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                            {
                                bool imposible = false;
                                bool posible = false;
                                for (int i = 1; i <= pieza.PosY; i++)
                                {
                                    if (Tablero[pieza.PosY - +i, pieza.PosX - i].EsVacia && posible == false)
                                    {

                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible == false)
                                    {
                                        posible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible)
                                    {
                                        imposible = true;
                                    }
                                    else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                    {
                                        puede_comer = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pieza.EsNegra)
                        {
                            if ((pieza.PosX == 0 || pieza.PosX == 1) && pieza.PosY != 1 && pieza.PosY != 0)
                            {
                                if (Tablero[pieza.PosY - 1, pieza.PosX + 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX + 2].EsVacia)
                                {
                                    puede_comer = true;
                                }
                            }
                            else if ((pieza.PosX == 7 || pieza.PosX == 6) && pieza.PosY != 1 && pieza.PosY != 0)
                            {
                                if (Tablero[pieza.PosY - 1, pieza.PosX - 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX - 2].EsVacia)
                                {
                                    puede_comer = true;
                                }
                            }
                            else if(pieza.PosY != 1  && pieza.PosY !=0)
                            {
                                if (Tablero[pieza.PosY - 1, pieza.PosX - 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX - 2].EsVacia)
                                {
                                    puede_comer = true;
                                }
                                else if (Tablero[pieza.PosY - 1, pieza.PosX + 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX + 2].EsVacia)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                    }
                }
            }
            return puede_comer;
            
        }
        // verificar para una pieza especifica
        private bool PuedeComer(Pieza pieza)
        {
            bool puede_comer = false;
            if (pieza.EsReina)
            {
                if (pieza.EsBlanca)
                {

                    {
                        //comer en diagonal hacia abajo
                        if (7 - pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (7 - pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }

                        //comer en diagonal hacia arriba
                        if (pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }

                        //comer en diagonal hacia la izquierda
                        if (7 - pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (7 - pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }

                        //diagonal superior izquierda
                        if (pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY - +i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                    }
                }
                if (pieza.EsNegra)
                {

                    {
                        //comer en diagonal hacia abajo
                        if (7 - pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (7 - pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }

                        //comer en diagonal hacia arriba
                        if (pieza.PosY > 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (pieza.PosY < 7 - pieza.PosX && pieza.PosX != 6 && pieza.PosX != 7 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX + i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }

                        //comer en diagonal hacia la izquierda
                        if (7 - pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (7 - pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 6 && pieza.PosY != 7)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= 7 - pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY + i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }

                        //diagonal superior izquierda
                        if (pieza.PosY > pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosX; i++)
                            {
                                if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                        else if (pieza.PosY < pieza.PosX && pieza.PosX != 0 && pieza.PosX != 1 && pieza.PosY != 0 && pieza.PosY != 1)
                        {
                            bool imposible = false;
                            bool posible = false;
                            for (int i = 1; i <= pieza.PosY; i++)
                            {
                                if (Tablero[pieza.PosY - +i, pieza.PosX - i].EsVacia && posible == false)
                                {

                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsNegra)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible == false)
                                {
                                    posible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsBlanca && posible)
                                {
                                    imposible = true;
                                }
                                else if (Tablero[pieza.PosY - i, pieza.PosX - i].EsVacia && posible && imposible == false)
                                {
                                    puede_comer = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (pieza.EsBlanca)
                {
                    if ((pieza.PosX == 0 || pieza.PosX == 1) && pieza.PosY != 6 && pieza.PosY != 7)
                    {
                        if (Tablero[pieza.PosY + 1, pieza.PosX + 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX + 2].EsVacia)
                        {
                            puede_comer = true;
                        }

                    }
                    else if ((pieza.PosX == 7 || pieza.PosX == 6) && pieza.PosY != 6 && pieza.PosY != 7)
                    {
                        if (Tablero[pieza.PosY + 1, pieza.PosX - 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX - 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                    }
                    else if (pieza.PosY != 6 && pieza.PosY != 7)
                    {
                        if (Tablero[pieza.PosY + 1, pieza.PosX - 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX - 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                        else if (Tablero[pieza.PosY + 1, pieza.PosX + 1].EsNegra && Tablero[pieza.PosY + 2, pieza.PosX + 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                    }
                }
                else if (pieza.EsNegra)
                {
                    if ((pieza.PosX == 0 || pieza.PosX == 1) && pieza.PosY != 1 && pieza.PosY != 0)
                    {
                        if (Tablero[pieza.PosY - 1, pieza.PosX + 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX + 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                    }
                    else if ((pieza.PosX == 7 || pieza.PosX == 6) && pieza.PosY != 1 && pieza.PosY != 0)
                    {
                        if (Tablero[pieza.PosY - 1, pieza.PosX - 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX - 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                    }
                    else if (pieza.PosY != 1 && pieza.PosY != 0)
                    {
                        if (Tablero[pieza.PosY - 1, pieza.PosX - 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX - 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                        else if (Tablero[pieza.PosY - 1, pieza.PosX + 1].EsBlanca && Tablero[pieza.PosY - 2, pieza.PosX + 2].EsVacia)
                        {
                            puede_comer = true;
                        }
                    }
                }
            }
            return puede_comer;
        }
        // Mover la pieza
        private bool Mover()
        {
            // si la casilla de destino esta vacia
            if (pieza2.EsVacia == true)
            {
                // comprobar si se puede comer
                bool comer = PuedeComer();
                //si no es reina
                if (pieza1.EsReina == false)
                {
                    // si es blanca
                    if(pieza1.EsBlanca)
                    {
                        //  si esta en el borde izquierdo
                        if(pieza1.PosX == 0)
                        {
                            // se puede mover pero no comer
                            if(pieza2.PosX == pieza1.PosX + 1 && pieza2.PosY == pieza1.PosY + 1 && comer == false)
                            {
                                //Mover
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = false; // actualizar jugada
                                return true;
                            }
                            // se puede mover y comer
                            else if(Tablero[pieza1.PosY + 1, pieza1.PosX + 1].EsNegra && pieza2.PosY == pieza1.PosY + 2 
                                && pieza2.PosX == pieza1.PosX + 2 && comer)
                            {
                                //Comer
                                Comer(Tablero[pieza1.PosY + 1, pieza1.PosX + 1]);
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = true;
                                return true;
                            }
                            // ninguna de las anteriores
                            else
                            {
                                if (comer == false)
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                                // si evita comer una pieza que ya se podia comer
                                else
                                {
                                    MessageBox.Show("Es obligatorio caputar.");
                                    return false;
                                }
                            }
                        }
                        // para el borde derecho
                        else if (pieza1.PosX == 7)
                        {
                            // se puede mover pero no comer
                            if (pieza2.PosX == pieza1.PosX - 1 && pieza2.PosY == pieza1.PosY + 1 && comer == false)
                            {
                                //Mover
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = false; // actualizar jugada
                                return true;
                            }
                            // se puede mover y comer
                            else if (Tablero[pieza1.PosY + 1, pieza1.PosX - 1].EsNegra && pieza2.PosY == pieza1.PosY + 2
                                && pieza2.PosX == pieza1.PosX - 2 && comer)
                            {
                                //Comer
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY + 1, pieza1.PosX - 1]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            // ninguna de las anteriores
                            else
                            {
                                if (comer == false)
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                                // si evita comer una pieza que ya se podia comer
                                else
                                {
                                    MessageBox.Show("Es obligatorio caputar.");
                                    return false;
                                }
                            }
                        }
                        // no estan en los bordes
                        else
                        {
                            if ((pieza2.PosX == pieza1.PosX - 1 || pieza2.PosX == pieza1.PosX + 1) &&
                                (pieza2.PosY == pieza1.PosY + 1) && comer == false)
                            {
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = false;
                                return true;
                            }
                            if(Tablero[pieza1.PosY + 1, pieza1.PosX + 1].EsNegra && pieza2.PosY == pieza1.PosY + 2 && pieza2.PosX == pieza1.PosX + 2 && comer)
                            {
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY + 1, pieza1.PosX + 1]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            if(Tablero[pieza1.PosY + 1, pieza1.PosX - 1].EsNegra && pieza2.PosY == pieza1.PosY + 2 && pieza2.PosX == pieza1.PosX - 2 && comer)
                            {
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY + 1, pieza1.PosX - 1 ]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            else
                            {
                                if (comer == false)
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                                else 
                                {
                                    MessageBox.Show("Es obligatorio caputar.");
                                    return false;
                                }
                            }
                        }
                    }
                    else if (pieza1.EsNegra)
                    {
                        if (pieza1.PosX == 0)
                        {
                            if (pieza2.PosX == pieza1.PosX + 1 && pieza2.PosY == pieza1.PosY - 1 && comer == false)
                            {
                                //Mover
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = false;
                                return true;
                            }
                            else if (Tablero[pieza1.PosY - 1, pieza1.PosX + 1].EsBlanca && pieza2.PosY == pieza1.PosY - 2
                                && pieza2.PosX == pieza1.PosX + 2 && comer)
                            {
                                //Comer
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY - 1, pieza1.PosX + 1]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            else
                            {
                                if (comer == false)
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Es obligatorio caputar.");
                                    return false;
                                }
                            }
                        }
                        else if (pieza1.PosX == 7)
                        {
                            if (pieza2.PosX == pieza1.PosX - 1 && pieza2.PosY == pieza1.PosY - 1 && comer == false)
                            {
                                //Mover
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = false;
                                return true;
                            }
                            else if (Tablero[pieza1.PosY - 1, pieza1.PosX - 1].EsBlanca && pieza2.PosY == pieza1.PosY - 2
                                && pieza2.PosX == pieza1.PosX - 2 && comer)
                            {
                                //Comer
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY - 1, pieza1.PosX - 1]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            else
                            {
                                if (comer == false)
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Es obligatorio caputar.");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if ((pieza2.PosX == pieza1.PosX - 1 || pieza2.PosX == pieza1.PosX + 1) &&
                                (pieza2.PosY == pieza1.PosY - 1) && comer == false)
                            {
                                MoverPieza(pieza1.Tipo);
                                UltimaJugada = false;
                                return true;
                            }
                            if (Tablero[pieza1.PosY - 1, pieza1.PosX + 1].EsBlanca && pieza2.PosY == pieza1.PosY - 2 && pieza2.PosX == pieza1.PosX + 2 && comer)
                            {
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY - 1, pieza1.PosX + 1]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            if (Tablero[pieza1.PosY - 1, pieza1.PosX - 1].EsBlanca && pieza2.PosY == pieza1.PosY - 2 && pieza2.PosX == pieza1.PosX - 2 && comer)
                            {
                                UltimaJugada = true;
                                Comer(Tablero[pieza1.PosY - 1, pieza1.PosX - 1]);
                                MoverPieza(pieza1.Tipo);
                                return true;
                            }
                            else
                            {
                                if (comer == false)
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Es obligatorio caputar.");
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (pieza1.EsBlanca)
                    {
                        if ((pieza2.PosY - pieza1.PosY) > 0 && (pieza2.PosX - pieza1.PosX) > 0)
                        {
                            if (pieza2.PosY-pieza1.PosY == pieza2.PosX - pieza1.PosX)
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < (pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY + i, pieza1.PosX + i];
                                    if (pieza.EsNegra)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY + i, pieza1.PosX + i];
                                    }
                                    if (pieza.EsBlanca)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }

                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido.");
                                return false;
                            }
                        }
                        if ((pieza2.PosY - pieza1.PosY) > 0 && (pieza2.PosX - pieza1.PosX) < 0)
                        {
                            if(pieza2.PosY - pieza1.PosY == -(pieza2.PosX - pieza1.PosX))
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < (pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY + i, pieza1.PosX - i];
                                    if (pieza.EsNegra)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY + i, pieza1.PosX - i];
                                    }
                                    if (pieza.EsBlanca)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido.");
                                return false;
                            }

                        }
                        if ((pieza2.PosY - pieza1.PosY) < 0 && (pieza2.PosX - pieza1.PosX) < 0)
                        {
                            if (-(pieza2.PosY - pieza1.PosY) == -(pieza2.PosX - pieza1.PosX))
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < -(pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY - i, pieza1.PosX - i];
                                    if (pieza.EsNegra)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY - i, pieza1.PosX - i];
                                    }
                                    if (pieza.EsBlanca)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido.");
                                return false;
                            }


                        }
                        if ((pieza2.PosY - pieza1.PosY) < 0 && (pieza2.PosX - pieza1.PosX) > 0)
                        {
                            if (-(pieza2.PosY - pieza1.PosY) == (pieza2.PosX - pieza1.PosX))
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < -(pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY - i, pieza1.PosX + i];
                                    if (pieza.EsNegra)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY - i, pieza1.PosX + i];
                                    }
                                    if (pieza.EsBlanca)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido.");
                                return false;
                            }

                        }
                    }
                    else if (pieza1.EsNegra)
                    {
                        if ((pieza2.PosY - pieza1.PosY) > 0 && (pieza2.PosX - pieza1.PosX) > 0)
                        {
                            if (pieza2.PosY - pieza1.PosY == pieza2.PosX - pieza1.PosX)
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < (pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY + i, pieza1.PosX + i];
                                    if (pieza.EsBlanca)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY + i, pieza1.PosX + i];
                                    }
                                    if (pieza.EsNegra)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido 7.");
                                return false;
                            }
                        }
                        if ((pieza2.PosY - pieza1.PosY) > 0 && (pieza2.PosX - pieza1.PosX) < 0)
                        {
                            if (pieza2.PosY - pieza1.PosY == -(pieza2.PosX - pieza1.PosX))
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < (pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY + i, pieza1.PosX - i];
                                    if (pieza.EsBlanca)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY + i, pieza1.PosX - i];
                                    }
                                    if (pieza.EsNegra)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido.");
                                return false;
                            }

                        }
                        if ((pieza2.PosY - pieza1.PosY) < 0 && (pieza2.PosX - pieza1.PosX) < 0)
                        {
                            if (-(pieza2.PosY - pieza1.PosY) == -(pieza2.PosX - pieza1.PosX))
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < -(pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY - i, pieza1.PosX - i];
                                    if (pieza.EsBlanca)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY - i, pieza1.PosX - i];
                                    }
                                    if (pieza.EsNegra)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido 9.");
                                return false;
                            }


                        }
                        if ((pieza2.PosY - pieza1.PosY) < 0 && (pieza2.PosX - pieza1.PosX) > 0)
                        {
                            if (-(pieza2.PosY - pieza1.PosY) == (pieza2.PosX - pieza1.PosX))
                            {
                                int num = 0;
                                Pieza piezacaptura = null;
                                for (int i = 1; i < -(pieza2.PosY - pieza1.PosY); i++)
                                {
                                    Pieza pieza = Tablero[pieza1.PosY - i, pieza1.PosX + i];
                                    if (pieza.EsBlanca)
                                    {
                                        num++;
                                        piezacaptura = Tablero[pieza1.PosY - i, pieza1.PosX + i];
                                    }
                                    if (pieza.EsNegra)
                                    {
                                        num = num + 2; //No se puede mover
                                    }
                                }
                                if (num == 0 && comer==false)
                                {
                                    MoverPieza(pieza1.Tipo);
                                    UltimaJugada = false;
                                    return true;
                                }
                                else if (num == 1 && comer)
                                {
                                    UltimaJugada = true;
                                    Comer(Tablero[piezacaptura.PosY, piezacaptura.PosX]);
                                    MoverPieza(pieza1.Tipo);
                                    return true;
                                }
                                else if (comer)
                                {
                                    MessageBox.Show("La captura es obligatoria");
                                    return false;
                                }
                                else
                                {
                                    MessageBox.Show("Movimento Inválido.");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Movimento Inválido.");
                                return false;
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Movimento Inválido - El espacio no está vacío.");
                return false;
            }
            MessageBox.Show("Erro 5");
            return false;
        }

        // boton de salir
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿De verdad quieres salir?", "Cerrar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                //nada
            }
        }

        // boton de rendirse
        private void button4_Click(object sender, EventArgs e)
        {
            if (TurnoBlancas)
            {
                if (MessageBox.Show("¿De verdad quieres rendirte, Blanco?", "Rendirse", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (MessageBox.Show("¡El negro gana! ¿Reanudar?", "Vcitoria!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Restart();
                        return;
                    }
                    else
                    {
                        //nada
                    }
                }
            }
            else if (TurnoBlancas == false)
            {
                if (MessageBox.Show("¿De verdad quieres rendirte, Negro?", "Rendirse", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (MessageBox.Show("¡Blanco gana! ¿Reanudar?", "Victoria!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Restart();
                        return;
                    }
                    else
                    {
                        //nada
                    }
                }
            }
        }

        // al clickear una ficha
        private void Button_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            // si no se ha seleccionado una pieza
            if (pieza1 == null)
            {
                boton1 = boton; // casilla presionada
                pieza1 = Tablero[boton1.Location.Y / 100, boton1.Location.X / 100]; // obtener ubicacion de la pieza
                // si se selecciona una casilla vacia
                if (pieza1.EsVacia)
                {
                    // no pasa nada, y se permite seleccionar otra pieza
                    pieza1 = null;
                    boton1 = null;
                    return;
                }
                // si se selecciona una pieza blanca en el turno de las negras
                else if (pieza1.EsBlanca && TurnoBlancas == false)
                {
                    MessageBox.Show("¡Es el turno de las negras!");
                    // no se mueven
                    pieza1 = null;
                    boton1 = null;
                    return;
                }
                // si se selecciona una pieza negra en el turno de las blancas
                else if(pieza1.EsNegra && TurnoBlancas)
                {
                    MessageBox.Show("¡Es el turno de las blancas!");
                    // no se muven
                    pieza1 = null;
                    boton1 = null;
                    return;
                }
                boton1.FlatAppearance.BorderColor = Color.Green;
                boton1.FlatAppearance.BorderSize = 2;

            }
            else
            {
                boton2 = boton; // destino
                pieza2 = Tablero[boton2.Location.Y / 100, boton2.Location.X / 100]; // pieza seleccionada
                bool move = Mover();
                boton1.FlatAppearance.BorderColor = Color.White; // seleccion anulada
                // se completo el movimiento y se puede seleccionar otra pieza
                pieza2 = null;
                boton1 = null;
                boton2 = null;
                // si se realizo la ultima jugada, y se hizo una nueva
                if (UltimaJugada == true && move)
                {
                    // se puede comer?
                    if (PuedeComer(pieza1))
                    {
                        // se selecciona la pieza
                        pieza1 = null;
                        return;
                    }   
                }
                // se selecciona la pieza
                pieza1 = null;
                // no quedan fichas negras
                if (Score_White == 12)
                {
                    if (MessageBox.Show("¡Blanco gana! ¿Reanudar?", "Victoria!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Restart();
                        return;
                    }
                    else
                    {
                        //nada
                    }
                }
                // no quedan fichas blancas
                if (Score_Black == 12)
                {
                    if (MessageBox.Show("¡El negro gana! ¿Reanudar?", "Victoria!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Restart();
                        return;
                    }
                    else
                    {
                        //nada
                    }
                }

                // cambiar el color para indicar los turnos
                if (TurnoBlancas && move)
                {
                    ScoreBlancas.BackColor = Color.Transparent;
                    ScoreNegras.BackColor = Color.Red;
                    TurnoBlancas = false;
                }
                else if (TurnoBlancas == false && move)
                {
                    ScoreBlancas.BackColor = Color.Beige;
                    ScoreNegras.BackColor = Color.Transparent;
                    TurnoBlancas = true;
                }
            }
        }
    }
}
