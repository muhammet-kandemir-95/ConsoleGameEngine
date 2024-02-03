using System;
using System.ComponentModel;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public class Map
    {
        private List<IGameObject> gameObjects = new List<IGameObject>();
        public IEnumerable<IGameObject> GameObjects => this.gameObjects;
        public Sprite[] GameObjectSprites => gameObjects.SelectMany(o => o.Sprites).ToArray();
        public int X { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GameEngine GameEngine { get; private set; }
        public Player Player { get; private set; }

        public Map(GameEngine gameEngine, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.GameEngine = gameEngine;
        }

        public void TriggerUpdate()
        {
            foreach (IGameObject gameObject in this.gameObjects)
            {
                gameObject.OnUpdated();
            }
        }

        public void InitializeGameObject(IGameObject gameObject)
        {
            this.gameObjects.Add(gameObject);

            gameObject.OnInitialized();

            if (gameObject is Player)
            {
                this.Player = (Player)gameObject;
            }
        }

        public void DisposeGameObject(IGameObject gameObject)
        {
            gameObject.IsRemoved = true;

            gameObject.OnDisposed();
        }
    }
}