using System.Runtime.CompilerServices;

namespace UnitTests;

[TestClass]
public class UserLogicTest
{
    [TestMethod]
    public void TestMethod1()
    {
    }

    [DataTestMethod]
    [DataRow("Ab")]
    [DataRow("A")]
    [DataRow("")]
    public void IsNameValid_CharacterLengthTooSmall_False(string name)
    {
            bool actual = UserLogic.IsNameValid(name);

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
            bool actual = UserLogic.IsNameValid(name);

            Assert.AreEqual(true, actual);

    }

    [DataTestMethod]
    [DataRow("Aba$")]
    [DataRow("Abab3")]
    public void IsNameValid_NameWithSymbolsOrNumbers_false(string name)
        {
            bool actual = UserLogic.IsNameValid(name);

            Assert.AreEqual(false, false);
        }
        }