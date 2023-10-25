using System.Collections.Generic;

namespace Game.NovelVisualization.Editor
{
    public class DSNodeErrorData
    {
        public DSErrorData ErrorData { get; set; }
        public List<CustomNode> Nodes { get; set; }

        public DSNodeErrorData()
        {
            ErrorData = new DSErrorData();
            Nodes = new List<CustomNode>();
        }
    }
}