using System;
using NUnit.Framework;
using RadFramework.Libraries.DictionaryMapper;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            IMapper mapper = new Mapper();

            TestType obj = new TestType();
            obj.PropString = "val";
            obj.ChildObject = new TestType2
            {
                PropString = "val"
            };
            obj.Cloneable = new TestType3()
            {
                PropString = "val"
            };

            TestType cloned = (TestType)mapper.DeepClone(typeof(TestType), obj);
            
            Assert.IsFalse(obj == cloned);
            Assert.IsFalse(obj.ChildObject == cloned.ChildObject);
            Assert.IsTrue(obj.PropString == cloned.PropString);
            Assert.IsTrue(obj.ChildObject.PropString == cloned.ChildObject.PropString);
            Assert.IsTrue(cloned.Cloneable.Cloned);
        }
    }

    public class TestType
    {
        public string PropString { get; set; }
        public TestType2 ChildObject { get; set; }
        public TestType3 Cloneable { get; set; }
    }

    public class TestType2
    {
        public string PropString { get; set; }
    }
    
    public class TestType3 : ICloneable
    {
        public bool Cloned;
        public string PropString { get; set; }
        public object Clone()
        {
            return new TestType3()
            {
                PropString = PropString,
                Cloned = true
            };
        }
    }
}