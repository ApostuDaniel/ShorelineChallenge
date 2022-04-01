using NUnit.Framework;
using ShorelineChallenge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShorelineTests
{
    [TestFixture]
    public class UserTests
    {
        

        [TestCase(-1, "")]
        [TestCase(-4, "a name")]
        [TestCase(-5, null)]
        public void InitializeUserNegativeId(int id, string name)
        {
            Assert.Throws<ArgumentException>(() => new User(id, name));
        }

        [TestCase(0, null)]
        [TestCase(1, null)]
        public void InitializeUserNullName(int id, string name)
        {
            Assert.Throws<ArgumentNullException>(() => new User(id, name));
        }
        
        [TestCase(0, "0", 0, "0")]
        [TestCase(0, "0", 0, "1")]
        public void TestEqualsTrue(int id1, string name1, int id2, string name2)
        {
            Assert.IsTrue(new User(id1, name1).Equals(new User(id2, name2)));
        }

        [TestCase(0, "0", 1, "0")]
        [TestCase(0, "0", 1, "1")]
        public void TestEqualsFalse(int id1, string name1, int id2, string name2)
        {
            Assert.IsFalse(new User(id1, name1).Equals(new User(id2, name2)));
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(new User(0, "0").ToString(), "0 : 0");
        }
    }

    [TestFixture]
    public class NetworkTests
    {
        SocialNetwork network;

        SocialNetwork unconnectedNetwork;

        [SetUp]
        public void Setup()
        {
            network = new();
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

            unconnectedNetwork = new();
            unconnectedNetwork.AddUsers(new List<User> {
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
             });

            unconnectedNetwork.AddFriendships(new Dictionary<int, List<int>> {
                {0, new List<int>{1, 3 } },
                {1,  new List<int>{2} },
                {3,  new List<int>{7, 4 } },
                {6,  new List<int>{5 }}
             });
        }

        [Test]
        public void AddUserNull()
        {
            Assert.Throws<ArgumentNullException>(() => network.AddUser(null));
        }

        [Test]
        public void AddUserDuplicate()
        {
            Assert.Throws<ArgumentException>(() => network.AddUser(new User(2, "name doesn't matter")));
        }

        [TestCase(12, 14)]
        [TestCase(0, 14)]
        [TestCase(12, 0)]
        public void AddFriendshipUserInexistent(int userIdA, int userIdB)
        {
            Assert.Throws<ArgumentException>(() => network.AddFriendship(userIdA, userIdB));
        }


        [TestCase(12, 14)]
        [TestCase(0, 14)]
        [TestCase(12, 0)]
        public void GetFriendsChainBetweenInexistentUsers(int userIdA, int userIdB)
        {
            Assert.Throws<ArgumentException>(() => network.ShortestFriendsChain(userIdA, userIdB));
        }

        [Test]
        public void NoChainBetweenUnconectedUsers()
        {
            Assert.IsTrue(unconnectedNetwork.ShortestFriendsChain(2, 5).Count == 0);
        }

        [Test]
        public void ValidShortestChain()
        {
            List<User> shortestLinkBetweenUsers = network.ShortestFriendsChain(2, 6);
            List<User> path1 = new List<User>
            {
                 new User(2, "2"),
                 new User(1, "1"),
                 new User(0, "0"),
                 new User(3, "3"),
                 new User(4, "4"),
                 new User(6, "6"),
            };

            List<User> path2 = new List<User>
            {
                 new User(2, "2"),
                 new User(1, "1"),
                 new User(0, "0"),
                 new User(3, "3"),
                 new User(7, "7"),
                 new User(6, "6"),
            };

            Assert.IsTrue(shortestLinkBetweenUsers.SequenceEqual(path1) || shortestLinkBetweenUsers.SequenceEqual(path2));

        }
    }
}