namespace Logic.Core
{
    public class Door
    {
        public float Width;
        public float Height;
        public Direction Direction = 0;
        internal Wall Wall;
        internal const float BaseWidthConstant = 10;
    }
}