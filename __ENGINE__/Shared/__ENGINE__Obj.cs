namespace __ENGINE__.Shared
{
    public class __ENGINE__Obj
    {
        private readonly HashSet<Component> _components;
        public bool markedForRemoval;

        public __ENGINE__Obj()
        {
            _components = new HashSet<Component>();
        }

        public void update()
        {
            foreach (Component component in _components)
            {
                component.update(this);
            }
        }

        public void render()
        {
            foreach (Component component in _components)
            {
                component.render(this);
            }
        }

        public void collide(__ENGINE__Obj other)
        {
            foreach (Component component in _components)
            {
                component.collide(this, other);
            }
        }

        public void add(Component component)
        {
            _components.Add(component);
        }
        
        private bool comp_finder<T>(Component comp)
        {
            return typeof(T) == comp.GetType();
        }

        private readonly Dictionary<Type, Component> _cache = new();

        public T get<T>() where T : Component
        {
            if (_cache.TryGetValue(typeof(T), out Component comp))
            {
                return (T) comp;
            }
            
            T val = (T)_components.FirstOrDefault(comp_finder<T>, null);
            _cache[typeof(T)] = val;
            return val;
        }
        
        public bool has<T>() where T : Component
        {
            return _components.Any(comp_finder<T>);
        }

        public class Component
        {
            public virtual void update(__ENGINE__Obj objIn)
            {
                
            }

            public virtual void render(__ENGINE__Obj objIn)
            {
                
            }
            
            public virtual void collide(__ENGINE__Obj objIn, __ENGINE__Obj other)
            {
                
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode();
            }
        }
    }
}