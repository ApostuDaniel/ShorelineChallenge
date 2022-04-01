using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorelineChallenge
{
    class SocialNetwork
    {
        public List<User> Users { get; set; }

        public SocialNetwork()
        {
            Users = new List<User>();
        }

        public void AddUser(User newUser)
        {
            if (newUser == null) throw new ArgumentNullException(nameof(newUser));
            if (string.IsNullOrEmpty(newUser.Name)) throw new ArgumentException("Name of user can't be null");

            if (Users.Contains(newUser)) throw new ArgumentException("Duplicate users in network");

            Users.Add(newUser);
        }

        public void AddUsers(List<User> users)
        {
            foreach (var user in users)
            {
                AddUser(user);
            }
        }

        public void AddFriendship(int userAId, int userBId)
        {
            User userA = Users.FirstOrDefault<User>(user => user.Id == userAId);
            User userB = Users.FirstOrDefault<User>(user => user.Id == userBId);

            if (userA == null) throw new ArgumentException("user with id " + userAId + " isn't in the network");
            if (userB == null) throw new ArgumentException("user with id " + userBId + " isn't in the network");

            userA.Friends.Add(userBId);
            userB.Friends.Add(userAId);
        }

        public void AddFriendships(Dictionary<int, List<int>> relations)
        {
            foreach (var relation in relations)
            {
                foreach (var userId in relation.Value)
                {
                    AddFriendship(relation.Key, userId);
                }
            }
        }

        public List<User> ShortestFriendsChain(int startUserId, int endUserId)
        {
            if (Users.FirstOrDefault<User>(user => user.Id == startUserId) == null) throw new ArgumentException("startUserId isn't in the network");
            if (Users.FirstOrDefault<User>(user => user.Id == endUserId) == null) throw new ArgumentException("endUserId isn't in the network");

            Dictionary<int, int> pred = new();
            List<User> chain = new();

            if (!BFS(startUserId, endUserId, pred)) return chain;

            User currentUserInChain= Users.FirstOrDefault<User>(user => user.Id == endUserId);
            chain.Add(currentUserInChain);

            while(pred[currentUserInChain.Id] != -1)
            {
                currentUserInChain = Users.FirstOrDefault<User>(user => user.Id == pred[currentUserInChain.Id]);
                chain.Add(currentUserInChain);
            }

            chain.Reverse();

            return chain;
        }

        private bool BFS(int startUserId, int endUserId, Dictionary<int,int> predecesor)
        {
            Queue<int> bfsQueue = new Queue<int>();
            Dictionary<int, bool> visited = new Dictionary<int, bool>();

            foreach (User user in Users)
            {
                visited.Add(user.Id, false);
                predecesor.Add(user.Id, -1);
            }

            visited[startUserId] = true;
            bfsQueue.Enqueue(startUserId);

            while(bfsQueue.Count > 0)
            {
                int currentId = bfsQueue.Dequeue();
                User currentUser = Users.FirstOrDefault<User>(user => user.Id == currentId);
                foreach (var userId in currentUser.Friends)
                {
                    if(!visited[userId])
                    {
                        visited[userId] = true;
                        predecesor[userId] = currentId;
                        bfsQueue.Enqueue(userId);
                    }

                    if (userId == endUserId) return true;
                }
            }

            return false;
        }
    }
}
