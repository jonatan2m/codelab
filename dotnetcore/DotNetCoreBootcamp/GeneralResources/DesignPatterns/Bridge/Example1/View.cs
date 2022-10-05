using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Bridge.Example1
{
    public abstract class View
    {
        protected IResource resource;
        public View(IResource resource)
        {
            this.resource = resource;
        }

        public abstract string Show();
    }
}
