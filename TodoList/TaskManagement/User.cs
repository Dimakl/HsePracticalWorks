using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class User
    {

        public static List<User> Users { get; }

        public string Name { get; set; }

        static User()
        {
            Users = new List<User>();
        }

        private User(string name)
        {
            Name = name;
        }

        public static void AddUser(string name)
        {
            Users.Add(new User(name));
        }


        public static void DeleteUser(string name)
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Name == name)
                    Users.RemoveAt(i);

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
