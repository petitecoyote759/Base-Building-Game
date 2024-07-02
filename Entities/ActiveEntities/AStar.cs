using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        
        public class AStar
        {
            Func<Tile,bool, bool> tileChecker { get; }
            IVect destination { get; set; }
            IVect start { get; set; }
            AStar_Node currentNode { get; set; }
            PriorityQueue<AStar_Node, int> nodesToVisit { get; } = new PriorityQueue<AStar_Node, int>();
            List<AStar_Node> visitedNodes { get; } = new List<AStar_Node>();

            public AStar(Func<Tile,bool, bool> tileChecker, IVect destination, IVect start)
            {
                this.tileChecker = tileChecker;
                this.destination = destination;
                this.start = start;               
            }

            public Stack<IVect>? GetPath(int maxDist)
            {
                bool found = false;
                nodesToVisit.Enqueue(new AStar_Node(start / 32, 0),0);
                while (nodesToVisit.Count > 0)
                {
                    AStar_Node currentNode = nodesToVisit.Dequeue();
                    int temp = currentNode.ComputeHeuristic(destination / 32);
                    if (currentNode.ComputeHeuristic(destination / 32) + currentNode.distanceHere > maxDist)
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
                            if (tileChecker(world.GetTile(checkedPos.x, checkedPos.y),false))
                            {
                                bool hasVisited = false;
                                foreach (AStar_Node node in visitedNodes)
                                {
                                    if (node.pos == checkedPos)
                                    {
                                        if (node.distanceHere > (currentNode.distanceHere + 1))
                                        {
                                            node.distanceHere = currentNode.distanceHere + 1;
                                            node.lastNode = currentNode;
                                        }
                                        hasVisited = true;
                                        break;
                                    }
                                }
                                
                                if (!hasVisited)
                                {
                                    AStar_Node newNode = new AStar_Node(checkedPos, currentNode.distanceHere + 1);
                                    newNode.lastNode = currentNode;
                                    nodesToVisit.Enqueue(newNode, newNode.distanceHere + newNode.ComputeHeuristic(destination / 32));
                                    visitedNodes.Add(newNode);
                                }

                                if (checkedPos == destination / 32)
                                {
                                    found = true;
                                    break;
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
                
                
                
                Stack<IVect> movements = new Stack<IVect>();
                AStar_Node checking = visitedNodes[visitedNodes.Count - 1];
                while (checking.lastNode != null)
                {
                    movements.Push(checking.pos);
                    checking = checking.lastNode;
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
            }
            public int ComputeHeuristic(IVect endPos)
            {
                return (endPos - pos).Mag();
            }
        }
    }
}
