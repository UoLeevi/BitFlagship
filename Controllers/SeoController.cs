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
    [Route("/api/seo/")]
    public class SeoController : Controller
    {
        HttpClient httpClient = new HttpClient();

        [HttpGet("{protocol?}/{uriString}")]
        public async Task<IActionResult> GetAnchorTags(string protocol, string uriString)
        {
            if (!Uri.TryCreate($"{protocol}://{uriString}", UriKind.Absolute, out Uri uri))
                return BadRequest("It seems like the specified text string is not a valid URL!");
            else
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    return BadRequest( $"The URL specified is formatted correctly but the web request replied with status code {response.StatusCode}.");
                else
                {
                    string textResponse = await response.Content.ReadAsStringAsync();
                    Regex hrefRegex = new Regex("<a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1");
                    Match hrefMatch = hrefRegex.Match(textResponse);
                    List<string> list = new List<string>();
                    while (hrefMatch.Success)
                    {
                        list.Add(hrefMatch.Groups[2].Value);
                        hrefMatch = hrefMatch.NextMatch();
                    }
                    return Ok($"{string.Join(Environment.NewLine, list.ToArray())}");
                }
            }
        }
    }
}