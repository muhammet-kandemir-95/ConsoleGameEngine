namespace LessonConsoleGame.Engine
{
    public class ValidationService
    {
        public UIEngine UIEngine { get; private set; }

        public string ConsoleSizeValidationMessage => $"PLEASE CHANGE CONSOLE SIZE {Console.WindowWidth}/{Console.WindowHeight} TO {this.UIEngine.TotalWidth}/{this.UIEngine.TotalHeight}!";

        public void Initialize(UIEngine uiEngine)
        {
            this.UIEngine = uiEngine;
        }

        public bool ValidateConsoleSize()
        {
            return this.UIEngine.TotalWidth < Console.WindowWidth && this.UIEngine.TotalHeight < Console.WindowHeight;
        }}
}