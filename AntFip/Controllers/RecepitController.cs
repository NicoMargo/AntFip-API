using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetReceiptById(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    {"pId", id}
                };
                return Ok(DBHelper.callProcedureReader("spReceiptGetById", args));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error al obtener la información del recibo. " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult Create()
        {
            Receipt receipt = new Receipt();
            receipt.ReceiptLineList = new List<ReceiptLine>();
            ReceiptLine line1 = new ReceiptLine();
            line1.Quantity = 3;
            line1.IdProduct = 1;

            ReceiptLine line2 = new ReceiptLine();
            line2.Quantity = 2;
            line2.IdProduct = 2;

            ReceiptLine line3 = new ReceiptLine();
            line3.Quantity = 1;
            line3.IdProduct = 5;

            receipt.ReceiptLineList.Add(line1);
            receipt.ReceiptLineList.Add(line2); 
            receipt.ReceiptLineList.Add(line3); 

            int success;
            try
            {
                if (receipt != null && receipt.IdClient >= 0 && receipt.ReceiptLineList.Count > 0)
                {
                    DataTable receiptLineTable = new DataTable();
                    receiptLineTable.Columns.Add("IdProduct", typeof(int));
                    receiptLineTable.Columns.Add("Quantity", typeof(int));
                    receiptLineTable.Columns.Add("Price", typeof(double));

                    foreach (var line in receipt.ReceiptLineList)
                    {
                        DataRow row = receiptLineTable.NewRow();
                        row["IdProduct"] = Convert.ToInt32(line.IdProduct);
                        row["Quantity"] = line.Quantity;
                        row["Price"] = line.Price;
                        receiptLineTable.Rows.Add(row);
                    }
                    
                    Dictionary<string, object> args = new Dictionary<string, object>
                    {
                         {"pDate", receipt.Date},
                         {"pTotal", receipt.GetTotal()},
                         {"pCuitUser", receipt.CuitUser},
                         {"pIdClient", receipt.IdClient}
                    };

                    success = DBHelper.CallNonQueryTable("spReceiptCreate", args, receiptLineTable, "ReceiptLineType");

                    if (success > 1)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(500, "Error al guardar el recibo :(");
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error al guardar el recibo" + e.Message);
            }
            return StatusCode(500, "Error al guardar el recibo");
        }



    }
}
