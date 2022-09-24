﻿namespace __ENGINE__.Shared
{
    public class EngineObj
    {
        private readonly HashSet<Component> _components;
        public bool markedForRemoval;

        public EngineObj()
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

        public void collide(EngineObj other)
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
        
        private bool compFinder<T>(Component comp)
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
            
            T val = (T)_components.FirstOrDefault(compFinder<T>, null);
            _cache[typeof(T)] = val;
            return val;
        }
        
        public bool has<T>() where T : Component
        {
            return _components.Any(compFinder<T>);
        }

        public class Component
        {
            public virtual void update(EngineObj objIn)
            {
                
            }

            public virtual void render(EngineObj objIn)
            {
                
            }
            
            public virtual void collide(EngineObj objIn, EngineObj other)
            {
                
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode();
            }
        }
    }
}