using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;

namespace Grid_Template___Windows_Forms_App__.NET_Framework__COPY
{
    public partial class Form1 : Form
    {
        private Grid gameGrid;
        private int tileSize = 25; // Size of each tile
        private int gridWidth;
        private int gridHeight;

        string gameState = "startScreen";

        string openMsg = "sampleStartScreen"; //Placeholder for start screen message

        public Form1()
        {
            InitializeComponent();
            GameInit();

            // Set the form to start maximized
            this.WindowState = FormWindowState.Maximized;

            // Disable the maximize box
            this.MaximizeBox = false;

            // Disable the ability to restore down
            this.Resize += Form1_Resize;
        }

        private void GameInit()
        {
            //Opens the game on the start screen
            gameState = "startScreen";

            // Hook up the Paint, KeyDown, and Resize events
            this.BackColor = Color.Black;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Resize += new EventHandler(Form1_Resize);

            // Initialize the game grid
            UpdateGridSize();
        }

        private void UpdateGridSize()
        {
            //Changes grid size to match clients screen dimentions
            gridWidth = this.ClientSize.Width / tileSize;
            gridHeight = this.ClientSize.Height / tileSize;

            // Initialize game grid and player position
            gameGrid = new Grid(gridWidth, gridHeight, tileSize);

            // Redraw the grid
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Displays start screen
            if (gameState == "startScreen")
            {
                titleLabel.Text = openMsg;
                titleLabel.Visible = true;
            }

            //Draws grid to screen upon leaving the start screen
            else
            {
                titleLabel.Visible = false;

                // Draw the game grid

                titleLabel.Visible = false;

                // Draw the game grid
                foreach (var tile in gameGrid.Tiles)
                {
                    //Colors tiles in the "on" state white
                    using (Brush onBrush = new SolidBrush(Color.White))
                    {
                        if (tile.on == true)
                        {
                            e.Graphics.FillRectangle(onBrush, tile.X, tile.Y, tile.Size, tile.Size);
                        }
                    }

                    //Outlines each tile gray
                    using (Pen gridOutline = new Pen(Color.Gray))
                    {
                        if (tile.on == false)
                        {
                            e.Graphics.DrawRectangle(gridOutline, tile.X, tile.Y, tile.Size, tile.Size);
                        }
                    }
                }
            }
        }

        //Allows player to interact with the game
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //Default pause / start key
                case Keys.Space:
                    if (gameState != "paused")
                    {
                        gameState = "paused";
                        Refresh();
                    }

                    else
                    {
                        gameState = "running";
                    }
                    break;

                //Default "close form" key
                case Keys.Escape:
                    this.Close();
                    break;

                //Default "clear grid" key
                case Keys.R:
                    ClearGrid();
                    break;
            }
        }

        private void ClearGrid()
        {
            //Sets all tiles to the "off" state
            if (gameState == "paused")
            {
                foreach (var tile in gameGrid.Tiles)
                {
                    tile.on = false;
                }
                Refresh();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Ensure the form stays maximized
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                // Update grid size and redraw when form is resized
                UpdateGridSize();
            }
        }

        // Define a class to represent a tile
        public class Tile
        {
            //Sets positions on screen
            public int X { get; set; }
            public int Y { get; set; }

            //Defines size of tiles according to tileSize variable
            public int Size { get; set; }
            public bool on { get; set; }

            //Gives a tiles X/Y coordinates relative to its grid
            public int refX { get; set; }
            public int refY { get; set; }
        }

        // Define a class to represent the game grid
        public class Grid
        {
            //Holds each individual tile in the grid
            public List<Tile> Tiles { get; set; }

            //Defines a grid
            public Grid(int width, int height, int tileSize)

            {
                //Generates grid with tiles defaulting to the "off" state
                Tiles = new List<Tile>();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Tiles.Add(new Tile
                        {
                            X = x * tileSize,
                            Y = y * tileSize,
                            Size = tileSize,
                            on = false,
                            refX = x,
                            refY = y
                        });

                    }
                }
            }

            //Gets the states of each tiles neighbours
            public int CheckNeighborStates(Tile tile)
            {
                //Defines positions of neighbours relative to a given tile
                int count = 0;
                int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
                int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

                //Checks neighbours states
                for (int i = 0; i < 8; i++)
                {
                    int newX = tile.refX + dx[i];
                    int newY = tile.refY + dy[i];

                    Tile neighbour = Tiles.Find(t => t.refX == newX && t.refY == newY);

                    //Counts number of neighbours in the "on" state
                    if (neighbour != null && neighbour.on == true)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        //Allows player to interact with tile states using a mouse
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //Prevents player from interfering with tile states while the game is running
            if (gameState == "paused")
            {
                int mouseX = e.X;
                int mouseY = e.Y;

                //Allows user to switch tile states between "on" and "off"
                foreach (var tile in gameGrid.Tiles)
                {
                    if (mouseX >= tile.X && mouseY >= tile.Y
                        && mouseX <= tile.X + tileSize && mouseY <= tile.Y + tileSize)
                    {
                        tile.on = !tile.on;
                        Refresh();
                    }
                }
            }
        }

        //Updates game state each frame
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (gameState == "running")
            {

            }
        }
    }
}