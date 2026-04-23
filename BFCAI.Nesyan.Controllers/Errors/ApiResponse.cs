using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode, string? message=null )
        {
            StatusCode = statusCode;
            Message = message??GetDefaultMessage(StatusCode);
        }
        public string? GetDefaultMessage(int statusCode)
        {

            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource was not found",
                500 => "Errors are the bath to the dark side. Errors lead to anger. anger lead to hate. hate lead to carrer change",
                _ => null
            };

        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        }
    }
}
