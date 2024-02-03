using System.Drawing;

namespace LessonConsoleGame.Engine
{
    public class Sprite
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public char[,] Characters { get; private set; }
        public int Height => this.Characters.GetLength(0);
        public int Width => this.Characters.GetLength(1);
        public Rectangle Rectangle => new Rectangle(this.X, this.Y, this.Width, this.Height);

        public Sprite(char[,] characters, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Characters = characters;
        }

        public char Get(int x, int y)
        {
            x -= this.X;
            y -= this.Y;
            
            if (x >= Width || y >= Height || x < 0 || y < 0)
            {
                return '\0';
            }

            return this.Characters[y, x];
        }
    }
}