namespace ConsoleDraw.Interaction
{
    public interface IInput
    {
        string Get(string name);
        void Respond(string v);
    }
}