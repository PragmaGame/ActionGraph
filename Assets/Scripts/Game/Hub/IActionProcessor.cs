namespace Game.Hub
{
    public interface IActionProcessor
    {
        public void Construct();
        public void Process();
    }
}