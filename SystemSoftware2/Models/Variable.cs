using System;
using SystemSoftware1.Constants;

namespace SystemSoftware1.Models
{
    public class Variable
    {
        public int Id { get; private set; }
        public string DataType { get; private set; }
        public string Name { get; private set; }
        public Variable(int id, string dataType, string name)
        {
            Id = id;
            DataType = dataType;
            Name = name;
        }
        public override string ToString()
        {
            return $"ID = '{Id}' Variable of type '{DataType}' with name '{Name}'";
        }
    }
}
