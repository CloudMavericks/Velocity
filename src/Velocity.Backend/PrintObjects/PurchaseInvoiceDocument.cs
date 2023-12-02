using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using Velocity.Shared.Responses.PurchaseInvoices;
using Velocity.Shared.Responses.Suppliers;

namespace Velocity.Backend.PrintObjects;

public class PurchaseInvoiceDocument : IDocument
{
    private readonly PurchaseInvoiceResponse _purchaseOrderResponse;
    private readonly SupplierResponse _supplierResponse;

    public PurchaseInvoiceDocument(PurchaseInvoiceResponse purchaseOrderResponse, SupplierResponse supplierResponse)
    {
        _purchaseOrderResponse = purchaseOrderResponse;
        _supplierResponse = supplierResponse;
    }
    
    public static byte[] Generate(PurchaseInvoiceResponse purchaseOrderResponse, SupplierResponse supplierResponse)
    {
        return new PurchaseInvoiceDocument(purchaseOrderResponse, supplierResponse).GeneratePdf();
    }
    
    public void Compose(IDocumentContainer container)
    {
        
        container.Page(page =>
        {
            page.DefaultTextStyle(x => x.FontFamily("Segoe UI"));
            page.Margin(50);
            
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            
            page.Footer().Column(column =>
            {
                column.Spacing(5);
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("Supplier Signature").FontSize(8f);
                    row.RelativeItem().AlignRight().Text("Receiver Signature").FontSize(8f);
                });
                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text(x =>
                    {
                        x.Span("Printed on: ").SemiBold();
                        x.Span($"{DateTime.Now:dd/MM/yyyy hh:mm:ss tt}");
                    });
                    row.RelativeItem().AlignRight().Text(x =>
                    {
                        x.Span("Page: ").SemiBold();
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor("594AE2");
    
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Invoice Number: {_purchaseOrderResponse.InvoiceNumber}").Style(titleStyle);

                column.Item().Text(text =>
                {
                    text.Span("Invoice date: ").SemiBold();
                    text.Span($"{_purchaseOrderResponse.InvoiceDate:dd/MM/yyyy}");
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Reference Number: ").SemiBold();
                    text.Span($"{_purchaseOrderResponse.ReferenceNumber}");
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Purchase Date: ").SemiBold();
                    text.Span($"{_purchaseOrderResponse.PurchaseDate:dd/MM/yyyy}");
                });

                column.Item().Row(innerRow =>
                {
                    innerRow.RelativeItem().Text("Order Status: ").SemiBold();
                    innerRow.RelativeItem().MinimalBox().Layers(layers =>
                    {
                        layers.Layer().Canvas((canvas, size) =>
                        {
                            DrawRoundedRectangle(Colors.White, false);
                            DrawRoundedRectangle(Colors.Blue.Darken2, true);
                            return;

                            void DrawRoundedRectangle(string color, bool isStroke)
                            {
                                using var paint = new SKPaint();
                                paint.Color = SKColor.Parse(color);
                                paint.IsStroke = isStroke;
                                paint.StrokeWidth = 2;
                                paint.IsAntialias = true;

                                canvas.DrawRoundRect(0, 0, size.Width, size.Height, 20, 20, paint);
                            }
                        });

                        layers
                            .PrimaryLayer()
                            .PaddingVertical(3)
                            .PaddingHorizontal(7)
                            .Text($"{_purchaseOrderResponse.Status}")
                            .FontSize(9f).FontColor(Colors.Blue.Darken2).SemiBold();
                    });
                });
            });

            row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(10).Column(column =>
        {
            column.Spacing(5);
            
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(innerColumn =>
                {
                    innerColumn.Spacing(2);

                    innerColumn.Item().BorderBottom(1).PaddingBottom(5).Text("Supplier").SemiBold();

                    innerColumn.Item().Text($"{_supplierResponse.ContactName} - {_supplierResponse.Name}")
                        .FontSize(11f);
                    innerColumn.Item().Text($"{_supplierResponse.Address}").FontSize(10f);
                    innerColumn.Item()
                        .Text($"{_supplierResponse.City}, {_supplierResponse.State} - {_supplierResponse.ZipCode}")
                        .FontSize(10f);
                    innerColumn.Item().Text(text =>
                    {
                        text.Span("GSTN: ").SemiBold().FontSize(10f);
                        text.Span(_supplierResponse.Gstn).FontSize(10f);
                    });
                    innerColumn.Item().Text(text =>
                    {
                        text.Span("Email: ").SemiBold().FontSize(10f);
                        text.Span(_supplierResponse.ContactEmail).FontSize(10f);
                    });
                    innerColumn.Item().Text(text =>
                    {
                        text.Span("Phone: ").SemiBold().FontSize(10f);
                        text.Span(_supplierResponse.ContactPhone).FontSize(10f);
                    });
                });
            });

            column.Item().Element(ComposeTable);
            
            column.Item().AlignRight().Text($"Grand total: {_purchaseOrderResponse.Items.Sum(x => x.Total):C}").FontSize(13f);
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.ConstantColumn(100);
                columns.RelativeColumn(3);
                columns.RelativeColumn(3);
                columns.RelativeColumn(3);
                columns.RelativeColumn(3);
                columns.RelativeColumn(4);
                columns.RelativeColumn(6);
            });
            
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Product");
                header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                header.Cell().Element(CellStyle).AlignRight().Text("Amount");
                header.Cell().Element(CellStyle).AlignRight().Text("Tax %");
                header.Cell().Element(CellStyle).AlignRight().Text("Discount");
                header.Cell().Element(CellStyle).AlignRight().Text("Total");
                return;

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold().FontSize(11f)).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });
            
            foreach (var item in _purchaseOrderResponse.Items)
            {
                table.Cell().Element(CellStyle).Text($"{_purchaseOrderResponse.Items.IndexOf(item) + 1}");
                table.Cell().Element(CellStyle).Text(item.Product);
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.TotalPrice:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.TaxPercentage}%");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.DiscountAmount:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Total:C}");
                continue;

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.FontSize(10f)).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }
}