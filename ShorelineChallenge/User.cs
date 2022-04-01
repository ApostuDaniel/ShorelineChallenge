using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorelineChallenge
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Friends { get; set; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            Friends = new List<int>();
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
