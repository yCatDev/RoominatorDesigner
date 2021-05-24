namespace Logic.Core
{
    
    public enum Direction{
        Horizontal = 0,
        Vertical = 1
    }
    
    public class Window
    {
        public float Width;
        public float Height;
        public Direction Direction = 0;
        internal const float BaseWidthConstant = 10;
    }
}