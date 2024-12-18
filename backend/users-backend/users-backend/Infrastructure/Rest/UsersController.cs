using framework.Application;
using framework.Domain.Persistence;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using users_backend.Application.Dtos;
using users_backend.Application.Services;

namespace users_backend.Infrastructure.Rest;

[Route("/users")]
[ApiController]
public class UsersController : GenericCrudController<UserDto>
{
    private IUserService _userService;

    public UsersController(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    [NonAction]
    public override ActionResult<IEnumerable<UserDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Produces("application/json")]
    public ActionResult<PagedResponse<UserDto>> Get([FromQuery] string? filter,
        [FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            PagedList<UserDto> page = _userService.GetUsersByCriteriaPaged(filter, paginationParameters);
            var response = new PagedResponse<UserDto>
            {
                CurrentPage = page.CurrentPage,
                TotalPages = page.TotalPages,
                PageSize = page.PageSize,
                TotalCount = page.TotalCount,
                Data = page
            };
            return Ok(response);
        }
        catch (MalformedFilterException)
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public override ActionResult<UserDto> Update(UserDto userDto)
    {
        try
        {
            var updatedUser = _userService.Update(userDto);
            return Ok(updatedUser);
        }
        catch (ConcurrencyException ex){
            return Conflict(new { message = ex.Message });
        }
    }
}