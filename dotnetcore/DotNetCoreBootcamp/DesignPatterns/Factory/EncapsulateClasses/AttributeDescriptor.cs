using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Factory.EncapsulateClasses
{
    public abstract class AttributeDescriptor
    {
        private readonly string _name;
        protected AttributeDescriptor(string name, Type t)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public static AttributeDescriptor ForInt(string name)
        {
            return new DefaultDescriptor(name, typeof(int));
        }

        public static AttributeDescriptor ForBool(string name)
        {
            return new BooleanDescriptor(name);
        }

        public static AttributeDescriptor ForDatetime(string name)
        {
            return new DefaultDescriptor(name, typeof(DateTime));
        }
    }

    sealed class BooleanDescriptor : AttributeDescriptor        
    {
        public BooleanDescriptor(string name) : base(name, typeof(bool))
        {
        }
    }

    sealed class DefaultDescriptor : AttributeDescriptor
    {
        public DefaultDescriptor(string name, Type t) : base(name, t)
        {
        }
    }



}
