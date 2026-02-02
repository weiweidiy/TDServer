using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace JFramework.Tests
{
    [TestFixture]
    public class JLanguageManagerTests
    {
        private JLanguageManager _manager;
        private List<ILanguage> _languages;
        private ILanguage _english;
        private ILanguage _chinese;
        private const string EnglishKey = "en";
        private const string ChineseKey = "zh";

        [SetUp]
        public void SetUp()
        {
            // Create mock languages
            _english = Substitute.For<ILanguage>();
            _english.GetText(Arg.Any<string>()).Returns(x => $"English_{x.Arg<string>()}");

            _chinese = Substitute.For<ILanguage>();
            _chinese.GetText(Arg.Any<string>()).Returns(x => $"Chinese_{x.Arg<string>()}");

            _languages = new List<ILanguage> { _english, _chinese };

            // Create manager with key selector that returns "en" for English, "zh" for Chinese
            _manager = new JLanguageManager(_languages, lang =>
                lang == _english ? EnglishKey :
                lang == _chinese ? ChineseKey :
                throw new ArgumentException("Unknown language"));
        }

        [Test]
        public void Constructor_InitializesWithLanguages()
        {
            // Assert
            //Assert.AreEqual(2, _manager.Count);
            Assert.IsNotNull(_manager.GetLanguage(EnglishKey));
            Assert.IsNotNull(_manager.GetLanguage(ChineseKey));
        }

        [Test]
        public void GetCurLanguage_InitiallyNull()
        {
            // Act & Assert
            Assert.IsNull(_manager.GetCurLanguage());
        }

        [Test]
        public void SetCurLanguage_SetsCurrentLanguage()
        {
            // Act
            _manager.SetCurLanguage(_english);

            // Assert
            Assert.AreEqual(_english, _manager.GetCurLanguage());
        }

        [Test]
        public void SetCurLanguage_SameLanguage_DoesNotInvokeEvent()
        {
            // Arrange
            var eventFired = false;

            _manager.SetCurLanguage(_english);

            _manager.onLanguageChanged += _ => eventFired = true;

            //_manager.SetCurLanguage(_english); // First set

            // Act
            _manager.SetCurLanguage(_english); // Set same language again

            // Assert
            Assert.IsFalse(eventFired);
        }

        [Test]
        public void SetCurLanguage_DifferentLanguage_InvokesEvent()
        {
            // Arrange
            ILanguage eventLanguage = null;
            _manager.onLanguageChanged += lang => eventLanguage = lang;

            // Act
            _manager.SetCurLanguage(_english);

            // Assert
            Assert.AreEqual(_english, eventLanguage);
        }

        [Test]
        public void GetLanguage_ReturnsCorrectLanguage()
        {
            // Act & Assert
            Assert.AreEqual(_english, _manager.GetLanguage(EnglishKey));
            Assert.AreEqual(_chinese, _manager.GetLanguage(ChineseKey));
        }

        [Test]
        public void GetLanguage_UnknownKey_ReturnsNull()
        {
            // Act & Assert
            Assert.IsNull(_manager.GetLanguage("unknown"));
            //var ex = Assert.Throws<KeyNotFoundException>(() => _manager.GetLanguage("unknown"));
        }

        [Test]
        public void GetText_NoCurrentLanguage_ThrowsException()
        {
            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _manager.GetText("test"));
            Assert.AreEqual("没有初始化当前语言对象 ", ex.Message);
        }

        [Test]
        public void GetText_WithCurrentLanguage_ReturnsText()
        {
            // Arrange
            _manager.SetCurLanguage(_english);

            // Act
            var result = _manager.GetText("hello");

            // Assert
            Assert.AreEqual("English_hello", result);
        }

        [Test]
        public void GetText_UnknownUid_ReturnsFallback()
        {
            // Arrange
            _manager.SetCurLanguage(_english);
            _english.GetText("unknown").Returns((string)null);

            // Act
            var result = _manager.GetText("unknown");

            // Assert
            Assert.AreEqual("#unknown", result);
        }
    }
}