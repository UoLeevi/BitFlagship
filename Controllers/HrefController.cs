using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BitFlagship.Controllers
{
    [Route("/href/")]
    public class HrefController : Controller
    {
        HttpClient httpClient = new HttpClient();

        [HttpGet]
        public IActionResult Get()
            => Ok("With the href API you can quickly get a list of outbound links on a webpage.\r\n" +
                  "Just paste the URL of the webpage in the address bar after the 'href/'.\r\n" +
                  "Example: bitflagship.com/href/google.com");

        [HttpGet("http/{part0}/{part1?}/{part2?}/{part3?}/{part4?}/{part5?}")]
        public async Task<IActionResult> GetHrefsForHttp(
            string protocol,
            string part0,
            string part1 = null,
            string part2 = null,
            string part3 = null,
            string part4 = null,
            string part5 = null)
            => await GetHrefs("http", part0, part1, part2, part3, part4, part5);

        [HttpGet("{part0}/{part1?}/{part2?}/{part3?}/{part4?}/{part5?}")]
        [HttpGet("https/{part0}/{part1?}/{part2?}/{part3?}/{part4?}/{part5?}")]
        public async Task<IActionResult> GetHrefsForHttps(
            string protocol,
            string part0,
            string part1 = null,
            string part2 = null,
            string part3 = null,
            string part4 = null,
            string part5 = null)
            => await GetHrefs("https", part0, part1, part2, part3, part4, part5);

        public async Task<IActionResult> GetHrefs(
            string protocol, string part0, string part1, string part2, string part3, string part4, string part5)
        {
            string part1OrNull = part1 is null ? string.Empty : $"/{part1}";
            string part2OrNull = part2 is null ? string.Empty : $"/{part2}";
            string part3OrNull = part3 is null ? string.Empty : $"/{part3}";
            string part4OrNull = part4 is null ? string.Empty : $"/{part4}";
            string part5OrNull = part5 is null ? string.Empty : $"/{part5}";
            if (!Uri.TryCreate($@"{protocol}://{
                part0}{
                part1OrNull}{
                part2OrNull}{
                part3OrNull}{
                part4OrNull}{
                part5OrNull}",
                UriKind.Absolute, out Uri uri))
                return BadRequest("It seems like the specified text string is not a valid URL!");
            else
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    return BadRequest($"The URL specified is formatted correctly but the web request replied with status code {response.StatusCode}.");
                else
                {
                    string textResponse = await response.Content.ReadAsStringAsync();
                    Regex hrefRegex = new Regex("<a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1");
                    Match hrefMatch = hrefRegex.Match(textResponse);
                    List<string> list = new List<string>();
                    while (hrefMatch.Success)
                    {
                        string href = hrefMatch.Groups[2].Value;
                        list.Add(hrefMatch.Groups[2].Value);
                        hrefMatch = hrefMatch.NextMatch();
                    }
                    list.Sort();
                    return Ok($"{string.Join(Environment.NewLine, list.ToArray())}");
                }
            }

        }
    }
}