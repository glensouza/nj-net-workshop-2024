using HtmlAgilityPack;
using System.Net;
using System.Xml;

namespace CarSnapScore.Services;

public class CarDoesNotExist
{
    private const string CarDoesNotExistUrl = "https://www.thisautomobiledoesnotexist.com/";
    private readonly HttpClient httpClient = new();
    private readonly HtmlDocument htmlDoc = new();

    public async Task<string> GetPicture()
    {
        // go get a car image
        string carDoesNotExistHtml = await this.httpClient.GetStringAsync(CarDoesNotExistUrl);
        if (string.IsNullOrEmpty(carDoesNotExistHtml))
        {
            throw new Exception("website down!");
        }

        this.htmlDoc.LoadHtml(carDoesNotExistHtml);

        // Check if the image exists
        HtmlNode imgNode = this.htmlDoc.DocumentNode.SelectSingleNode("//img[@id='vehicle']") ?? throw new Exception("website down!");
        string src = imgNode.GetAttributeValue("src", "");
        return src;
    }
}
