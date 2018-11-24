using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach2
{
    class B_Tree_Node<T> where T :  IComparable
    {

        public List<T> Keys { get; set; }
        public int NodeId;
        public List<B_Tree_Node<T>> Pointers
        { get; set; }

        public B_Tree_Node(T[] keys, B_Tree_Node<T>[] pointers, int nodeId)
        {
            NodeId = nodeId;
            Keys = new List<T>();
            if (keys != null)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Keys.Add(keys[i]);
                }
            }

            if (pointers != null)
            {
                this.Pointers = new List<B_Tree_Node<T>>();
                for (int i = 0; i < pointers.Length; i++)
                {
                    this.Pointers.Add(pointers[i]);
                }

            }
        }

        public B_Tree_Node(int nodeId)
        {
            NodeId = nodeId;
            Keys = new List<T>();
            this.Pointers = new List<B_Tree_Node<T>>();
        }

        public override string ToString()
        {
            string res = "";
            foreach (var k in Keys)
            {
                res += k + " ";
            }
            return res;
        }

    }

    class B_Tree<T> where T :  IComparable
    {
        private B_Tree_Node<T> root;
        public B_Tree_Node<T> Root
        { get { return root; } }
        private int t;//Степень B-дерева
        public int Branching
        {   get { return t; }
            set { }
        }

        private string TreeDirectory;
        private int nodeCount = 0;
        public B_Tree(int t, keyFromString fromStr)
        {
            this.t = t;
            KeyFromStr = fromStr;
            root = new B_Tree_Node<T>(nodeCount++);

        }
        public delegate T keyFromString(string str);
        public keyFromString KeyFromStr;


        
        

        public void ReadFromDir(string dir)
        {
            TreeDirectory = dir;
            FileStream fileStream = new FileStream(dir + "/"  + "root.txt", FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);

            int rootId = Convert.ToInt32(reader.ReadLine());
            int t = Convert.ToInt32(reader.ReadLine());
            reader.Close();
            this.t = t;
            this.root = new B_Tree_Node<T>(rootId);
            nodeCount++;
            ReadTreeFromFile_r(root);
        } 

        private void ReadTreeFromFile_r(B_Tree_Node<T> root)
        {
            ReadNodeFromFile(root);
            foreach(var child in root.Pointers)
            {
                ReadTreeFromFile_r(child);
            }
        }

        private void ReadNodeFromFile(B_Tree_Node<T> node)
        {
            FileStream fileStream = new FileStream(TreeDirectory + "/" + node.NodeId + ".txt", FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            var keys = reader.ReadLine().Split('\t');
            if(node.Keys == null)
            {
                node.Keys = new List<T>();
            }
            node.Keys.Clear();
            foreach(var key in keys)
            {
                node.Keys.Add(KeyFromStr(key));
            }
            
            var ids = reader.ReadLine();
            reader.Close();
            if(ids != null)
            {
                node.Pointers.Clear();
                var childIds = ids.Split('\t');
                foreach (var childId in childIds)
                {
                    node.Pointers.Add(new B_Tree_Node<T>(Convert.ToInt32(childId)));
                    nodeCount++;
                }
            }
            
        }
        public void Save(string dir)
        {
            TreeDirectory = dir;
            FileStream fileStream = new FileStream(TreeDirectory + "/root.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.WriteLine(root.NodeId);
            writer.WriteLine(t);
            writer.Flush();
            writer.Close();
            WriteTreeToFile_r(root);
        }
        private void WriteTreeToFile_r(B_Tree_Node<T> root)
        {
            
            WriteNodeToFile(root);

            foreach(var node in root.Pointers)
            {
                WriteTreeToFile_r(node);
            }
        }

        private void WriteNodeToFile(B_Tree_Node<T> node)
        {
            FileStream fileStream = new FileStream(TreeDirectory + "/" + node.NodeId + ".txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            for(int i = 0; i < node.Keys.Count; i++)
            {
                writer.Write(node.Keys[i].ToString());
                if(i < node.Keys.Count - 1)
                {
                    writer.Write("\t");
                }
                
            }
            writer.WriteLine();
            for(int i = 0; i < node.Pointers.Count; i++)
            {
                writer.Write(node.Pointers[i].NodeId);
                if (i < node.Pointers.Count - 1)
                {
                    writer.Write("\t");
                }
            }
            writer.Flush();
            writer.Close();
        }

        public void Remove(T key)
        {
            Remove_r(root, key);
        }

        private void Remove_r(B_Tree_Node<T> node, T key)
        {
            var index = -1;
            for (int i = 0; i < node.Keys.Count; i++)//Ищем ключ в текущем узле
            {
                if (key.CompareTo(node.Keys[i]) == 0)
                {
                    index = i;
                }
            }
            if (index >= 0)//Ключ в текущем узле
            {
                if (node.Pointers == null || node.Pointers.Count == 0)//Текущий узел - лист
                {
                    node.Keys.RemoveAt(index);
                }
                else
                {
                    var y = node.Pointers[index];
                    var z = node.Pointers[index + 1];
                    if (y.Keys.Count >= t)
                    {
                        var key1 = y.Keys[y.Keys.Count - 1];
                        Remove_r(y, key1);
                        node.Keys[index] = key1;
                    }
                    else if (z.Keys.Count >= t)
                    {
                        var key1 = z.Keys[0];
                        Remove_r(z, key1);
                        node.Keys[index] = key1;
                    }
                    else
                    {
                        y.Keys.Add(key);
                        foreach (var k in z.Keys)
                        {
                            y.Keys.Add(k);
                        }
                        foreach(var p in z.Pointers)
                        {
                            y.Pointers.Add(p);
                        }
                        node.Keys.RemoveAt(index);
                        node.Pointers.RemoveAt(index + 1);
                        if(node == root && node.Keys.Count == 0)
                        {
                            root = y;
                        }
                        Remove_r(y, key);
                    }
                }
            }
            else if (node.Pointers != null && node.Pointers.Count > 0)
            {
                var i = 0;
                for (i = 0; i < node.Keys.Count; i++)
                {
                    if (key.CompareTo(node.Keys[i]) < 0)
                    {
                        break;
                    }
                }
                if (node.Pointers[i].Keys.Count >= t)
                {
                    Remove_r(node.Pointers[i], key);
                }
                else
                {
                    //var y = node.Pointers[i - 1];
                    var x = node.Pointers[i];
                    if (i + 1 < node.Pointers.Count && node.Pointers[i + 1].Keys.Count >= t)
                    {
                        var z = node.Pointers[i + 1];
                        x.Keys.Add(node.Keys[i]);
                        if (x.Pointers.Count > 0)
                        {
                            x.Pointers.Add(z.Pointers[0]);
                            z.Pointers.RemoveAt(0);
                        }

                        node.Keys[i] = z.Keys[0];
                        z.Keys.RemoveAt(0);
                        Remove_r(x, key);
                    }
                    else if (i - 1 >= 0 && node.Pointers[i - 1].Keys.Count >= t)
                    {
                        var y = node.Pointers[i - 1];
                        x.Keys.Insert(0, node.Keys[i - 1]);
                        if (x.Pointers.Count > 0)
                        {
                            x.Pointers.Insert(0, y.Pointers[y.Pointers.Count - 1]);
                            y.Pointers.RemoveAt(y.Pointers.Count - 1);
                        }

                        node.Keys[i - 1] = y.Keys[y.Keys.Count - 1];
                        y.Keys.RemoveAt(y.Keys.Count - 1);
                        Remove_r(x, key);
                    }
                    else
                    {
                        var y = new B_Tree_Node<T>(nodeCount++);
                        if (i + 1 < node.Pointers.Count)
                        {
                            y = node.Pointers[i + 1];
                            x.Keys.Add(node.Keys[i]);
                            node.Keys.RemoveAt(i);
                            for (int j = 0; j < y.Keys.Count; j++)
                            {
                                x.Keys.Add(y.Keys[j]);
                            }
                            for (int j = 0; j < y.Pointers.Count; j++)
                            {
                                x.Pointers.Add(y.Pointers[j]);
                            }
                            node.Pointers.RemoveAt(i + 1);
                            Remove_r(x, key);
                        }
                        else if (i - 1 >= 0)
                        {
                            y = node.Pointers[i - 1];
                            x.Keys.Insert(0, node.Keys[i - 1]);
                            node.Keys.RemoveAt(i - 1);
                            for (int j = y.Keys.Count - 1; j >= 0; j--)
                            {
                                x.Keys.Insert(0, y.Keys[j]);
                            }
                            for (int j = y.Pointers.Count - 1; j >= 0; j--)
                            {
                                x.Pointers.Insert(0, y.Pointers[j]);
                            }
                            node.Pointers.RemoveAt(i - 1);
                            if (node.Keys.Count == 0 && node == root)
                            {
                                root = x;
                            }
                            Remove_r(x, key);
                        }

                    }

                }

            }

        }

        private void Split_Child(B_Tree_Node<T> node, int i)
        {
            var z = new B_Tree_Node<T>(nodeCount++);
            var y = node.Pointers[i];
            int mid = t - 1;
            for (int j = mid + 1; j < y.Keys.Count; j++)
            {
                z.Keys.Add(y.Keys[j]);
            }
            for (int j = y.Keys.Count - 1; j > mid; j--)
            {
                y.Keys.RemoveAt(j);
            }
            //if (y.Pointers != null)
            //{
                for (int j = mid + 1; j < y.Pointers.Count; j++)
                {
                    z.Pointers.Add(y.Pointers[j]);
                }
                for (int j = y.Pointers.Count - 1; j > mid; j--)
                {
                    y.Pointers.RemoveAt(j);
                }
            //}

            node.Keys.Insert(i, y.Keys[mid]);
            y.Keys.RemoveAt(mid);
            node.Pointers.Insert(i + 1, z);

        }

        private void InsertNonfull(B_Tree_Node<T> node, T key)
        {
            var i = 0;
            for (i = 0; i < node.Keys.Count; i++)
            {
                if (key.CompareTo(node.Keys[i]) == 0)
                {
                    return;
                }
                if (key.CompareTo(node.Keys[i]) < 0)
                {
                    break;
                }
            }
            if (node.Pointers == null || node.Pointers.Count == 0)
            {
                node.Keys.Insert(i, key);
            }
            else
            {
                if (node.Pointers[i].Keys.Count == 2 * t - 1)
                {
                    Split_Child(node, i);
                    if (key.CompareTo(node.Keys[i]) > 0)
                    {
                        i++;
                    }
                }
                InsertNonfull(node.Pointers[i], key);

            }
        }

        public void Insert(T key)
        {
            if (root.Keys.Count == 2 * t - 1)
            {
                var s = new B_Tree_Node<T>(null, new B_Tree_Node<T>[] { root }, nodeCount++);
                root = s;
                Split_Child(s, 0);
                InsertNonfull(s, key);
            }
            else
            {
                InsertNonfull(root, key);
            }
        }

        public class FoundElement<F> where F : IComparable
        {
            public B_Tree_Node<F> node;
            public int index;
        }

        public FoundElement<T> Search(T key)
        {
            return Search_r(key, root);
        }

        private FoundElement<T> Search_r(T key, B_Tree_Node<T> root)
        {
            FoundElement<T> foundElement = new FoundElement<T>();

            int i = 0;
            while (i < root.Keys.Count && root.Keys[i].CompareTo(key) < 0)
            {
                i++;
            }
            if (i < root.Keys.Count && root.Keys[i].CompareTo(key) == 0)
            {
                return new FoundElement<T> { index = i, node = root };
            }

            if (root.Pointers != null && root.Pointers.Count > i)
            {
                return Search_r(key, root.Pointers[i]);
            }

            return null;
        }





        public override string ToString()
        {
            string result = "";

            result += ToString(root);

            return result;
        }

        public List<T> ToList()
        {
            List<T> list = new List<T>();
            ToList_r(Root, list);
            return list;
        }

        private void ToList_r(B_Tree_Node<T> root, List<T> list)
        {
            if(root.Pointers.Count != 0)
            {
                foreach (var node in root.Pointers)
                {
                  
                    ToList_r(node, list);
                    //list.AddRange(list);
                }
            }
            list.AddRange(root.Keys);


        }

        static private string ToString(B_Tree_Node<T> root, string prefix = "")
        {
            string result = "";
            if (root == null || root.Keys.Count == 0)
            {
                return "";
            }

            result += prefix + "|--";
            prefix += "|   ";
            result += "[";
            var i = 0;
            foreach (var k in root.Keys)
            {
                result += k.ToString();
                if (i != root.Keys.Count - 1)
                {
                    result += ",";
                }
                i++;
            }
            result += "]";
            result += "\n";
            if (root.Pointers != null)
            {
                foreach (var node in root.Pointers)
                {
                    result += ToString(node, prefix);
                }
            }

            return result;
        }
    }


    class ComparableInt : IComparable
    {
        public int Value;
        public override string ToString()
        {
            return Value.ToString();
        }
        public ComparableInt(int value)
        {
            Value = value;
        }
        public static B_Tree<ComparableInt>.keyFromString FromStr = str => { return new ComparableInt(Convert.ToInt32(str)); };
        public int CompareTo(object obj)
        {
            if (obj is ComparableInt)
            {
                ComparableInt n = obj as ComparableInt;
                return Value.CompareTo(n.Value);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
