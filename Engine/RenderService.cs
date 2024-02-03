using System;
using System.Drawing;
using LessonConsoleGame.Game;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Engine
{
    public class RenderService
    {
        public int RenderedFPS {get;private set;}
        public int FPS { get; init; }

        public ValidationService ValidationService { get; private set; }
        public UIEngine UIEngine { get; private set; }
        public GameEngine GameEngine { get; private set; }

        public void Initialize(ValidationService validationService, UIEngine uiEngine, GameEngine gameEngine)
        {
            this.ValidationService = validationService;
            this.UIEngine = uiEngine;
            this.GameEngine = gameEngine;
        }

        public void SetCursorToOrigin()
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
        }

        public void Start()
        {
            int sleepMs = Math.Max(1, 1000 / FPS);
            Thread threadReadKey = new Thread(() =>
            {
                int renderedFPS = 0;
                DateTime lastUpdatedFPS = DateTime.UtcNow;

                while (true)
                {
                    this.SetCursorToOrigin();

                    if (this.ValidationService.ValidateConsoleSize())
                    {
                        List<(Rectangle validArea, Sprite[] sprites)> renderSprites = new List<(Rectangle validArea, Sprite[] sprites)>();
                        renderSprites.Add((new Rectangle(0, 0, this.UIEngine.TotalWidth, this.UIEngine.TotalHeight), new Sprite[] { this.UIEngine.GetMapSprite() }));

                        if (this.GameEngine.CurrentMap != null)
                        {
                            renderSprites.Add((new Rectangle(this.UIEngine.BorderSize, this.UIEngine.BorderSize, this.UIEngine.Width, this.UIEngine.Height), this.GameEngine.CurrentMap.GameObjectSprites.Select(o => new Sprite(o.Characters, this.UIEngine.BorderSize + o.X - this.GameEngine.CurrentMap.X, this.UIEngine.TotalHeight - this.UIEngine.BorderSize - o.Y - o.Height)).ToArray()));
                            this.GameEngine.TriggerUpdate();
                        }

                        this.RenderSprites(renderSprites.ToArray());
                        renderSprites.Clear();
                    }
                    else
                    {
                        this.RenderMessage(this.ValidationService.ConsoleSizeValidationMessage);
                    }

                    Thread.Sleep(sleepMs);

                    renderedFPS++;

                    if ((DateTime.UtcNow - lastUpdatedFPS).TotalSeconds >= 1)
                    {
                        lastUpdatedFPS = DateTime.UtcNow;
                        this.RenderedFPS = renderedFPS;
                        renderedFPS = 0;
                    }
                }
            });

            threadReadKey.Start();
        }

        public void RenderMessage(string message)
        {
            int messageLine = Console.WindowHeight / 2;

            string emptyLineText = "*".PadRight(Console.WindowWidth - 2, ' ') + "*";

            int paddingForCentering = (Console.WindowWidth - message.Length - 2) / 2;
            string paddingText = "".PadRight(paddingForCentering, ' ');
            string messageLineTopBottomText = paddingText + "".PadRight(message.Length + 2, '-') + paddingText;
            string messageLineText = paddingText + $"|{message}|" + paddingText;

            for (int i = 1; i <= Console.WindowHeight; i++)
            {
                if (i == messageLine - 1 || i == messageLine + 1)
                {
                    Console.Write(messageLineTopBottomText);
                }
                else if (i == messageLine)
                {
                    Console.Write(messageLineText);
                }
                else
                {
                    Console.Write(emptyLineText);
                }

                if (i != Console.WindowHeight)
                {
                    Console.WriteLine();
                }
            }
        }

        public void RenderSprites((Rectangle validArea, Sprite[] sprites)[] sprites)
        {
            for (int ry = 0; ry < Console.WindowHeight; ry++)
            {
                string text = "";
                for (int rx = 0; rx < Console.WindowWidth; rx++)
                {
                    char c = sprites
                                .Where(o =>
                                    rx >= o.validArea.X &&
                                    rx < o.validArea.X + o.validArea.Width &&
                                    ry >= o.validArea.Y &&
                                    ry < o.validArea.Y + o.validArea.Height &&
                                    o.sprites.Any(s => s.Get(rx, ry) != '\0')
                                )
                                .Select(o =>
                                    o.sprites.Where(s => s.Get(rx, ry) != '\0').Select(s => s.Get(rx, ry)).FirstOrDefault()
                                )
                                .FirstOrDefault();
                    if (c == '\0')
                    {
                        c = ' ';
                    }

                    text += c;
                }

                Console.Write(text);

                if (ry != Console.WindowHeight - 1)
                {
                    Console.WriteLine();
                }
            }

        }
    }
}