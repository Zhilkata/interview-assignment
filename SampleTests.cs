/* Sample tests from set up. Left for discussion/historical reason, otherwise ignored from execution. */

namespace RoseticTask;

[Ignore]
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