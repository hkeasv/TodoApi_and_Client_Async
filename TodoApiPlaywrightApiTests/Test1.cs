using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Newtonsoft.Json;
using TodoApi.Models;

namespace TodoApiPlaywrightApiTests;

[TestClass]
public class Test1 : PageTest
{
    private IAPIRequestContext request = null!;
    
    [TestInitialize]
    public async Task SetUpApiTesting()
    {
        request = await this.Playwright.APIRequest.NewContextAsync(new() {
            BaseURL = "https://localhost:5001",
            IgnoreHTTPSErrors = true
        });
    }
    
    [TestCleanup]
    public async Task TearDownApiTesting()
    {
        await request.DisposeAsync();
    }

    [TestMethod]
    public async Task Get_ReturnsListWithCorrectNumberOfItems_Async()
    {
        var response = await request.GetAsync("/Todo");
        
        //var todoItemsJson = await response.JsonAsync<TodoItem[]>();

        //var todoItems = JsonConvert.DeserializeObject<TodoItem>(string);
        
        
        Assert.IsNotNull(response);
        Assert.AreEqual(200, response.Status);
        //Assert.AreEqual(2, todoItemsJson);
    }
}