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



namespace Base_Building_Game
{
    public static partial class General
    {
        
        public class AStar
        {
            Func<int, int, bool, bool> tileChecker { get; }
            IVect destination { get; set; }
            IVect start { get; set; }
            AStar_Node currentNode { get; set; }
            PriorityQueue<AStar_Node, int> nodesToVisit { get; } = new PriorityQueue<AStar_Node, int>();
            List<AStar_Node> visitedNodes { get; } = new List<AStar_Node>();

            public AStar(Func<int, int, bool, bool> tileChecker, IVect destination, IVect start)
            {
                this.tileChecker = tileChecker;
                this.destination = destination;
                this.start = start;               
            }

            public Stack<Vector2>? GetPath(int maxDist)
            {
                bool found = false;
                nodesToVisit.Enqueue(new AStar_Node(start, 0),0);
                while (nodesToVisit.Count > 0)
                {
                    

                    AStar_Node currentNode = nodesToVisit.Dequeue();
                    int temp = currentNode.ComputeHeuristic(destination);
                    if (currentNode.ComputeHeuristic(destination) + currentNode.distanceHere > maxDist)
                    {
                        continue;
                    }
                    //Checks all nodes around it in a 3x3 square (not including itself).
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            if (x == 0 && y == 0)
                            {
                                continue;
                            }
                            IVect checkedPos = new IVect(currentNode.pos.x + x, currentNode.pos.y + y);
                            //If that tile is valid according to the delegate, then we add it to the priority queue.
                            if (tileChecker(checkedPos.x, checkedPos.y, false))
                            {
                                bool hasVisited = false;
                                foreach (AStar_Node node in visitedNodes)
                                {
                                    if (node.pos == checkedPos)
                                    {
                                        if (node.distanceHere > (currentNode.distanceHere + (x == 0 || y == 0 ? 1 : 2)))
                                        {
                                            node.distanceHere = currentNode.distanceHere + (x == 0 || y == 0 ? 1 : 2);
                                            node.lastNode = currentNode;
                                            nodesToVisit.Enqueue(node, node.distanceHere + node.ComputeHeuristic(destination));

                                        }
                                        hasVisited = true;
                                        break;
                                    }
                                }
                                
                                if (!hasVisited)
                                {
                                    AStar_Node newNode = new AStar_Node(checkedPos, currentNode.distanceHere + (x == 0 || y == 0 ? 1 : 2));
                                    newNode.lastNode = currentNode;
                                    nodesToVisit.Enqueue(newNode, newNode.distanceHere + newNode.ComputeHeuristic(destination));
                                    visitedNodes.Add(newNode);
                                }

                                if (checkedPos == destination)
                                {
                                    found = true;
                                }
                                
                            }
                            
                        }
                        if (found)
                        {
                            break;
                        }

                    }
                    if (found)
                    {
                        break;
                    }
                    
                }
                
                if (!found)
                {
                    return null;
                }
                
                Stack<Vector2> movements = new Stack<Vector2>();
                if (visitedNodes.Count != 0)
                {
                    //AStar_Node checking = visitedNodes[visitedNodes.Count - 1];
                    AStar_Node checking = new AStar_Node(new IVect(),1000);
                    int best = maxDist;
                    foreach (AStar_Node node in visitedNodes)
                    {
                        if (destination == node.pos && node.distanceHere < best)
                        {
                            checking = node;
                            best = node.distanceHere;
                        }
                    }

                    while (checking.lastNode != null)
                    {
                        movements.Push(checking.pos);
                        checking = checking.lastNode;
                    } 
                }
                return movements;
                
                
            }

        }
        private class AStar_Node
        {
            public IVect pos;
            public int distanceHere { get; set; } = 0;
            public AStar_Node? lastNode { get; set; } = null;

            public AStar_Node(IVect pos,int distanceHere)
            {
                this.pos = pos;
                this.distanceHere = distanceHere;
            }
            public int ComputeHeuristic(IVect endPos)
            {
                return (endPos - pos).Mag();
            }
        }
    }
}
