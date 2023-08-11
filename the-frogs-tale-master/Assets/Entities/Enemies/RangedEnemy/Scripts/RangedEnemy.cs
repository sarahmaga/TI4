using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Priority_Queue;

public class RangedEnemy : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;
    public Transform firingPoint;

    private Animator animator;

    public float speed = 3.5f;
    public float distanceToShoot = 10f;
    public float distanceToStop = 7f;

    private float fireRate = 2f;
    private float timeToFire = 0f;


    private bool lastDirection;
    private Vector3 direction;

    private int[] width = { -17, 19 };
    private int[] height = { -12, 7 };

    // A*

    public IDictionary<Vector3, bool> walkablePositions = new Dictionary<Vector3, bool>();
    public IDictionary<Vector3, string> obstacles;
    IDictionary<Vector3, Vector3> nodeParents = new Dictionary<Vector3, Vector3>();
    public IDictionary<Vector3, GameObject> nodeReference = new Dictionary<Vector3, GameObject>();

    bool moveCube = false;
    int i;

    IList<Vector3> path;

    void InitializeNodeNetwork()
    {
        obstacles = getObstacles();

        for (int i = width[0]; i < width[1]; i++)
        {
            for (int j = height[0]; j < height[1]; j++)
            {
                Vector3 newPosition = new Vector3(i + 0.5f, j + 0.5f);

                if (obstacles.TryGetValue(newPosition, out _))
                {
                    walkablePositions.Add(new KeyValuePair<Vector3, bool>(newPosition, false));
                }
                else
                {
                    walkablePositions.Add(new KeyValuePair<Vector3, bool>(newPosition, true));
                }

                nodeReference.Add(newPosition, null);
            }
        }
    }

    Dictionary<Vector3, string> getObstacles()
    {
        Dictionary<Vector3, string> res = new Dictionary<Vector3, string>
        {
            // BENCHES

            { new Vector3(-9.5f, 3.5f), null },
            { new Vector3(-9.5f, 2.5f), null },
            { new Vector3(-9.5f, 1.5f), null },

            { new Vector3(-6.5f, 3.5f), null },
            { new Vector3(-6.5f, 2.5f), null },
            { new Vector3(-6.5f, 1.5f), null },

            { new Vector3(-12.5f, -0.5f), null },
            { new Vector3(-12.5f, -1.5f), null },
            { new Vector3(-12.5f, -2.5f), null },

            { new Vector3(-9.5f, -0.5f), null },
            { new Vector3(-9.5f, -1.5f), null },
            { new Vector3(-9.5f, -2.5f), null },

            { new Vector3(-6.5f, -0.5f), null },
            { new Vector3(-6.5f, -1.5f), null },
            { new Vector3(-6.5f, -2.5f), null },

            { new Vector3(-9.5f, -4.5f), null },
            { new Vector3(-9.5f, -5.5f), null },
            { new Vector3(-9.5f, -6.5f), null },

            { new Vector3(-6.5f, -4.5f), null },
            { new Vector3(-6.5f, -5.5f), null },
            { new Vector3(-6.5f, -6.5f), null },
        
            // BROKEN FOUNTAIN IN THE MIDDLE

            { new Vector3(-1.5f, -8.5f), null },
            { new Vector3(-0.5f, -8.5f), null },
            { new Vector3(0.5f, -8.5f), null },
            { new Vector3(-1.5f, -7.5f), null },
            { new Vector3(-0.5f, -7.5f), null },
            { new Vector3(0.5f, -7.5f), null },

            // BARRIER BLOCKING GRAVEYARD

            // { new Vector3(6.5f, -4.5f), null },
            // { new Vector3(6.5f, -3.5f), null },
            // { new Vector3(6.5f, -2.5f), null },
            // { new Vector3(6.5f, -1.5f), null },
            // { new Vector3(6.5f, -0.5f), null },
            // { new Vector3(6.5f, 1.5f), null },
            // { new Vector3(6.5f, 2.5f), null },
            // { new Vector3(6.5f, 3.5f), null },
            // { new Vector3(6.5f, 4.5f), null },
            // { new Vector3(6.5f, 5.5f), null },
            // { new Vector3(6.5f, 6.5f), null },

            // { new Vector3(7.5f, -4.5f), null },
            // { new Vector3(7.5f, -3.5f), null },
            // { new Vector3(7.5f, -2.5f), null },
            // { new Vector3(7.5f, -1.5f), null },
            // { new Vector3(7.5f, -0.5f), null },
            // { new Vector3(7.5f, 1.5f), null },
            // { new Vector3(7.5f, 2.5f), null },
            // { new Vector3(7.5f, 3.5f), null },
            // { new Vector3(7.5f, 4.5f), null },
            // { new Vector3(7.5f, 5.5f), null },
            // { new Vector3(7.5f, 6.5f), null },

            // TOMBSTONES AND ROCKS

            { new Vector3(9.5f, 2.5f), null },
            { new Vector3(9.5f, 3.5f), null },

            { new Vector3(9.5f, -0.5f), null },
            { new Vector3(9.5f, 0.5f), null },

            { new Vector3(12.5f, -0.5f), null },
            { new Vector3(12.5f, 0.5f), null },

            { new Vector3(15.5f, -0.5f), null },
            { new Vector3(15.5f, 0.5f), null },

            // MONUMENTS

            { new Vector3(9.5f, -4.5f), null },
            { new Vector3(9.5f, -3.5f), null },
            { new Vector3(9.5f, -2.5f), null },

            { new Vector3(12.5f, -4.5f), null },
            { new Vector3(12.5f, -3.5f), null },
            { new Vector3(12.5f, -2.5f), null },

            { new Vector3(15.5f, -4.5f), null },
            { new Vector3(15.5f, -3.5f), null },
            { new Vector3(15.5f, -2.5f), null },

        };

        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        InitializeNodeNetwork();

        path = FindShortestPath();

        moveCube = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        timeToFire = 0f;

        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (path != null)
        {
            if (moveCube)
            {
                float speed = 5f;
                float step = Time.deltaTime * speed;

                // set animation

                animator.SetBool("walking", true);

                Vector3 targetPos = target.position;
                direction = (targetPos - transform.position).normalized;

                bool flipped = direction.x < 0f;
                if (direction.x == 0f)
                    transform.rotation = Quaternion.Euler(new Vector3(0f, lastDirection ? 180f : 0f, 0f));
                else
                    transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));

                lastDirection = flipped;

                // move

                transform.position = Vector3.MoveTowards(transform.position, path[i], step);

                Vector3 normalizedPositionLeft = new Vector3(Mathf.Floor(transform.position.x) + 0.5f,
                                            Mathf.Floor(transform.position.y) + 0.5f);
                Vector3 normalizedPositionRight = new Vector3(Mathf.Ceil(transform.position.x) + 0.5f,
                                            Mathf.Ceil(transform.position.y) + 0.5f);

                if (i >= 5 /*&& (normalizedPositionLeft.Equals(path[i]) || normalizedPositionRight.Equals(path[i]))*/)
                {
                    animator.SetBool("walking", true);
                    if (i >= 7)
                        animator.SetBool("charging", false);
                    else
                        shoot();
                    i--;
                }
                if (i < 5)
                {
                    shoot();
                    animator.SetBool("walking", false);

                    nodeParents.Clear();
                    path = FindShortestPath();
                    i = path != null ? path.Count - 1 : -1;
                    moveCube = i >= 5;
                }
            } else
            {
                animator.SetBool("walking", false);

                nodeParents.Clear();
                path = FindShortestPath();
                i = path != null ? path.Count - 1 : -1;
                moveCube = i >= 5;
            }
        }
        else
        {
            nodeParents.Clear();
            path = FindShortestPath();
            i = path != null ? path.Count - 1 : -1;
            moveCube = i >= 5;
        }

        /*if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
        {
            // set animation
            
            animator.SetBool("walking", true);

            Vector3 targetPos = target.position;
            direction = (targetPos - transform.position).normalized;

            bool flipped = direction.x < 0f;
            if (direction.x == 0f)
                transform.rotation = Quaternion.Euler(new Vector3(0f, lastDirection ? 180f : 0f, 0f));
            else
                transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));

            lastDirection = flipped;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("walking", false);
        }

        if (Vector2.Distance(target.position, transform.position) <= distanceToShoot)
            shoot();
        else
            animator.SetBool("charging", false);*/
    }

    private void shoot()
    {
        if (timeToFire <= 0f)
        {
            float offset = direction.x > 0 ? 2f : -2f;

            // set animation
            
            animator.SetBool("charging", true);

            float waitForAnimation = 2f;
            while (waitForAnimation >= 0f) { waitForAnimation -= Time.deltaTime; }

            // instanciate projectile

            Instantiate(projectilePrefab,
                new Vector2(transform.position.x + offset, transform.position.y), Quaternion.identity);

            timeToFire = fireRate;
        }
        else
        {
            timeToFire -= Time.deltaTime;
        }
    }

    Vector3 FindShortestPathAStar(Vector3 startPosition, Vector3 goalPosition, string heuristic)
    {

        uint nodeVisitCount = 0;
        float timeNow = Time.realtimeSinceStartup;

        // A* tries to minimize f(x) = g(x) + h(x), where g(x) is the distance from the start to node "x" and
        //    h(x) is some heuristic that must be admissible, meaning it never overestimates the cost to the next node.
        //    There are formal logical proofs you can look up that determine how heuristics are and are not admissible.

        IEnumerable<Vector3> validNodes = walkablePositions
            .Where(x => x.Value == true)
            .Select(x => x.Key);

        // Represents h(x) or the score from whatever heuristic we're using
        IDictionary<Vector3, int> heuristicScore = new Dictionary<Vector3, int>();

        // Represents g(x) or the distance from start to node "x" (Same meaning as in Dijkstra's "distances")
        IDictionary<Vector3, int> distanceFromStart = new Dictionary<Vector3, int>();

        foreach (Vector3 vertex in validNodes)
        {
            heuristicScore.Add(new KeyValuePair<Vector3, int>(vertex, int.MaxValue));
            distanceFromStart.Add(new KeyValuePair<Vector3, int>(vertex, int.MaxValue));
        }

        heuristicScore[startPosition] = HeuristicCostEstimate(startPosition, goalPosition, heuristic);
        distanceFromStart[startPosition] = 0;

        // The item dequeued from a priority queue will always be the one with the lowest int value
        //    In this case we will input nodes with their calculated distances from the start g(x),
        //    so we will always take the node with the lowest distance from the queue.
        SimplePriorityQueue<Vector3, int> priorityQueue = new SimplePriorityQueue<Vector3, int>();
        priorityQueue.Enqueue(startPosition, heuristicScore[startPosition]);

        while (priorityQueue.Count > 0)
        {
            // Get the node with the least distance from the start
            Vector3 curr = priorityQueue.Dequeue();
            nodeVisitCount++;

            // If our current node is the goal then stop
            if (curr == goalPosition)
            {
                return goalPosition;
            }

            IList<Vector3> neighbors = GetWalkableNodes(curr);

            foreach (Vector3 node in neighbors)
            {
                // Get the distance so far, add it to the distance to the neighbor
                int currScore = distanceFromStart[curr];

                // If our distance to this neighbor is LESS than another calculated shortest path
                //    to this neighbor, set a new node parent and update the scores as our current
                //    best for the path so far.
                if (currScore < distanceFromStart[node])
                {
                    nodeParents[node] = curr;
                    distanceFromStart[node] = currScore;

                    int hScore = distanceFromStart[node] + HeuristicCostEstimate(node, goalPosition, heuristic);
                    heuristicScore[node] = hScore;

                    // If this node isn't already in the queue, make sure to add it. Since the
                    //    algorithm is always looking for the smallest distance, any existing entry
                    //    would have a higher priority anyway.
                    if (!priorityQueue.Contains(node))
                    {
                        priorityQueue.Enqueue(node, hScore);
                    }
                }
            }
        }
        return startPosition;
    }

    int EuclideanEstimate(Vector3 node, Vector3 goal)
    {
        return (int)Mathf.Sqrt(Mathf.Pow(node.x - goal.x, 2) +
            Mathf.Pow(node.y - goal.y, 2) +
            Mathf.Pow(node.z - goal.z, 2));
    }

    int ManhattanEstimate(Vector3 node, Vector3 goal)
    {
        return (int)(Mathf.Abs(node.x - goal.x) +
            Mathf.Abs(node.y - goal.y) +
            Mathf.Abs(node.z - goal.z));
    }

    int HeuristicCostEstimate(Vector3 node, Vector3 goal, string heuristic)
    {
        switch (heuristic)
        {
            case "euclidean":
                return EuclideanEstimate(node, goal);
            case "manhattan":
                return ManhattanEstimate(node, goal);
        }

        return -1;
    }

    IList<Vector3> GetWalkableNodes(Vector3 curr)
    {

        IList<Vector3> walkableNodes = new List<Vector3>();

        IList<Vector3> possibleNodes = new List<Vector3>() {
            new Vector3 (curr.x + 1, curr.y),
            new Vector3 (curr.x - 1, curr.y),
            new Vector3 (curr.x, curr.y + 1),
            new Vector3 (curr.x, curr.y - 1),
        };

        foreach (Vector3 node in possibleNodes)
        {
            if (CanMove(node))
            {
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    bool CanMove(Vector3 nextPosition)
    {
        return (walkablePositions.ContainsKey(nextPosition) && walkablePositions[nextPosition]);
    }

    IList<Vector3> FindShortestPath()
    {

        IList<Vector3> res = new List<Vector3>();
        Vector3 player;
        Vector3 tmp = GameObject.Find("Player").transform.localPosition;
        Vector3 thisPosNormalized = new Vector3((float)System.Math.Floor(transform.localPosition.x) + 0.5f,
                                    (float)System.Math.Floor(transform.localPosition.y) + 0.5f);
        Vector3 playerPos = new Vector3((float)System.Math.Floor(tmp.x) + 0.5f, (float)System.Math.Floor(tmp.y) + 0.5f);
        player = FindShortestPathAStar(thisPosNormalized, playerPos, "manhattan");

        if (player == thisPosNormalized || !nodeParents.ContainsKey(new Vector3(player.x, player.y)))
        {
            return null;
        }

        Vector3 curr = player;
        while (curr != thisPosNormalized)
        {
            res.Add(curr);
            curr = nodeParents[curr];
        }
        return res;
    }
}
