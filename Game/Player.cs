using System;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public enum PlayerAnimType
    {
        Idle,
        Walk,
        Jump,
        Fall
    }

    public class Player : GameObject, IGravity
    {
        public Player(Map map) : base(map)
        {
        }

        public override int X { get; protected set; } = 2;

        public override int Y { get; protected set; }

        public override int Width { get; protected set; } = 3;

        public override int Height { get; protected set; } = 3;

        public int Score => this.Point;
        public int Point { get; set; }

        private const int ScreenXLeftLimit = 13;
        private const int ScreenXRightLimit = 20;

        private const int JumpLong = 10;
        private int jump = 0;
        private DateTime jumpUpdatedOn = DateTime.UtcNow;

        private DateTime animSetOn = DateTime.UtcNow;
        public PlayerAnimType CurrentAnim { get; set; } = PlayerAnimType.Idle;

        private char[,] spriteIdle = new char[,]
        {
            { '\0', 'O', '\0' },
            { '/', '|', '\\' },
            { '/', '\0', '\\' },
        };

        private char[,] spriteWalk01 = new char[,]
        {
            { '\0', 'O', '\0' },
            { '/', '|', '\\' },
            { '\0', '\\', '\0' },
        };

        private char[,] spriteWalk02 = new char[,]
        {
            { '\0', 'O', '\0' },
            { '/', '|', '\\' },
            { '\0', '/', '\0' },
        };

        private char[,] spriteJump = new char[,]
        {
            { '\0', 'O', '\0' },
            { '/', '|', '\\' },
            { '\0', '^', '\0' },
        };

        private char[,] spriteFall = new char[,]
        {
            { '\0', 'O', '\0' },
            { '\\', '|', '/' },
            { '\\', '|', '/' },
        };

        private int animCounter;
        public override Sprite[] Sprites
        {
            get
            {
                this.animCounter = (this.animCounter + 1) % 100;
                char[,] sprite = null;

                switch (this.CurrentAnim)
                {
                    case PlayerAnimType.Fall:
                        sprite = this.spriteFall;
                        break;
                    case PlayerAnimType.Jump:
                        sprite = this.spriteJump;
                        break;
                    case PlayerAnimType.Walk:
                        if (this.animCounter % 20 < 10)
                        {
                            sprite = this.spriteWalk01;
                        }
                        else
                        {
                            sprite = this.spriteWalk02;
                        }
                        break;
                    case PlayerAnimType.Idle:
                    default:
                        sprite = this.spriteIdle;
                        break;
                }

                return new Sprite[] { new Sprite(sprite, this.X, this.Y) };
            }
        }

        public bool GravityDisabled { get; set; }
        public DateTime FallingSetOn { get; set; }
        public bool IsDead { get; set; }
        private int? deadStep = null;
        private DateTime deadStepDoneOn = DateTime.UtcNow;

        public override void OnUpdated()
        {
            if (this.IsDead)
            {
                this.GravityDisabled = true;
                deadStep = deadStep ?? 5;

                if ((DateTime.UtcNow - deadStepDoneOn).TotalMilliseconds > 100)
                {
                    deadStepDoneOn = DateTime.UtcNow;
                    this.CurrentAnim = PlayerAnimType.Fall;

                    if (deadStep > 0)
                    {
                        deadStep--;
                        this.Y++;
                    }
                    else
                    {
                        this.Y = Math.Max(-100, this.Y - 1);
                    }
                }

                return;
            }

            this.GravityDisabled = this.jump != 0;

            if (this.jump > 0)
            {
                this.CurrentAnim = PlayerAnimType.Jump;

                if ((DateTime.UtcNow - this.jumpUpdatedOn).TotalMilliseconds > GameEngine.AnimationMillisecondInterval)
                {
                    if (this.Move(0, 1))
                    {
                        this.jump--;
                        this.jumpUpdatedOn = DateTime.UtcNow;
                    }
                    else
                    {
                        this.jump = 0;
                    }
                }
            }
            else if (this.IsFalling)
            {
                this.CurrentAnim = PlayerAnimType.Fall;
            }
            else if ((DateTime.UtcNow - this.animSetOn).TotalMilliseconds > GameEngine.AnimationMillisecondInterval)
            {
                this.CurrentAnim = PlayerAnimType.Idle;
            }
        }

        public override void OnKeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (this.Move(-1, 0))
                    {
                        if (this.ScreenX == ScreenXLeftLimit)
                        {
                            this.Map.X = Math.Max(0, this.Map.X - 1);
                        }
                    }

                    if (!this.IsFalling && this.jump == 0)
                    {
                        this.CurrentAnim = PlayerAnimType.Walk;
                        this.animSetOn = DateTime.UtcNow;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (this.Move(1, 0))
                    {
                        if (this.ScreenX == this.Map.Width - ScreenXRightLimit)
                        {
                            this.Map.X = this.Map.X + 1;
                        }
                    }

                    if (!this.IsFalling && this.jump == 0)
                    {
                        this.CurrentAnim = PlayerAnimType.Walk;
                        this.animSetOn = DateTime.UtcNow;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (!this.IsFalling && this.jump == 0)
                    {
                        this.jump = JumpLong;
                        this.GravityDisabled = true;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    this.jump = 0;
                    this.GravityDisabled = false;
                    break;
                case ConsoleKey.A:
                    this.Map.X = Math.Max(0, this.Map.X - 1);
                    break;
                case ConsoleKey.D:
                    this.Map.X = Math.Max(0, this.Map.X + 1);
                    break;
                default:
                    break;
            }
        }
    }
}