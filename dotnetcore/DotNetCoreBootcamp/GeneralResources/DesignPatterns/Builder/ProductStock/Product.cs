using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Builder.ProductStock
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ProductStockReport
    {
        public string HeaderPart { get; set; }
        public string BodyPart { get; set; }
        public string FooterPart { get; set; }

        public override string ToString() =>
            new StringBuilder()
            .AppendLine(HeaderPart)
            .AppendLine(BodyPart)
            .AppendLine(FooterPart)
            .ToString();
    }

    public interface IProductStockReportBuilder
    {
        void BuildHeader();
        void BuildBody();
        void BuildFooter();
        ProductStockReport GetReport();
    }

    public class ProductStockReportBuilder : IProductStockReportBuilder
    {
        ProductStockReport _productStockReport;
        IEnumerable<Product> _products;

        public ProductStockReportBuilder(IEnumerable<Product> products)
        {
            _products = products;
            _productStockReport = new ProductStockReport();
        }

        public void BuildBody()
        {
            _productStockReport.BodyPart = 
                string.Join(Environment.NewLine, _products.Select(p => $"Product name: {p.Name}, product price: {p.Price}"));
        }

        public void BuildFooter()
        {
            _productStockReport.FooterPart = "\nReport provided by the IT_PRODUCTS company.";
        }

        public void BuildHeader()
        {
            _productStockReport.HeaderPart = $"STOCK REPORT FOR ALL THE PRODUCTS ON DATE: {DateTime.Now}\n";
        }

        public ProductStockReport GetReport()
        {
            var productReport = _productStockReport;
            Clear();
            return productReport;
        }

        private void Clear() => _productStockReport = new ProductStockReport();
    }

    public class ProductStockReportDirector
    {
        private readonly IProductStockReportBuilder _productStockReportBuilder;

        public ProductStockReportDirector(IProductStockReportBuilder productStockReportBuilder)
        {
            _productStockReportBuilder = productStockReportBuilder;
        }

        public void BuildStockReport()
        {
            _productStockReportBuilder.BuildHeader();
            _productStockReportBuilder.BuildBody();
            _productStockReportBuilder.BuildFooter();
        }

    }
}
