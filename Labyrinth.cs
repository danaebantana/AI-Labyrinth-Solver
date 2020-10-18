using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_AI_EE_2020
{
    public partial class Labyrinth : Form
    {
        
        private struct node   //Structure that describes the cells of the labyrinth.
        {
            public int X;
            public int Y;
            public int cost;
            public string path;
        }
        private node create_node(int x, int y, int c, String p)   //Create node.
        {
            node n = new node();
            n.X = x;
            n.Y = y;
            n.cost = c;
            n.path = p + "" + n.X.ToString() + "," + n.Y.ToString();
            return n;
        }

        int[,] labyrinth = new int[,] {{0,0,0,0,0},{0,1,1,1,0},{0,1,0,1,0},{0,1,0,1,0},{0,1,1,1,1},{0,1,0,0,0},}; //2D array
        Queue<node> frontSearchQueue = new Queue<node>();   //search front.
        List<node> closedSetList = new List<node>();   //closed set, that contain all the states that have been visited.
        int upperBound = int.MaxValue;  //upperbound.
        node final_node = new node();

        public Labyrinth()   //Constructor
        {
            InitializeComponent();    
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            label_shortestPath.Text = "-";
            label_cost.Text = "-";
            labelI.BackColor = Color.White;
            labelG.BackColor = Color.White;
            frontSearchQueue.Clear();
            upperBound = int.MaxValue;
            foreach (node n in closedSetList)
            {
                string name = "pictureBox" + n.X + n.Y;
                foreach (Control p in panel1.Controls)  
                {
                    if (p is PictureBox)
                    {
                        if (p.Name == name)
                        {
                            p.BackColor = Color.White;
                        }
                    }
                }
            }          
            closedSetList.Clear();         
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            node starting_node = create_node(5, 1, 0, "");  //Initial node.
            final_node = create_node(4, 4, 0, "");   //Final node.
            frontSearchQueue.Enqueue(starting_node);    //Insert initial node to the front search queue.      

            branch_and_bound();  //Branch&Bound Algorithm

            label_shortestPath.Text = final_node.path;
            label_cost.Text = final_node.cost.ToString();
            colour_paths();
        }
       
        private void branch_and_bound()
        {
            node current_node = new node();
            //shortest path finding process.  
            while (frontSearchQueue.Count != 0)  //Check if frontSearchQueue is empty, which means that there are not anymore elements/states to visit and examine.
            {
                current_node = frontSearchQueue.Dequeue();
                checkIfFinalStateNode(current_node, final_node);
                //Check if the current_node cost is less than the upperbound and if the node has already been extended. If not, I extend it.
                if (current_node.cost < upperBound & (closedSetList.Exists((node k) => k.X == current_node.X & k.Y == current_node.Y) == false))
                {
                    breadth_search_first_BFS(current_node);
                }          
                closedSetList.Add(current_node);
            }
        }

        private void checkIfFinalStateNode(node n, node fn)  //Check if the current_node, i.e. the one under examination is the final node.
        {
            if (n.X == fn.X & n.Y == fn.Y)  
            {
                if(n.cost < upperBound) {
                    final_node = n;
                    upperBound = final_node.cost; //Renew the cost / upperbound.
                }         
            }
        }

        //Examine node and find its children. The children can be right, left, up and down of the node and only the white cells (1) can be added as its children.
        private void breadth_search_first_BFS(node n)  
        {
            if (n.X >= 0 & n.X < 6)   //Search if there is a left and right child
            {
                if (n.Y > 0)   //Has left child
                {
                    if (labyrinth[n.X, n.Y - 1] == 1)
                    {
                        node left_child_node = create_node(n.X, n.Y - 1, n.cost + 1, n.path + " ");
                        frontSearchQueue.Enqueue(left_child_node);
                    }
                }
                if (n.Y < 4)   //Has right child
                {
                    if (labyrinth[n.X, n.Y + 1] == 1)
                    {
                        node right_child_node = create_node(n.X, n.Y + 1, n.cost + 1, n.path + " ");
                        frontSearchQueue.Enqueue(right_child_node);
                    }
                }
            }
            if (n.Y >= 0 & n.Y < 5)   //Search if there is up and down child.
            {
                if (n.X > 0)   //Has up child.
                {
                    if (labyrinth[n.X - 1, n.Y] == 1)
                    {
                        node top_child_node = create_node(n.X - 1, n.Y, n.cost + 1, n.path + " ");
                        frontSearchQueue.Enqueue(top_child_node);
                    }
                }
                if (n.X < 5)   //Has down child.
                {
                    if (labyrinth[n.X + 1, n.Y] == 1)
                    {
                        node down_child_node = create_node(n.X + 1, n.Y, n.cost + 1, n.path + " ");
                        frontSearchQueue.Enqueue(down_child_node);
                    }
                }
            }
        }

        public void colour_paths()
        {
            foreach (node n in closedSetList)   //Color the nodes that have been extended and saved in the close set list.
            {
                string name = "pictureBox" + n.X + n.Y;
                foreach (Control p in panel1.Controls)
                {
                    if (p is PictureBox)
                    {
                        if (p.Name == name)
                        {
                            p.BackColor = Color.DarkSeaGreen;
                        }
                    }
                }
            }

            string path = final_node.path;   //Color the nodes that show the shortest path from I to G.
            string[] path_split1 = path.Split(' ');
            foreach (string s in path_split1)
            {
                string[] path_split2 = s.Split(',');
                string name = "pictureBox" + path_split2[0] + path_split2[1];
                foreach (Control p in panel1.Controls)
                {
                    if (p is PictureBox)
                    {
                        if (p.Name == name)
                        {
                            p.BackColor = Color.LightSeaGreen;
                        }
                    }
                }
            }
            labelI.BackColor = Color.LightSeaGreen;
            labelG.BackColor = Color.LightSeaGreen;
        }

    }
}
