using System.Collections.Generic;

namespace DesignPatterns.Composite.Example1
{
    /// <summary>
    /// The Composite class represents the complex components that may have
    /// children. Usually, the Composite objects delegate the actual work to
    /// their children and then "sum-up" the result.
    /// </summary>
    public class Composite : Component
    {
        protected List<Component> _children = new List<Component>();

        public override void Add(Component component)
        {
            this._children.Add(component);
        }

        public override void Remove(Component component)
        {
            this._children.Remove(component);
        }

        /// <summary>
        /// The Composite executes its primary logic in a particular way. It
        /// traverses recursively through all its children, collecting and
        /// summing their results. Since the composite's children pass these
        /// calls to their children and so forth, the whole object tree is
        /// traversed as a result.
        /// </summary>
        /// <returns></returns>
        public override string Operation()
        {
            int i = 0;
            string result = "Branch(";

            foreach (Component component in this._children)
            {
                result += component.Operation();
                if (i != this._children.Count - 1)
                {
                    result += "+";
                }
                i++;
            }

            return result + ")";
        }
    }
}
