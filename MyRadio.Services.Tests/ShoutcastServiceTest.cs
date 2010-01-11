using MyRadio.Services.Shoutcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MyRadio.Services.Tests
{
    
    
    /// <summary>
    ///This is a test class for ShoutcastServiceTest and is intended
    ///to contain all ShoutcastServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ShoutcastServiceTest
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
///A test for MakeWebRequest
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void MakeWebRequestTest1()
{
    //ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    //string url = "http://yp.shoutcast.com/sbin/newxml.phtml";
    //string expected = string.Empty; // TODO: Initialize to an appropriate value
    //string actual;
    //actual = target.MakeWebRequest(url);
    //Assert.AreEqual(expected, actual);
}


/// <summary>
///A test for GetGenres
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void GetGenresTest()
{
    ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    List<ShoutcastGenre> expected = null; // TODO: Initialize to an appropriate value
    List<ShoutcastGenre> actual;
    actual = target.GetGenres();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ParseGenres
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void ParseGenresTest()
{
    //ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    //string xmlString = target.MakeWebRequest("http://yp.shoutcast.com/sbin/newxml.phtml?rss=1");
    //List<ShoutcastGenre> expected = null; // TODO: Initialize to an appropriate value
    //List<ShoutcastGenre> actual;
    //actual = target.ParseGenres(xmlString);
    //Assert.AreEqual(expected, actual);
    //Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetStationsByGenre
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void GetStationsByGenreTest1()
{
    ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    ShoutcastGenre genre = new ShoutcastGenre() { Title = "60s" }; // TODO: Initialize to an appropriate value
    List<ShoutcastStation> expected = null; // TODO: Initialize to an appropriate value
    List<ShoutcastStation> actual;
    actual = target.GetStationsByGenre(genre);
    Assert.AreEqual(expected, actual);
}

/// <summary>
///A test for GetStationsByGenre
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void GetStationsByGenreTest()
{
    ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    string genre = "60s"; // TODO: Initialize to an appropriate value
    List<ShoutcastStation> expected = null; // TODO: Initialize to an appropriate value
    List<ShoutcastStation> actual;
    actual = target.GetStationsByGenre(genre);
    Assert.AreEqual(expected, actual);
}

/// <summary>
///A test for ParseStations
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void ParseStationsTest()
{
    //ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    //string xmlString = target.MakeWebRequest("http://yp.shoutcast.com/sbin/newxml.phtml?genre=60s");
    //List<ShoutcastStation> expected = null; // TODO: Initialize to an appropriate value
    //List<ShoutcastStation> actual;
    //actual = target.ParseStations(xmlString);
    //Assert.AreEqual(expected, actual);
    //Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for CreateStationGenreList
///</summary>
[TestMethod()]
[DeploymentItem("MyRadio.Services.dll")]
public void CreateStationGenreListTest()
{
    ShoutcastService_Accessor target = new ShoutcastService_Accessor(); // TODO: Initialize to an appropriate value
    string genreString = string.Empty; // TODO: Initialize to an appropriate value
    List<ShoutcastGenre> expected = null; // TODO: Initialize to an appropriate value
    List<ShoutcastGenre> actual;
    actual = target.CreateStationGenreList(genreString);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
