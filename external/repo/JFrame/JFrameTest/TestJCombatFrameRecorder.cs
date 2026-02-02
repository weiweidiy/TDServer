using NUnit.Framework;
using JFramework.Game;

namespace JFramework.Game.Tests
{
    [TestFixture]
    public class JCombatFrameRecorderTests
    {
        private const int TestMaxFrame = 100;
        private JCombatTurnBasedFrameRecorder _recorder;

        [SetUp]
        public void Setup()
        {
            _recorder = new JCombatTurnBasedFrameRecorder(TestMaxFrame);
        }

        [Test]
        public void Constructor_SetsMaxFrameCorrectly()
        {
            // Arrange
            const int expectedMaxFrame = 50;

            // Act
            var recorder = new JCombatTurnBasedFrameRecorder(expectedMaxFrame);

            // Assert
            Assert.AreEqual(expectedMaxFrame, recorder.GetMaxFrame());
            Assert.AreEqual(0, recorder.GetCurFrame());
        }

        [Test]
        public void GetCurFrame_Initially_ReturnsZero()
        {
            // Act & Assert
            Assert.AreEqual(0, _recorder.GetCurFrame());
        }

        [Test]
        public void GetMaxFrame_ReturnsConstructorValue()
        {
            // Act & Assert
            Assert.AreEqual(TestMaxFrame, _recorder.GetMaxFrame());
        }

        [Test]
        public void IsMaxFrame_WhenBelowMax_ReturnsFalse()
        {
            // Arrange
            _recorder.NextFrame(); // Advance to frame 1

            // Act & Assert
            Assert.IsFalse(_recorder.IsMaxFrame());
        }

        [Test]
        public void IsMaxFrame_WhenAtMax_ReturnsTrue()
        {
            // Arrange
            for (int i = 0; i < TestMaxFrame; i++)
            {
                _recorder.NextFrame();
            }

            // Act & Assert
            Assert.IsTrue(_recorder.IsMaxFrame());
        }

        [Test]
        public void NextFrame_IncrementsCurrentFrame()
        {
            // Act
            var frame1 = _recorder.NextFrame();
            var frame2 = _recorder.NextFrame();

            // Assert
            Assert.AreEqual(1, frame1);
            Assert.AreEqual(2, frame2);
            Assert.AreEqual(2, _recorder.GetCurFrame());
        }

        [Test]
        public void ResetFrame_SetsCurrentFrameToZero()
        {
            // Arrange
            _recorder.NextFrame();
            _recorder.NextFrame();

            // Act
            _recorder.ResetFrame();

            // Assert
            Assert.AreEqual(0, _recorder.GetCurFrame());
        }
    }
}