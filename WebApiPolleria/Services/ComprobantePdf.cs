using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WebApiPolleria.Models;

namespace WebApiPolleria.Services
{
    public static class ComprobantePdf
    {
        public static void Generar(string ruta, VentaPdfModel venta)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(35);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Helvetica"));

                    // ================= HEADER =================
                    page.Header().Background(Colors.Grey.Lighten4).Padding(15).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("LA MATRIARCA")
                                .FontSize(20)
                                .Bold();

                            col.Item().Text("Pollería & Parrillas")
                                .FontSize(11)
                                .FontColor(Colors.Grey.Darken2);

                            col.Item().Text("RUC: 20481234567")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);
                        });

                        row.ConstantItem(160).AlignRight().Column(col =>
                        {
                            col.Item().Border(1).BorderColor(Colors.Grey.Darken1).Padding(8).AlignCenter().Column(c =>
                            {
                                c.Item().Text("COMPROBANTE")
                                    .Bold();

                                c.Item().Text($"{venta.Serie}-{venta.Numero}")
                                    .FontSize(14)
                                    .Bold();
                            });
                        });
                    });

                    // ================= CONTENT =================
                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Spacing(12);

                        // Datos generales
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text($"Fecha: {venta.FechaHora:dd/MM/yyyy HH:mm}");

                            });

                            row.ConstantItem(180)
                               .Background(Colors.Green.Lighten4)
                               .Padding(10)
                               .AlignCenter()
                               .Column(c =>
                               {
                                   c.Item().Text("TOTAL A PAGAR")
                                       .Bold();

                                   c.Item().Text($"S/ {venta.Total:0.00}")
                                       .FontSize(18)
                                       .Bold()
                                       .FontColor(Colors.Green.Darken3);
                               });
                        });

                        col.Item().LineHorizontal(0.8f);

                        // Tabla productos
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Producto").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Cant.").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("P.Unit").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Subtotal").Bold();
                            });

                            bool alternar = false;

                            foreach (var d in venta.Detalle)
                            {
                                var bg = alternar ? Colors.Grey.Lighten5 : Colors.White;
                                alternar = !alternar;

                                table.Cell().Background(bg).Padding(5).Text(d.Producto);
                                table.Cell().Background(bg).Padding(5).AlignRight().Text(d.Cantidad.ToString("0.##"));
                                table.Cell().Background(bg).Padding(5).AlignRight().Text($"S/ {d.PrecioUnitario:0.00}");
                                table.Cell().Background(bg).Padding(5).AlignRight().Text($"S/ {d.Subtotal:0.00}");
                            }
                        });

                        // Totales
                        col.Item().AlignRight().PaddingTop(10).Border(1).Padding(10).Column(c =>
                        {
                            c.Item().Text($"Subtotal: S/ {venta.Subtotal:0.00}");
                            c.Item().Text($"IGV (18%): S/ {venta.Igv:0.00}");
                            c.Item().LineHorizontal(0.5f);
                            c.Item().Text($"TOTAL: S/ {venta.Total:0.00}")
                                .FontSize(12)
                                .Bold();
                        });
                    });

                    // ================= FOOTER =================
                    page.Footer().PaddingTop(10).AlignCenter().Column(col =>
                    {
                        col.Item().Text("Gracias por su preferencia 💛")
                            .FontSize(9);

                        col.Item().Text("Este documento es una representación impresa del comprobante electrónico.")
                            .FontSize(8)
                            .FontColor(Colors.Grey.Darken2);
                    });
                });
            }).GeneratePdf(ruta);
        }
    }
}

