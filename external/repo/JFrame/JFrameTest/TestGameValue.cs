//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NSubstitute;
using NUnit.Framework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace JFrameTest
{

    //public class TestGameValue
    //{
    //    #region int
    //    [Test]
    //    public void TestIntPlusInt()
    //    {
    //        //arrange
    //        var v1 = new IntAttribute("atk", 1);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        var v3 = v1 + v2;

    //        //expect
    //        Assert.AreEqual(3, v3.Value);
    //    }

    //    [Test]
    //    public void TestIntPlusIntOverflowException()
    //    {
    //        //arrange
    //        Exception exception = null;
    //        var v1 = new IntAttribute("atk", int.MaxValue);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        try
    //        {
    //            var v3 = v1 + v2;
    //        }
    //        catch(Exception e) {
    //            exception = e;
    //        }

    //        //expect
    //        Assert.AreEqual(typeof(OverflowException), exception.GetType());
    //    }


    //    [Test]
    //    public void TestIntMinusInt()
    //    {
    //        //arrange
    //        var v1 = new IntAttribute("atk", 1);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        var v3 = v1 - v2;

    //        //expect
    //        Assert.AreEqual(-1, v3.Value);
    //    }

    //    [Test]
    //    public void TestIntMinusIntOverflowException()
    //    {
    //        //arrange
    //        Exception exception = null;
    //        var v1 = new IntAttribute("atk", int.MinValue);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        try
    //        {
    //            var v3 = v1 - v2;
    //        }
    //        catch (Exception e)
    //        {
    //            exception = e;
    //        }

    //        //expect
    //        Assert.AreEqual(typeof(OverflowException), exception.GetType());
    //    }

    //    [Test]
    //    public void TestMultPlusInt()
    //    {
    //        //arrange
    //        var v1 = new IntAttribute("atk", 1);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        var v3 = v1 * v2;

    //        //expect
    //        Assert.AreEqual(2, v3.Value);
    //    }

    //    [Test]
    //    public void TestIntMultIntOverflowException()
    //    {
    //        //arrange
    //        Exception exception = null;
    //        var v1 = new IntAttribute("atk", int.MaxValue);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        try
    //        {
    //            var v3 = v1 * v2;
    //        }
    //        catch (Exception e)
    //        {
    //            exception = e;
    //        }

    //        //expect
    //        Assert.AreEqual(typeof(OverflowException), exception.GetType());
    //    }


    //    [Test]
    //    public void TestDivPlusInt()
    //    {
    //        //arrange
    //        var v1 = new IntAttribute("atk", 1);
    //        var v2 = new IntAttribute("atk", 2);

    //        //action
    //        var v3 = v1 / v2;

    //        //expect
    //        Assert.AreEqual(0, v3.Value);
    //    }

    //    [Test]
    //    public void TestIntDivIntOverflowException()
    //    {
    //        //arrange
    //        Exception exception = null;
    //        var v1 = new IntAttribute("atk", int.MinValue);
    //        var v2 = new IntAttribute("atk", 0);

    //        //action
    //        try
    //        {
    //            var v3 = v1 / v2;
    //        }
    //        catch (Exception e)
    //        {
    //            exception = e;
    //        }

    //        //expect
    //        Assert.AreEqual(typeof(DivideByZeroException), exception.GetType());
    //    }

    //    [Test]
    //    public void TestIntConvertToFloat()
    //    {
    //        //arrange
    //        var v1 = new IntAttribute("atk", 1);

    //        //action
    //        var result =  v1.ToFloatValue();

    //        //expect
    //        Assert.AreEqual(true, result.Value is float);
    //        Assert.AreEqual(1.0f, result.Value);
    //    }

    //    //[Test]
    //    //public void TestIntSource()
    //    //{
    //    //    //arrange
    //    //    var v1 = new GameIntAttribute("atk", 1, new GameSource("equip"));
    //    //    var v2 = new GameIntAttribute("atk", 2, new GameSource("food"));

    //    //    //action
    //    //    var result = v1+v2;

    //    //    //expect
    //    //    Assert.AreEqual("equip", result.AttributeSource.Name);
    //    //}
    //    #endregion int


    //    #region float
    //    [Test]
    //    public void TestFloatPlusFloat()
    //    {
    //        //arrange
    //        var v1 = new FloatAttribute("atk", 1);
    //        var v2 = new FloatAttribute("atk", 2);

    //        //action
    //        var v3 = v1 + v2;

    //        //expect
    //        Assert.AreEqual(3, v3.Value);
    //    }



    //    [Test]
    //    public void TestFloatMinusInt()
    //    {
    //        //arrange
    //        var v1 = new FloatAttribute("atk", 1);
    //        var v2 = new FloatAttribute("atk", 2);

    //        //action
    //        var v3 = v1 - v2;

    //        //expect
    //        Assert.AreEqual(-1, v3.Value);
    //    }



    //    [Test]
    //    public void TestFloatMultFloat()
    //    {
    //        //arrange
    //        var v1 = new FloatAttribute("atk", 1);
    //        var v2 = new FloatAttribute("atk", 2);

    //        //action
    //        var v3 = v1 * v2;

    //        //expect
    //        Assert.AreEqual(2, v3.Value);
    //    }




    //    [Test]
    //    public void TestFloatDivFloat()
    //    {
    //        //arrange
    //        var v1 = new FloatAttribute("atk", 1);
    //        var v2 = new FloatAttribute("atk", 2);

    //        //action
    //        var v3 = v1 / v2;

    //        //expect
    //        Assert.AreEqual(0.5f, v3.Value);
    //    }



    //    [Test]
    //    public void TestFloatConvertToInt()
    //    {
    //        //arrange
    //        var v1 = new FloatAttribute("atk", 1);

    //        //action
    //        var result = v1.ToIntValue();

    //        //expect
    //        Assert.AreEqual(true, result.Value is int);
    //        Assert.AreEqual(1, result.Value);
    //    }
    //    #endregion int


    //}


}
