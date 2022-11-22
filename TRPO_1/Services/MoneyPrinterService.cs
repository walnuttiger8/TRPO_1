using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Word;
using TRPO_1.Models;
using Word = Microsoft.Office.Interop.Word.Application;

namespace TRPO_1.Services
{
    public class MoneyPrinterService
    {
        public Word PrintMoneyReport(string title, string caption, List<MoneyUnit> moneyUnits)
        {
            List<IGrouping<Bitmap, MoneyUnit>> moneyUnitsByNominal = moneyUnits
                .GroupBy(selector => selector.Image)
                .OrderByDescending(selector => selector.Count())
                .ToList();

            int totalSum = moneyUnits.Sum(money => money.Quantity);
            var application = new Word();

            Document document = application.Documents.Add();
            
            Paragraph titleParagraph = document.Paragraphs.Add();
            
            Range range = document.Range();

            titleParagraph.set_Style("Заголовок 1");
            titleParagraph.Range.Text = $"{title}\n";
            
            foreach (IGrouping<Bitmap,MoneyUnit> units in moneyUnitsByNominal)
            {
                int count = units.Count();
                Bitmap keyBill = units.Key;

                (string pictureFileName, int width, int height) = SaveTemp(keyBill);

                for (int i = 0; i < count; i++)
                {
                    var picture = range.InlineShapes.AddPicture(pictureFileName);
                    picture.Width = width - height > 100 ? 125 : 40;
                    picture.Height = width - height > 100 ? 50 : 40;
                }
                
                DeleteTemp(pictureFileName);
            }

            Paragraph sumAndCaptionParagraph = document.Paragraphs.Add();
            sumAndCaptionParagraph.Range.Text = $"Итого: {totalSum}₽";

            sumAndCaptionParagraph.Range.Text += $"\n{caption}";
            
            document.Words.Last.InsertBreak(WdBreakType.wdSectionBreakNextPage);

            return application;
        }

        private (string fileName, int width, int height) SaveTemp(Bitmap bitmap)
        {
            string fileName = $"{Directory.GetCurrentDirectory()}{Guid.NewGuid()}.png";
            
            bitmap.Save(fileName, ImageFormat.Png);

            return (fileName, bitmap.Width, bitmap.Height);
        }

        private void DeleteTemp(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            
            fileInfo.Delete();
        }
    }
}