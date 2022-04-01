using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorelineChallenge
{
    public class SocialNetwork
    {
        public List<User> Users { get; set; }

        public SocialNetwork()
        {
            Users = new List<User>();
        }

        /// <summary>
        /// Adds an newUser to the network.
        /// </summary>
        /// <param name="newUser">The new user to be added</param>
        public void AddUser(User newUser)
        {
            if (newUser == null) throw new ArgumentNullException(nameof(newUser));

            if (Users.Contains(newUser)) throw new ArgumentException("Duplicate users in network");

            Users.Add(newUser);
        }

        /// <summary>
        /// Adds a list of users to the network
        /// </summary>
        /// <param name="users"></param>
        public void AddUsers(List<User> users)
        {
            foreach (var user in users)
            {
                AddUser(user);
            }
        }

        /// <summary>
        /// Adds userAId in the set of friends of userB and and userBId in the set of friends of userB
        /// </summary>
        /// <param name="userAId">id of userA</param>
        /// <param name="userBId">id of UserB</param>
        public void AddFriendship(int userAId, int userBId)
        {
            User userA = Users.FirstOrDefault<User>(user => user.Id == userAId);
            User userB = Users.FirstOrDefault<User>(user => user.Id == userBId);

            if (userA == null) throw new ArgumentException("user with id " + userAId + " isn't in the network");
            if (userB == null) throw new ArgumentException("user with id " + userBId + " isn't in the network");

            userA.Friends.Add(userBId);
            userB.Friends.Add(userAId);
        }

        /// <summary>
        /// For each key in the dictionary, create a frienship relation between the user identified as the key
        /// and each user in the list identified as the value of the dictionary
        /// </summary>
        /// <param name="relations">A dicitonary in which the keys are user ids and the values are lists of user ids</param>
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

        /// <summary>
        /// Find the shortest path between a startUser and endUser in the network.
        /// We can model this problem with the problem of finding the shortest path between two nodes in an undirected, unweighted graph
        /// The problem can be solved doing a BFS traversal of the graph from the starting node until we find the destination node, and then
        /// tracing back the route.
        /// </summary>
        /// <param name="startUserId">The id of the user from which we start the traversal</param>
        /// <param name="endUserId">The id of the user where the traversal should finish</param>
        /// <returns>An empty list if there is no path between the start and the end user, or a list of Users representing that path</returns>
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

        /// <summary>
        /// A BFS algorithm that starts from the User with the Id startUserId and stops when we reach a user with the Id value of endUserId
        /// or when we finish visiting the node, and stores the previous user Id in the traversal for each user Id.
        /// </summary>
        /// <param name="startUserId">id of the start User</param>
        /// <param name="endUserId">id of the end User</param>
        /// <param name="predecesor">Dictionary in which for each key, value is the id of the previous User in the traversal</param>
        /// <returns>true if there is a path between start and end, false otherwise</returns>
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
