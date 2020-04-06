using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AdxLib
{
    public class JsonTree
    {
        private readonly JToken _root;

        public JsonTree(string json)
        {
            _root = JToken.Parse(json);    
        }

        public IEnumerable<JsonTreeNode> GetLeafNodes()
        {
            var leafNodes = new List<JsonTreeNode>();
            Traverse(_root, leafNodes);
            return leafNodes;
        }

        private void Traverse(JToken jt, List<JsonTreeNode> leafNodes)
        {
            if (jt is JValue jv)
            {
                leafNodes.Add(new JsonTreeNode(jv.Path, jv.Type));
            }

            foreach (var child in jt.Children())
            {
                Traverse(child, leafNodes);
            }
        }
    }

    public class JsonTreeNode
    {
        public string Path { get; }
        public JTokenType Type { get; }

        public JsonTreeNode(string path, JTokenType type)
        {
            Path = path;
            Type = type;
        }
    }
}
