using Microsoft.AspNetCore.Mvc;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Framework;

namespace P2Project.Volunteers.Web;

public class TestController : ApplicationController
{
    [HttpGet]
    public ActionResult<string[]> Get()
    {
        var names = new[] {"Oleg", "Kate", "Morris"};

        return names;
    }
}