using MyRadio.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace MyRadio.Services.Tests
{
    
    
    /// <summary>
    ///This is a test class for WebRequestHandlerTest and is intended
    ///to contain all WebRequestHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebRequestHandlerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for MakeWebRequestAsync
        ///</summary>
        [TestMethod()]
        public void MakeWebRequestAsyncTest()
        {
            WebRequestHandler target = new WebRequestHandler(); // TODO: Initialize to an appropriate value
            string url = "http://yp.shoutcast.com/sbin/newxml.phtml";
            int timeOut = 20000; // TODO: Initialize to an appropriate value
            target.AsyncResponseArrived += new System.Action<string>(target_AsyncResponseArrived);
            target.Error += new System.Action<System.Exception>(target_Error);
            m_Time = DateTime.Now;
            target.MakeWebRequestAsync(url, timeOut);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        DateTime m_Time;

        void target_Error(System.Exception obj)
        {
        }

        void target_AsyncResponseArrived(string obj)
        {
            TimeSpan elapsed = DateTime.Now - m_Time;
        }

        /// <summary>
        ///A test for MakeWebRequest
        ///</summary>
        [TestMethod()]
        public void MakeWebRequestTest()
        {
            WebRequestHandler target = new WebRequestHandler(); // TODO: Initialize to an appropriate value
            string url = "http://yp.shoutcast.com/sbin/newxml.phtml";
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            m_Time = DateTime.Now;
            actual = target.MakeWebRequest(url);
            TimeSpan elapsed = DateTime.Now - m_Time;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
