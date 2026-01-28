using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using TodoApi.Data;
using TodoApi.Models;


namespace TodoApiPlaywrightApiTests;

public class TodoApiTests
{
    private IAPIRequestContext req = null;
    public async Task TodoApiTests()
    {
        req = await APIRequest.NewContextAsync(new() {
            BaseURL = "https://localhost:5001",
        });
    }
        
    [Fact]
    public async Task Get_ReturnsListWithCorrectNumberOfItems_Async()
    {
    }
}