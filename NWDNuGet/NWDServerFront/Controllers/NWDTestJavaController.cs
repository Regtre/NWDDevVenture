using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace NWDServerFront.Controllers;

public class NWDTestJavaController : ControllerBase
{
    
    [HttpPost]
    public string Post([FromBody] DataName tDataName)
    {
        return tDataName.ToString();
    }
    
    [HttpGet]
    public string Get()
    {
        return "Get () ok";
    }
    
}