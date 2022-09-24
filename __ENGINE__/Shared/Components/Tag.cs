namespace __ENGINE__.Shared.Components
{
    public class Tag : __ENGINE__Obj.Component
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