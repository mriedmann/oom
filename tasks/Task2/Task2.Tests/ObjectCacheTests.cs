using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Tests
{
    public class TestObject
    {
        public string Prop1;
        public int Prop2;
    }

    [TestFixture]
    public class ObjectCacheTests
    {
        string actualPath;
        string expectedPath;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var dir = Path.GetDirectoryName(typeof(Task2.ObjectCache).Assembly.Location);
            Environment.CurrentDirectory = dir;

            actualPath = Path.Combine(Environment.CurrentDirectory, "cache", "testObj.json");
            expectedPath = Path.Combine(Environment.CurrentDirectory, "exp_testObj.json");

            File.Delete(actualPath);
        }

        [Test]
        public void TestInit()
        {
            Assert.NotNull(ObjectCache.Instance);
        }

        [Test]
        public void TestStoring()
        {
            var testObject = new TestObject { Prop1 = "test", Prop2 = 1234 };
            ObjectCache.Instance.SetObject("testObj", testObject);

            FileAssert.Exists(actualPath);
            FileAssert.AreEqual(expectedPath, actualPath);
        }

        [Test]
        public void TestRetrieving()
        {
            File.Copy(this.expectedPath, this.actualPath, true);

            var expTestObject = new TestObject { Prop1 = "test", Prop2 = 1234 };
            var actTestObject = ObjectCache.Instance.GetObject<TestObject>("testObj");

            Assert.AreEqual(expTestObject.Prop1, actTestObject.Prop1);
            Assert.AreEqual(expTestObject.Prop2, actTestObject.Prop2);
        }

        [Test]
        public void TestRetrievingOfNonExisting()
        {
            var obj = ObjectCache.Instance.GetObject<TestObject>("isNonExisting");
            Assert.AreEqual(default(TestObject), obj);
        }

        [Test]
        public void TestStoringOfInvalidObject()
        {
            Assert.Throws<JsonSerializationException>(() => ObjectCache.Instance.SetObject("invalid", System.Threading.Thread.CurrentThread));
        }

        [Test]
        public void TestGetOrSetIfNew()
        {
            var expTestObject = new TestObject { Prop1 = "test", Prop2 = 1234 };
            var actTestObject = ObjectCache.Instance.GetOrSetObject("testObj", () => new TestObject { Prop1 = "test", Prop2 = 1234 });

            Assert.AreEqual(expTestObject.Prop1, actTestObject.Prop1);
            Assert.AreEqual(expTestObject.Prop2, actTestObject.Prop2);

            FileAssert.Exists(actualPath);
            FileAssert.AreEqual(expectedPath, actualPath);
        }

        [Test]
        public void TestGetOrSetIfExists()
        {
            File.Copy(this.expectedPath, this.actualPath, true);

            var expTestObject = new TestObject { Prop1 = "test", Prop2 = 1234 };
            var actTestObject = ObjectCache.Instance.GetOrSetObject("testObj", () => new TestObject { Prop1 = "test", Prop2 = 1234 });

            Assert.AreEqual(expTestObject.Prop1, actTestObject.Prop1);
            Assert.AreEqual(expTestObject.Prop2, actTestObject.Prop2);

            FileAssert.Exists(actualPath);
            FileAssert.AreEqual(expectedPath, actualPath);
        }

        [Test]
        public void TestCleaning()
        {
            ObjectCache.Instance.SetObject("testObj", new TestObject { Prop1 = "test", Prop2 = 1234 });

            FileAssert.Exists(actualPath);
            ObjectCache.Instance.Clear();
            FileAssert.DoesNotExist(actualPath);
        }
    }
}
