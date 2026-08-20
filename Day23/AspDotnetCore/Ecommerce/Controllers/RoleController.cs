using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly EcommerceDataContext _context;

    public RoleController(EcommerceDataContext context)
    {
        _context = context;
    }
    [HttpPost]

    public ActionResult Create([FromBody] Role role)
    {
        if (role == null)
        {
            return BadRequest();
        }

        Role roleToUpdate = new Role
        {
            RoleName = role.RoleName,
            RoleId = role.RoleId
        };
        _context.Roles.Add(roleToUpdate);

        return Ok("Yes Update");




    }

}
