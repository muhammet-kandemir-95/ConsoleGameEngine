using System;
using System.Drawing;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public interface IGravity
    {
        bool GravityDisabled { get; set; }
        public DateTime FallingSetOn { get; set; }
    }
}