using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
                    {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
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
            decimal total;

            try
            {
                if (receipt != null && receipt.IdClient >= 0 && receipt.ReceiptLineList.Count > 0)
                {
                    DataTable receiptLineTable = new DataTable();
                    receiptLineTable.Columns.Add("idProduct", typeof(int));
                    receiptLineTable.Columns.Add("Price", typeof(decimal));
                    receiptLineTable.Columns.Add("count", typeof(int));

                    total = receipt.GetTotal();

                    if (total.ToString().Length > 18)
                    {

                        return StatusCode(500, "Precio total excede el limite. Por favor revise la cantidad de productos.");

                    }

                Dictionary<string, object> args = new Dictionary<string, object>
                    {
                         {"pTotal", total},
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
