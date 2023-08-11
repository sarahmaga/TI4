using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PathfindingBFS : MonoBehaviour
{

    private int[] width = { -17, 19 };
    private int[] height = { -12, 7 };

    private Rigidbody2D rb;

    public IDictionary<Vector3, bool> walkablePositions = new Dictionary<Vector3, bool>();
    public IDictionary<Vector3, string> obstacles;
    IDictionary<Vector3, Vector3> nodeParents = new Dictionary<Vector3, Vector3>();
    public IDictionary<Vector3, GameObject> nodeReference = new Dictionary<Vector3, GameObject>();

    IList<Vector3> path;
    bool moveCube = false, lastDirection;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        // obstacles = nodeMap.obstacles;
        // walkablePositions = nodeMap.walkablePositions;

        rb = GetComponent<Rigidbody2D>();

        InitializeNodeNetwork();    
        
        path = FindShortestPath();
        
        moveCube = true;
        lastDirection = false; // false if not flipped; else ...
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

                Vector3 targetPos = path[i];
                Vector3 direction = (targetPos - transform.position).normalized;

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

                if (i >= 0 /*&& (normalizedPositionLeft.Equals(path[i]) || normalizedPositionRight.Equals(path[i]))*/)
                    i--;
                if (i < 0)
                {
                    moveCube = false;
                    nodeParents.Clear();
                    path = FindShortestPath();
                    i = path != null ? path.Count - 1 : -1;
                }
            }
        } else
        {
            nodeParents.Clear();
            path = FindShortestPath();
            i = path != null ? path.Count - 1 : -1;
        }
    }

    Vector3 FindShortestPathBFS(Vector3 startPosition, Vector3 goalPosition)
    {

        uint nodeVisitCount = 0;

        Queue<Vector3> queue = new Queue<Vector3>();
        HashSet<Vector3> exploredNodes = new HashSet<Vector3>();
        queue.Enqueue(startPosition);

        while (queue.Count != 0)
        {
            Vector3 currentNode = queue.Dequeue();
            nodeVisitCount++;
            if (currentNode == goalPosition)
            {
                moveCube = true;
                return currentNode;
            }

            IList<Vector3> nodes = GetWalkableNodes(currentNode);

            foreach (Vector3 node in nodes)
            {
                if (!exploredNodes.Contains(node))
                {
                    //Mark the node as explored
                    exploredNodes.Add(node);

                    //Store a reference to the previous node
                    nodeParents.Add(node, currentNode);

                    //Add this to the queue of nodes to examine
                    queue.Enqueue(node);
                }
            }
        }
        moveCube = false;
        return startPosition;
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
        player = FindShortestPathBFS(thisPosNormalized, playerPos);

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
}
