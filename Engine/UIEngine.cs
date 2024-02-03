namespace LessonConsoleGame.Engine
{
    public class UIEngine
    {
        public int Width { get; init; }
        public int Height { get; init; }
        public int BorderSize { get; set; }
        public int TotalWidth => this.Width + this.BorderSize * 2;
        public int TotalHeight => this.Height + this.BorderSize * 2;

        public Sprite GetMapSprite()
        {
            char[,] characters = new char[TotalHeight, TotalWidth];

            // Corners
            for (int i = 0; i < this.BorderSize; i++)
            {
                characters[i, i] = '*';
                characters[TotalHeight - 1 - i, i] = '*';
                characters[TotalHeight - 1 - i, TotalWidth - 1 - i] = '*';
                characters[i, TotalWidth - 1 - i] = '*';
            }

            // Top/Bottom Lines
            for (int x = this.BorderSize; x <= this.Width + this.BorderSize - 1; x++)
            {
                characters[this.BorderSize - 1, x] = '-';
                characters[TotalHeight - this.BorderSize, x] = '^';
            }

            // Left/Right Lines
            for (int y = this.BorderSize; y <= this.Height + this.BorderSize - 1; y++)
            {
                characters[y, this.BorderSize - 1] = '|';
                characters[y, TotalWidth - this.BorderSize] = '|';
            }

            return new Sprite(characters, 0, 0);
        }
    }
}