using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorelineChallenge
{
    public class User
    {
        private int id;
        public int Id {
            get => id;
            set {
                if (value < 0) throw new ArgumentException("Id can't be smaller than 0");
                id = value;
            } }
        private string name;
        public string Name { get => name;
            set {
                name = value ?? throw new ArgumentNullException(nameof(value));
            } }

        public HashSet<int> Friends { get;}

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            Friends = new();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Id.Equals((obj as User).Id);  
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id + " : " + Name;
        }
    }
}
