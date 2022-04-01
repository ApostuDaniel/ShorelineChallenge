using System;
using System.Collections.Generic;

namespace ShorelineChallenge
{
    static class Program
    {
        static void Main(string[] args)
        {
            SocialNetwork network = new SocialNetwork();
            network.AddUsers(new List<User> { 
            new User(0, "0"),
            new User(1, "1"),
            new User(2, "2"),
            new User(3, "3"),
            new User(4, "4"),
            new User(5, "5"),
            new User(6, "6"),
            new User(7, "7"),
            });

            network.AddFriendships(new Dictionary<int, List<int>> {
                {0, new List<int>{1, 3 } },
                {1,  new List<int>{2} },
                {3,  new List<int>{7, 4 } },
                {4,  new List<int>{7, 6, 5 }},
                {6,  new List<int>{5 }}
             }) ;

            List<User> friendChain = network.ShortestFriendsChain(2, 6);

            foreach (var user in friendChain)
            {
                Console.WriteLine(user);
            }
        }
    }
}
