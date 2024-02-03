using System;
using LessonConsoleGame.Game;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame
{
    public class Program
    {
        private static string blocks = "";
        private static void regenerateBlocks(Map map)
        {
            string newBlocks = File.ReadAllText("Blocks.txt");
            if (newBlocks != blocks)
            {
                List<IGameObject> gameObjects = (List<IGameObject>)map.GameObjects;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Block)
                    {
                        gameObjects.RemoveAt(i);
                        i--;
                    }
                }

                foreach (string blockData in newBlocks.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int[] formattedData = blockData.Replace(" ", "").Split(',').Select(o => Convert.ToInt32(o)).ToArray();
                    map.InitializeGameObject(new Block(map, formattedData[0], formattedData[1], formattedData[2], formattedData[3]));
                }

                blocks = newBlocks;
            }
        }

        private static string points = "";
        private static void regeneratePoints(Map map)
        {
            string newPoints = File.ReadAllText("Points.txt");
            if (newPoints != points)
            {
                List<IGameObject> gameObjects = (List<IGameObject>)map.GameObjects;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Point)
                    {
                        gameObjects.RemoveAt(i);
                        i--;
                    }
                }

                foreach (string pointData in newPoints.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int[] formattedData = pointData.Replace(" ", "").Split(',').Select(o => Convert.ToInt32(o)).ToArray();
                    map.InitializeGameObject(new Point(map, formattedData[0], formattedData[1]));
                }

                points = newPoints;
            }
        }
        
        private static string monsters = "";
        private static void regenerateMonsters(Map map)
        {
            string newMonsters = File.ReadAllText("Monsters.txt");
            if (newMonsters != monsters)
            {
                List<IGameObject> gameObjects = (List<IGameObject>)map.GameObjects;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Monster)
                    {
                        gameObjects.RemoveAt(i);
                        i--;
                    }
                }

                foreach (string monsterData in newMonsters.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int[] formattedData = monsterData.Replace(" ", "").Split(',').Select(o => Convert.ToInt32(o)).ToArray();
                    map.InitializeGameObject(new Monster(map, formattedData[0], formattedData[1]));
                }

                monsters = newMonsters;
            }
        }

        public static void Main(string[] args)
        {
            const int FPS = 30;
            const int UIWidth = 100;
            const int UIHeight = 30;
            const int UIBorderSize = 2;

            // Services
            ValidationService validationService = new ValidationService();
            GameEngine gameEngine = new GameEngine();
            RenderService renderService = new RenderService()
            {
                FPS = FPS
            };

            // Game
            UIEngine ui = new UIEngine()
            {
                Width = UIWidth,
                Height = UIHeight,
                BorderSize = UIBorderSize,
            };

            // Initialize
            validationService.Initialize(ui);
            gameEngine.Initialize(validationService, ui);
            renderService.Initialize(validationService, ui, gameEngine);

            Map map = new Map(gameEngine, ui.Width, ui.Height);
            gameEngine.SetCurrentMap(map);
            Player player = new Player(map);

            map.InitializeGameObject(new Message(map, 0, map.Height - 3)
            {
                Text = () => $" Score: {player.Score}  {Environment.NewLine} FPS:{renderService.RenderedFPS}  {Environment.NewLine} X: {player.X}, Y:{player.Y}  "
            });
            map.InitializeGameObject(new Message(map, map.Width / 2 - 8, map.Height / 2 - 1)
            {
                Text = () => player.IsDead ? ("_______________" + Environment.NewLine + 
                             ">| GAME OVER |<" + Environment.NewLine + 
                             "---------------") : "\0"
            });
            map.InitializeGameObject(player);

            // Start Rendering...
            renderService.Start();

            while (true)
            {
                regenerateBlocks(map);
                regeneratePoints(map);
                regenerateMonsters(map);

                ConsoleKey pressedKey = Console.ReadKey(true).Key;
                gameEngine.TriggerKeyPressed(pressedKey);
                gameEngine.TriggerKeyPressed(pressedKey);
            }
        }
    }
}