﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPath
{


    class Program
    {

        class Node
        {

            private List<Node> neighbors;
            private int i, j;


            public List<Node> Neighbors
            {
                get { return neighbors; }
            }
            public int I
            {
                get { return i; }
            }
            public int J
            {
                get { return j; }
            }

            public void addNeighbor(Node g)
            {
                neighbors.Add(g);
            }

            public int getDegree()
            {
                return neighbors.Count();
            }

            public Node(int i, int j)
            {
                neighbors = new List<Node>();
                this.i = i;
                this.j = j;
            }

            public override string ToString()
            {
                return string.Format("({0}, {1})", i, j);
            }

        }

        class Graph
        {

            private List<Node> nodes;
            private KeyValuePair<int,int> start;
            private KeyValuePair<int, int> finish;

            public Node Start
            {
                get { return findByIndex(start.Key, start.Value); }
            }

            public Node Finish
            {
                get { return findByIndex(finish.Key, finish.Value); }
            }

            public Graph(List<List<char>> matrix)
            {
                nodes = new List<Node>();


                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (matrix[i][j] != '#')
                        {
                            Node node = new Node(i, j);
                            if (i != 0 && matrix[i - 1][j] != '#') node.addNeighbor(findByIndex(i - 1, j));
                            if (i != 9 && matrix[i + 1][j] != '#') node.addNeighbor(findByIndex(i + 1, j));
                            if (j != 0 && matrix[i][j - 1] != '#') node.addNeighbor(findByIndex(i, j - 1));
                            if (j != 9 && matrix[i][j + 1] != '#') node.addNeighbor(findByIndex(i, j + 1));

                            if (matrix[i][j] == 'A') start = new KeyValuePair<int, int>(i, j);
                            if (matrix[i][j] == 'B') finish = new KeyValuePair<int, int>(i, j);
                            nodes.Add(node);
                        }

                    }
                }


            }

            private Node findByIndex(int i, int j)
            {
                Node node = nodes.Find((n) => { return n.I == i && n.J == j; });
                if (node == null) return new Node(i, j);
                else return node;
            }

        }

        static class Dijkstra
        {
            static public void Apply(Matrix matrix)
            {
                Graph g = new Graph(matrix.GetMatrix);
                //Dictionary<Node, int> _check = new Dictionary<Node, int>();
                Dictionary<Node, int> distance = new Dictionary<Node, int>();
                List<Node> check = new List<Node>();

                //check.Add(g.Start);
                distance.Add(g.Start, 0);


                while (!distance.ContainsKey(g.Finish))
                {

                    Node node = distance.OrderBy(n => n.Value).Where((n) => { return !check.Contains(n.Key); }).FirstOrDefault().Key;
                    foreach (Node item in node.Neighbors.SkipWhile((n)=> { return check.Contains(n) && distance.ContainsKey(n); }))
                    {
                        if (distance.ContainsKey(item))
                        {
                            if (distance[item] > distance[node] + 1)
                            {
                                distance[item] = distance[node] + 1;
                            }

                        }
                        else
                        {
                            distance.Add(item, distance[node] + 1);
                        }


                        //distance.Add(item, 1);

                    }
                    check.Add(node);



                }







            }

        }

        class Matrix
        {

            private List<List<char>> matrix;

            public List<List<char>> GetMatrix
            {
                get { return matrix; }
            }

            public void print()
            {
                System.Console.Out.WriteLine(new String('-', 23));

                for (int i = 0; i < 10; i++)
                {
                    System.Console.Out.Write("| ");

                    for (int j = 0; j < 10; j++)
                    {
                        System.Console.Out.Write(matrix[i][j]);
                        System.Console.Out.Write(" ");

                    }

                    System.Console.Out.WriteLine("|");
                }
                System.Console.Out.WriteLine(new String('-', 23));


            }

            public Matrix()
            {
                matrix = new List<List<char>>();
                for (int i = 0; i < 10; i++)
                {
                    matrix.Add(new List<char>());
                    for (int j = 0; j < 10; j++)
                    {
                        matrix[i].Add(' ');
                    }
                }
            }

            public void setStart(int i, int j)
            {
                Console.Out.WriteLine("Setting start point at ({0},{1})", i, j);
                matrix[i][j] = 'A';
            }

            public void setObstacle(int i, int j)
            {
                matrix[i][j] = '#';
            }

            public void setPathTile(int i, int j)
            {
                matrix[i][j] = 'o';
            }

            public void setDestination(int i, int j)
            {
                Console.Out.WriteLine("Setting destination point at ({0},{1})", i, j);
                matrix[i][j] = 'B';
            }


        }

        static void Main(string[] args)
        {
            Matrix matrix = new Matrix();

            matrix.setStart(3, 2);
            matrix.setDestination(4, 5);
            matrix.setObstacle(3, 3);
            matrix.setObstacle(4, 4);
            matrix.setObstacle(5, 5);
            matrix.setObstacle(2, 2);

            Dijkstra.Apply(matrix);

            matrix.print();
            Console.In.Read();
        }
    }
}
