#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetapp;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Formats;

public class OfferRequest
{
    public string name { get; set; }
}
public class OfferResponse
{
    public int data { get; set; }
}

namespace aspnetapp.Controllers
{
    [Route("api/offer")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OfferController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // POST: api/offer
        [HttpPost]
        public async Task<ActionResult> PostOffer(OfferRequest data)
        {
            if (!string.IsNullOrWhiteSpace(data.name))
            {
                using var client = new HttpClient();

                var response = await client.GetAsync("https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fimg.jj20.com%2Fup%2Fallimg%2F511%2F101611154647%2F111016154647-10-1200.jpg&refer=http%3A%2F%2Fimg.jj20.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1662372086&t=074e443e71868f151624c2e7b731a2af");

                var bytes = await response.Content.ReadAsByteArrayAsync();

                using (Image image = Image.Load(bytes, out IImageFormat format))
                {
                    FontCollection collection = new();

                    collection.Add(_webHostEnvironment.WebRootPath + @"\fonts\微软雅黑.ttf");

                    if (collection.TryGet("Microsoft Yahei", out FontFamily family))
                    {
                        Font font = family.CreateFont(20, FontStyle.Italic);

                        string yourText = "爱你呀";

                        image.Mutate(x => x.DrawText(yourText, font, Color.Black, new PointF(10, 10)));

                        using MemoryStream ms = new();

                        await image.SaveAsync(ms, format);

                        return File(ms.ToArray(), "image/jpeg");
                    }
                }

                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
