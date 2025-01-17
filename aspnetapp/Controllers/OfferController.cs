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
using System.Globalization;

public class OfferRequest
{
    public string name { get; set; }

    public string date { get; set; }
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
            if (!string.IsNullOrWhiteSpace(data.name) && !string.IsNullOrWhiteSpace(data.date))
            {
                using (Image image = Image.Load(_webHostEnvironment.WebRootPath + @"/images/offer.png", out IImageFormat format))
                {
                    FontCollection collection = new();

                    collection.Add(_webHostEnvironment.WebRootPath + @"/fonts/msyh.ttf");

                    if (collection.TryGet("Microsoft Yahei", out FontFamily family))
                    {
                        var font1 = family.CreateFont(72, FontStyle.Regular);

                        FontRectangle size1 = TextMeasurer.Measure(data.name, new TextOptions(font1));

                        image.Mutate(x => x.DrawText(data.name, font1, Color.Black, new PointF(762 + (470 - size1.Width) / 2, 642)));

                        var font2 = family.CreateFont(32, FontStyle.Regular);

                        FontRectangle size2 = TextMeasurer.Measure(data.date, new TextOptions(font2));

                        image.Mutate(x => x.DrawText(data.date, font2, Color.Black, new PointF(1425 + (278 - size2.Width) / 2, 1089)));

                        using MemoryStream ms = new();

                        await image.SaveAsJpegAsync(ms);

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
