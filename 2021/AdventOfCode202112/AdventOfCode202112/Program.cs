using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode202112
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            try
            {
                input = File.ReadAllLines("../../../input.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }
            Node startNode = null;

            List<Node> nodes = new List<Node>();
            foreach (string line in input)
            {
                string[] temp = line.Split('-');
                Node[] tempNode = new Node[2];

                for (int i = 0; i < temp.Length; i++)
                {
                    bool exists = false;
                    foreach (Node n in nodes)
                    {
                        if (n.Name == temp[i])
                        {
                            exists = true;
                            tempNode[i] = n;
                            break;
                        }
                    }
                    if (exists == false)
                    {
                        nodes.Add(new Node(temp[i]));
                        // Zapamiętaj początek labiryntu
                        if ((temp[i] == "start") && (startNode == null)) startNode = nodes[^1];
                        tempNode[i] = nodes[^1];
                    }
                }

                if (tempNode[0].ConnectedTo.Contains(tempNode[1]) == false) tempNode[0].ConnectedTo.Add(tempNode[1]);
                if (tempNode[1].ConnectedTo.Contains(tempNode[0]) == false) tempNode[1].ConnectedTo.Add(tempNode[0]);
            }

            // Part one
            List<string> finishedPaths = new List<string>();
            List<Path> activePaths = new List<Path>();
            foreach (Node n in startNode.ConnectedTo)
            {
                activePaths.Add(new Path());
                activePaths[^1].ActualNode = n;
                activePaths[^1].VisitedNodes.Add(startNode.Name);
                activePaths[^1].VisitedNodes.Add(n.Name);
                activePaths[^1].PathTaken += startNode.Name + ',' + n.Name + ',';
            }

            while (activePaths.Count > 0)
            {
                List<Path> tempPaths = activePaths;
                activePaths = new List<Path>();

                foreach (Path path in tempPaths)
                {
                    foreach (Node connected in path.ActualNode.ConnectedTo)
                    {
                        if (connected.Name == "end")
                        {
                            finishedPaths.Add(path.PathTaken + "end");
                        }
                        else if ((connected.IsLowerCase == true && (path.VisitedNodes.Contains(connected.Name) == false))
                              || (connected.IsLowerCase == false))
                        {
                            activePaths.Add(new Path());
                            activePaths[^1].ActualNode = connected;
                            activePaths[^1].VisitedNodes = new List<string>();
                            activePaths[^1].VisitedNodes.AddRange(path.VisitedNodes);
                            activePaths[^1].VisitedNodes.Add(connected.Name);
                            activePaths[^1].PathTaken = path.PathTaken + connected.Name + ',';
                        }
                    }
                }
            }
            Console.WriteLine("Part one answer -> Through this cave system there are " + finishedPaths.Count + " paths.");

            // Part two
            finishedPaths = new List<string>();
            activePaths = new List<Path>();
            foreach (Node n in startNode.ConnectedTo)
            {
                activePaths.Add(new Path());
                activePaths[^1].ActualNode = n;
                activePaths[^1].VisitedNodes.Add(startNode.Name);
                activePaths[^1].VisitedNodes.Add(n.Name);
                activePaths[^1].PathTaken += startNode.Name + ',' + n.Name + ',';
            }

            while (activePaths.Count > 0)
            {
                List<Path> tempPaths = activePaths;
                activePaths = new List<Path>();

                foreach (Path path in tempPaths)
                {
                    foreach (Node connected in path.ActualNode.ConnectedTo)
                    {
                        if (connected.Name == "end")
                        {
                            finishedPaths.Add(path.PathTaken + "end");
                        }
                        else if (PartTwoCheck(path.VisitedNodes, connected.Name) || (connected.IsLowerCase == false))
                        {
                            activePaths.Add(new Path());
                            activePaths[^1].ActualNode = connected;
                            activePaths[^1].VisitedNodes = new List<string>();
                            activePaths[^1].VisitedNodes.AddRange(path.VisitedNodes);
                            activePaths[^1].VisitedNodes.Add(connected.Name);
                            activePaths[^1].PathTaken = path.PathTaken + connected.Name + ',';
                        }
                    }
                }
            }
            Console.WriteLine("Part two answer -> Through this cave system there are " + finishedPaths.Count + " paths.");

            Console.ReadLine();
        }

        static bool PartTwoCheck(List<string> visited, string node)
        {
            if (node == "start") return false;

            List<string> lowerCaseNodes = new List<string>();
            bool twoLowerCaseVisited = false;
            int nubmerOfLowerCaseVisited = 0;

            foreach (string visitedNode in visited)
            {
                if (visitedNode == visitedNode.ToLower())
                {
                    lowerCaseNodes.Add(visitedNode);

                    nubmerOfLowerCaseVisited = lowerCaseNodes.FindAll(x => x.Equals(visitedNode)).Count;
                    if (nubmerOfLowerCaseVisited == 2)
                    {
                        twoLowerCaseVisited = true;
                    }

                    if ((nubmerOfLowerCaseVisited == 2) && (visitedNode == node)) return false;
                    else if ((twoLowerCaseVisited == true) && (lowerCaseNodes.FindAll(x => x.Equals(node)).Count == 1)) return false;
                }
            }

            return true;
        }

        public class Node
        {
            public Node(string name)
            {
                Name = name;
                IsLowerCase = Name == name.ToLower();
                ConnectedTo = new List<Node>();
            }

            public string Name;
            public bool IsLowerCase = false;
            public List<Node> ConnectedTo;
        }

        public class Path
        {
            public List<string> VisitedNodes = new List<string>();
            public Node ActualNode;
            public string PathTaken;
        }
    }
}
