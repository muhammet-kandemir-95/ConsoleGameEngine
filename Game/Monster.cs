using System;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public class Monster : GameObject, IGravity
    {
        public Monster(Map map, int x, int y) : base(map)
        {
            this.X = x;
            this.Y = y;
        }

        public override int X { get; protected set; }

        public override int Y { get; protected set; }

        public override int Width { get; protected set; } = 3;

        public override int Height { get; protected set; } = 3;

        private bool hasItShown = false;
        private bool moveLeft = false;
        private DateTime lastAnimOn = DateTime.UtcNow;
        bool anim1 = false;
        public override Sprite[] Sprites
        {
            get
            {
                char[,] sprite = new char[,]
                {
                    { '\0', 'V', '\0'  },
                    { '*', '*', '*'  },
                    { '/', '^', '\\'  }
                };

                if ((DateTime.UtcNow - lastAnimOn).TotalMilliseconds > GameEngine.AnimationMillisecondInterval * 5)
                {
                    lastAnimOn = DateTime.UtcNow;
                    anim1 = !anim1;
                }

                if (anim1)
                {
                    sprite = new char[,]
                    {
                        { '\0', 'V', '\0'  },
                        { '*', '*', '*'  },
                        { '|', '^', '|'  }
                    };
                }

                return new Sprite[] { new Sprite(sprite, this.X, this.Y) };

            }
        }

        public bool GravityDisabled { get; set; }
        public DateTime FallingSetOn { get; set; }
        private DateTime lastMoveOn = DateTime.UtcNow;

        public override void OnUpdated()
        {
            if (this.ScreenX >= 0 && this.ScreenX < this.Map.Width)
            {
                this.hasItShown = true;
                this.GravityDisabled = false;
            }
            else if (!this.hasItShown)
            {
                this.GravityDisabled = true;
            }

            if (this.hasItShown && !this.Map.Player.IsDead && (DateTime.UtcNow - lastMoveOn).TotalMilliseconds > GameEngine.AnimationMillisecondInterval)
            {
                lastMoveOn = DateTime.UtcNow;

                if (moveLeft)
                {
                    this.Move(-1, 0);
                }
                else
                {
                    this.Move(1, 0);
                }
            }
        }

        public override bool OnTriggered(IGameObject gameObject, bool check, int x, int y)
        {
            if (!(gameObject is Player) && (check || X == 0))
            {
                return true;
            }

            if (!this.hasItShown)
            {
                return true;
            }

            if (gameObject is Player)
            {
                ((Player)gameObject).IsDead = true;
                return true;
            }
            else if (gameObject is Block)
            {
                moveLeft = !moveLeft;
            }

            return true;
        }
    }
}