namespace RoseticTask;

[TestClass]
public class SampleTests
{
    [TestMethod]
    public Task PassTest()
    {
        Assert.IsTrue(true, "Unexpected :(");
        return Task.CompletedTask;
    }
    
    [TestMethod]
    public Task FailTest()
    {
        //Assert.Fail("Expected :)");
        return Task.CompletedTask;
    }
}