using Mono.Rocks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace mdoc.Test
{
    [TestFixture]
    public class ObjectRocksTests
    {
        private class TreeNode
        {
            public int Value { get; set; }
            public List<TreeNode> Children { get; set; } = new List<TreeNode>();
        }

        [Test]
        public void TraverseDepthFirst_ShouldReturnValuesInDepthFirstOrder()
        {
            // Arrange
            var root = new TreeNode { Value = 1 };
            var child1 = new TreeNode { Value = 2 };
            var child2 = new TreeNode { Value = 3 };
            var grandChild1 = new TreeNode { Value = 4 };
            var grandChild2 = new TreeNode { Value = 5 };

            root.Children.Add(child1);
            root.Children.Add(child2);
            child1.Children.Add(grandChild1);
            child1.Children.Add(grandChild2);

            // Act
            var result = root.TraverseDepthFirst(node => node.Value, node => node.Children).ToList();

            // Assert
            var expected = new List<int> { 1, 2, 4, 5, 3 };
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TraverseDepthFirstWithParent_ShouldReturnValuesWithParentsInDepthFirstOrder()
        {
            // Arrange
            var root = new TreeNode { Value = 1 };
            var child1 = new TreeNode { Value = 2 };
            var child2 = new TreeNode { Value = 3 };
            var grandChild1 = new TreeNode { Value = 4 };
            var grandChild2 = new TreeNode { Value = 5 };

            root.Children.Add(child1);
            root.Children.Add(child2);
            child1.Children.Add(grandChild1);
            child1.Children.Add(grandChild2);

            // Act
            var result = root.TraverseDepthFirstWithParent(node => node.Value, node => node.Children).ToList();

            // Assert
            var expected = new List<KeyValuePair<TreeNode, int>>
            {
                new KeyValuePair<TreeNode, int>(null, 1),
                new KeyValuePair<TreeNode, int>(child1, 2),
                new KeyValuePair<TreeNode, int>(grandChild1, 4),
                new KeyValuePair<TreeNode, int>(grandChild2, 5),
                new KeyValuePair<TreeNode, int>(child2, 3)
            };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
