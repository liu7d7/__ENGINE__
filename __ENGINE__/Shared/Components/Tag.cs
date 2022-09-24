namespace __ENGINE__.Shared.Components
{
    public class Tag : EngineObj.Component
    {
        public int id;
        public string name;

        public Tag(int id, string name = "")
        {
            this.id = id;
            this.name = name;
        }
    }
}