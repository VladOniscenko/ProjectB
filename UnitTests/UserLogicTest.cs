using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.DataAccess;

namespace ProjectB.UnitTests;

[TestClass]
public class UserLogicTest
{
    [TestMethod]
    public void TestMethod1()
    {
    }
    
    private UserLogic? _userLogic;

    [TestInitialize]
    public void Initialize()
    {
        var userRepository = new UserRepository();
        _userLogic = new UserLogic(userRepository);
    }


    [DataTestMethod]
    [DataRow("Ab")]
    [DataRow("A")]
    [DataRow("")]
    public void IsNameValid_CharacterLengthTooSmall_False(string name)
    {
        bool actual = _userLogic.IsNameValid(name);
        Assert.AreEqual(false, actual);
    }


    [DataTestMethod]
    [DataRow("Aba")]
    [DataRow("Abab")]
    [DataRow("Ababa")]
    [DataRow("Ababab")]
    [DataRow("Abababa")]
    [DataRow("Elizabeth")]
    [DataRow("aBc")]
    public void IsNameValid_ValidName_True(string name)
    {
        bool actual = _userLogic.IsNameValid(name);
        Assert.AreEqual(true, actual);
    }

    [DataTestMethod]
    [DataRow("Aba$")]
    [DataRow("")]
    [DataRow("Abab3")]
    [DataRow("John3")]
    [DataRow("Ana@")]
    [DataRow("Anna Lee1")]
    public void IsNameValid_NameWithSymbolsOrNumbers_false(string name)
    {
        bool actual = _userLogic.IsNameValid(name);
        Assert.AreEqual(false, actual);
    }


    [DataTestMethod]
    [DataRow("Notanemail")]
    [DataRow("Notanemail@")]
    [DataRow("@Notanemail")]
    public void IsEmailValid_InvalidEmail_false(string email)
    {
        bool actual = _userLogic.IsEmailValid(email);
        Assert.AreEqual(false, actual);
    }


    [TestMethod]
    [DataRow("email@email.com")]
    [DataRow("a@gmail.com")]
    [DataRow("cinema@hotmail.com")]
    public void IsEmailValid_ValidEmail_True(string email)
    {
        bool actual = _userLogic.IsEmailValid(email);
        Assert.AreEqual(true, actual);
    }
    
    [TestMethod]
    [DataRow("strongpass1", true)]
    [DataRow("Password123", true)]
    [DataRow("SuperSecure!", true)]
    [DataRow("short", false)]
    [DataRow("", false)]
    [DataRow("1234567", false)]
    public void IsPasswordValid_TestCases(string password, bool expectedResult)
    {
        bool actual = _userLogic.IsPasswordValid(password);
        Assert.AreEqual(expectedResult, actual);
    }
}