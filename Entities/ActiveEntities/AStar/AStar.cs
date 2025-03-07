using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using System.Numerics;
using IVect = Short_Tools.General.ShortIntVector2;
using System.Reflection;
using System.Runtime.Intrinsics;

#pragma warning disable CS8618 // must contain value when exciting constructor -> i trust bayl on this one



namespace Base_Building_Game.Entities.AStar
{
    internal class AStar
    {

        Func<int, int, bool> Walkable;

        QuadTree visitedNodes;
        List<AStarNode> toVisitNodes;
        public bool useDiagonals { private get; set; }
        public int maxDist { private get; set; }


        public AStar(Func<int, int, bool> Walkable, int maxDist = 30, bool useDiagonals = true)
        {
            this.Walkable = Walkable;
            this.useDiagonals = useDiagonals;
            this.maxDist = maxDist;
        }





        private void UpdateTile(int x, int y, float pathLength, AStarNode parent, Vector2 end, Vector2 start)
        {
            if (Math.Abs(x - start.X) > maxDist || Math.Abs(y - start.Y) > maxDist) { return; }
            if (!Walkable(x, y)) { return; }


            AStarNode? foundNode = visitedNodes.GetValue(x, y);
            //foreach (AStarNode node in visitedNodes)
            //{
            //    if (node.x == x && node.y == y)
            //    {
            //        if (node.pathLength <= pathLength) { return; }
            //
            //        foundNode = node;
            //        break;
            //    }
            //}
            if (foundNode is not null)
            {
                if (foundNode.pathLength > pathLength)
                {
                    foundNode.pathLength = pathLength;
                    foundNode.parent = parent;
                }
            }
            else
            {
                AStarNode newNode = new AStarNode(x, y, parent, pathLength, end);

                visitedNodes.AddValue(newNode);
                toVisitNodes.Add(newNode);
            }


        }








        public Queue<Vector2> GetPath(Vector2 start, Vector2 end)
        {
            AStarNode startNode = new AStarNode(start, null, 0f, end);

            visitedNodes = new QuadTree(startNode);
            toVisitNodes = new List<AStarNode>() { startNode };

            while (toVisitNodes.Count > 0)
            {
                AStarNode? smallestNode = null;
                float smallestNodeValue = -1;


                foreach (AStarNode node in toVisitNodes) // <- next point to fix
                {
                    float currentNodeValue = node.heuristic + node.pathLength;

                    if (smallestNode is null || smallestNodeValue > currentNodeValue)
                    {
                        smallestNode = node;
                        smallestNodeValue = currentNodeValue;
                    }
                }
                if (smallestNode.x == (int)end.X && smallestNode.y == (int)end.Y)
                {
                    Queue<Vector2> path = new Queue<Vector2>();

                    while (true)
                    {
                        path.Enqueue(new Vector2(smallestNode.x, smallestNode.y));
                        smallestNode = smallestNode.parent;
                        if (smallestNode is null) { break; }
                    }
                    path = new Queue<Vector2>(path.Reverse());
                    return path;
                }



                UpdateTile(smallestNode.x + 1, smallestNode.y, smallestNode.pathLength + 1, smallestNode, end, start);
                UpdateTile(smallestNode.x - 1, smallestNode.y, smallestNode.pathLength + 1, smallestNode, end, start);
                UpdateTile(smallestNode.x, smallestNode.y + 1, smallestNode.pathLength + 1, smallestNode, end, start);
                UpdateTile(smallestNode.x, smallestNode.y - 1, smallestNode.pathLength + 1, smallestNode, end, start);

                if (useDiagonals)
                {
                    UpdateTile(smallestNode.x + 1, smallestNode.y + 1, smallestNode.pathLength + 1.41f, smallestNode, end, start);
                    UpdateTile(smallestNode.x - 1, smallestNode.y + 1, smallestNode.pathLength + 1.41f, smallestNode, end, start);
                    UpdateTile(smallestNode.x + 1, smallestNode.y - 1, smallestNode.pathLength + 1.41f, smallestNode, end, start);
                    UpdateTile(smallestNode.x - 1, smallestNode.y - 1, smallestNode.pathLength + 1.41f, smallestNode, end, start);
                }


                toVisitNodes.Remove(smallestNode);
            }

            return new Queue<Vector2>();
        }



    }


    internal class AStarNode
    {
        public int x;
        public int y;
        public float pathLength;
        public AStarNode? parent;


        public AStarNode(Vector2 inp, AStarNode? parent, float pathLength, Vector2 end)
        {
            x = (int)inp.X; y = (int)inp.Y;
            this.pathLength = pathLength;
            this.parent = parent;
            GenerateHeuristic(end);
        }
        public AStarNode(int x, int y, AStarNode? parent, float pathLength, Vector2 end)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
            this.pathLength = pathLength;
            GenerateHeuristic(end);
        }


        public float heuristic;

        private float GenerateHeuristic(Vector2 end)
        {
            return (end - new Vector2(x, y)).Length();
        }
    }
}
