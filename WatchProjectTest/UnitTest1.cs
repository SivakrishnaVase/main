using WatchProject.Controllers;

namespace WatchProjectTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        WatchController obj = new WatchController();
        List<string> vals = new List<string>();
        vals.Add("001");
        obj.Checkout(vals);
        
    }

    
}
