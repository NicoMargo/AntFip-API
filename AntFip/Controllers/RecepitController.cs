using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetReceiptById(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    {"pId", id}
                };
                return Ok(DBHelper.callProcedureReader("spVoucherGetById", args));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error al obtener la información del recibo. " + e.Message);
            }
        }


        // GET BY ID USER
        [HttpGet]
        public IActionResult GetReceiptByIdUser()
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    {"pId", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                };
                return Ok(DBHelper.callProcedureReader("spVoucherGetAllByIdUser", args));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error al obtener la información del recibo. " + e.Message);
            }
        }


        // CREATE
        [HttpPost]
        public IActionResult Create(Receipt receipt)
        {            

            int success;
            try
            {
                if (receipt != null && receipt.IdClient >= 0 && receipt.ReceiptLineList.Count > 0)
                {
                    DataTable receiptLineTable = new DataTable();
                    receiptLineTable.Columns.Add("idProduct", typeof(int));
                    receiptLineTable.Columns.Add("Price", typeof(double));
                    receiptLineTable.Columns.Add("count", typeof(int));
                    
                    
                    Dictionary<string, object> args = new Dictionary<string, object>
                    {
                         {"pDate", receipt.Date},
                         {"pTotal", receipt.GetTotal()},
                         {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)},
                         {"pIdClient", receipt.IdClient}
                    };

                    foreach (var line in receipt.ReceiptLineList)
                    {
                        DataRow row = receiptLineTable.NewRow();
                        row["idProduct"] = Convert.ToInt32(line.IdProduct);
                        row["price"] = line.Price;
                        row["count"] = line.Quantity;
                        receiptLineTable.Rows.Add(row);
                    }

                    success = DBHelper.CallNonQueryTable("spVoucherCreate", args, receiptLineTable, "LineVoucherType");

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
