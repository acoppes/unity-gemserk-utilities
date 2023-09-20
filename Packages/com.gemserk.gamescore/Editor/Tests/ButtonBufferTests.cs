using Game.Components;
using NUnit.Framework;

namespace Game.Editor.Tests
{
    public class ButtonBufferTests
    {
        [Test]
        public void Test_HasBufferedAction()
        {
            // var button = new InputAction("name");

            var bufferedInput = BufferedInputComponent.Default();
        
            Assert.IsFalse(bufferedInput.HasBufferedActions("right"));
            bufferedInput.buffer.Add("right");

            Assert.IsTrue(bufferedInput.HasBufferedActions("right"));
        }
    
        [Test]
        public void Test_HasBufferedActions_List()
        {
            var bufferedInput = BufferedInputComponent.Default();
            Assert.IsFalse(bufferedInput.HasBufferedActions("a", "b"));
        
            bufferedInput.buffer.Add("a");
            Assert.IsFalse(bufferedInput.HasBufferedActions("a", "b"));

            bufferedInput.buffer.Add("b");
            Assert.IsTrue(bufferedInput.HasBufferedActions("a", "b"));
        
            bufferedInput.buffer.Add("c");
            Assert.IsFalse(bufferedInput.HasBufferedActions("a", "b"));
        }
    }
}
