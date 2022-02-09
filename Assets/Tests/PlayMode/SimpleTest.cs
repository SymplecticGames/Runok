using NUnit.Framework;

public class SimpleTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SimpleTestSimplePasses()
    {
        Assert.AreEqual(2 + 2, 4);
    }
}
