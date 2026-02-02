using NUnit.Framework;
using JFramework.Configuration;
using System.Text;
using System;
using JFramework.Common;
using System.Collections.Generic;
using JFramework;

namespace JFrameTest
{
    public class Name
    {
        public string name;
    }
    public class TestConfigurationManager
    {

        //        string AppConfigContent = "< App > " + "\n" +

        //    < Config > 123 </ Config >
        //</ App >"
        string AppConfigContent = "";
        [SetUp]
        public void Setup()
        {
            var sb = new StringBuilder();
            sb.Append("<App>"); sb.Append("\n");
            sb.Append("<Config>"); sb.Append(123); sb.Append("</Config>"); sb.Append("\n");
            sb.Append("<ConfigArr>"); sb.Append("\n");
            sb.Append("<Config>"); sb.Append("a1"); sb.Append("</Config>");sb.Append("\n");
            sb.Append("<Config>"); sb.Append("a2"); sb.Append("</Config>"); sb.Append("\n");
            sb.Append("</ConfigArr>"); sb.Append("\n");
            sb.Append("</App>");
            AppConfigContent = sb.ToString();

            //Console.WriteLine(AppConfigContent);
        }


        /// <summary>
        /// 测试注册配置文件
        /// </summary>
        [Test]
        public void TestRegisterConfig()
        {
            //Arrange
            var manager = new ConfigurationManager();

            //Act
            manager.RegistConfiguration("App", "D:/App.txt");

            //Assert
            Assert.AreEqual(1, manager.GetRegistCount());
        }

        /// <summary>
        /// 测试从字符串加载配置
        /// </summary>
        [Test]
        public void TestLoadConfigFromString()
        {
            //Arrange
            var manager = new ConfigurationManager();

            //Act
            manager.Load("App", AppConfigContent,"");

            //Assert
            Assert.AreEqual("a1", manager["App"]["ConfigArr"][0].GetValue());
 
        }

        ///// <summary>
        ///// 从文件中加载
        ///// </summary>
        //[Test]
        //public void TestLoadConfigFromFile()
        //{
        //    //Arrange
        //    var manager = new ConfigurationManager();

        //    //Act
        //    manager.Load("App", "D:/App.json", new LocalReader(), new JsonNetParaser());

        //    //Assert
        //    Assert.AreEqual(1, manager["App"]["MyData"]["1"].GetValue<int>("ID"));
        //    Assert.AreEqual("1", manager["App"]["MyData"]["2"][0].GetValue<string>("name"));
        //    //var names = manager["App"]["MyData"]["2"].ToObject<List<Name>>();
        //}


    }
}