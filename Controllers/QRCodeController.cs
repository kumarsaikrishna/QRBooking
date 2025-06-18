using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;
using QRCodeBooking.Models;
 
 
public class QRCodeViewModel
{
    public string QRCodeBase64 { get; set; }
}

public class QRCodeController : Controller
{



    public IActionResult GenerateServiceQRCode()
    {
        string orderUrl = "https://localhost:5001/Orders/Create";

        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(orderUrl, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        string qrBase64 = Convert.ToBase64String(qrCodeBytes);

        var model = new QRCodeViewModel
        {
            QRCodeBase64 = $"data:image/png;base64,{qrBase64}"
        };

        TempData["QRCodeBytes"] = qrCodeBytes;

        return View(model);
    }

    [HttpGet]
    public IActionResult DownloadQRCode()
    {
        string orderUrl = "https://localhost:5001/Orders/Create";

        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(orderUrl, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        return File(qrCodeBytes, "image/png", "OrderQR.png");
    }
}
