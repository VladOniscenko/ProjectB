using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.DataAccess;

namespace UnitTests;

[TestClass]
public class UserLogicTest
{
    [TestMethod]
    public void TestMethod1()
    {
    }
    
    private UserLogic _userLogic;

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
    public void IsNameValid_ValidName_True(string name)
    {
        bool actual = _userLogic.IsNameValid(name);
        Assert.AreEqual(true, actual);
    }


    [DataTestMethod]
    [DataRow("Aba$")]
    [DataRow("Abab3")]
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
    public void IsEmailValid_ValidEmail_True()
    {
        string email = "valid@email.com";
        bool actual = _userLogic.IsEmailValid(email);
        Assert.AreEqual(true, actual);
    }
}