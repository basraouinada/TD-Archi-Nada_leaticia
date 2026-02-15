using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;
using Université_Domain.UseCases.SecurityUseCases.Get;
using Université_Domain.UseCases.UeUseCase.Create;
using Université_Domain.UseCases.UeUseCase.Delete;
using Université_Domain.UseCases.UeUseCase.Get;
using Université_Domain.UseCases.UeUseCase.Update;
using UniversiteDomain.Dtos;

namespace UniversiteRestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UeController(IRepositoryFactory repositoryFactory) : ControllerBase{

    [HttpGet("{id}")]
    public async Task<ActionResult<UeDto>> getUe(long id)
    {
        GetUnUeUseCase ueUseCase = new GetUnUeUseCase(repositoryFactory);
        UeDto dto=new UeDto();
        UeDto res = new UeDto();
        string role="";
        string email="";
        IUniversiteUser user = null;
        try
        {
            CheckSecu(out role, out email, out user);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
        
        if (!ueUseCase.IsAuthorized(role)) return Unauthorized();
        try
        {
            res =  dto.ToDto(await ueUseCase.ExecuteAsync(id));

        }
        catch (Exception e)
        {
            return StatusCode(404, e.Message);


        }
        
        return res;
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<Ue>>> getUeall()
    {
        GetAllUeUseCase ueUseCase = new GetAllUeUseCase(repositoryFactory);
        
        string role="";
        string email="";
        IUniversiteUser user = null;
        try
        {
            CheckSecu(out role, out email, out user);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
        
        if (!ueUseCase.IsAuthorized(role)) return Unauthorized();
        
        return await ueUseCase.ExecuteAsync();;
    }

    [HttpPost("create")]
    public async Task<ActionResult<UeDto>> createUe(Ue ue)
    {
        CreateUeUseCase ueUseCase = new CreateUeUseCase(repositoryFactory);
        UeDto dto = new UeDto();
        
        UeDto res = new UeDto();
        
        
        string role="";
        string email="";
        IUniversiteUser user = null;
        try
        {
            CheckSecu(out role, out email, out user);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
        
        if (!ueUseCase.IsAuthorized(role)) return Unauthorized();

        try
        {
            res = dto.ToDto(await ueUseCase.ExecuteAsync(ue));

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
        
        return res;

    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<String>> deleteUe(long id)
    {
        DeleteUeUseCase ueUseCase = new DeleteUeUseCase(repositoryFactory);
        
        string role="";
        string email="";
        IUniversiteUser user = null;
        try
        {
            CheckSecu(out role, out email, out user);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
        
        if (!ueUseCase.IsAuthorized(role)) return Unauthorized();
        
        
        if (await ueUseCase.ExecuteAsync(id))
        {
            return "Ue deleted";
        }
        else
        {
            return "Ue not deleted";

        }
        
        
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult<UeDto>> updateUe(long id, Ue ue)
    {
        UpdateUeUseCase ueUseCase = new UpdateUeUseCase(repositoryFactory);
        UeDto dto = new UeDto();
        
        UeDto res = new UeDto();
        
        
        string role="";
        string email="";
        IUniversiteUser user = null;
        try
        {
            CheckSecu(out role, out email, out user);
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
        
        if (!ueUseCase.IsAuthorized(role)) return Unauthorized();
        try
        {
            res = dto.ToDto(await ueUseCase.ExecuteAsync(id, ue));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return res;
    }
    // finishing the crud for main entities , still need to handle the crud for the relation between the tables many to many ....
    
    private void CheckSecu(out string role, out string email, out IUniversiteUser user)
    {
        role = "";
        ClaimsPrincipal claims = HttpContext.User;

        if (claims.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException();
        if (claims.FindFirst(ClaimTypes.Email) == null) throw new UnauthorizedAccessException();
            
        email = claims.FindFirst(ClaimTypes.Email).Value;
        if (email == null) throw new UnauthorizedAccessException();
            
        user = new FindUniversiteUserByEmailUseCase(repositoryFactory).ExecuteAsync(email).Result;
        if (user == null) throw new UnauthorizedAccessException();
            
        if (claims.FindFirst(ClaimTypes.Role) == null) throw new UnauthorizedAccessException();
            
        var ident = claims.Identities.FirstOrDefault();
        if (ident == null) throw new UnauthorizedAccessException();
            
        role = ident.FindFirst(ClaimTypes.Role).Value;
        if (role == null) throw new UnauthorizedAccessException();
            
        bool isInRole = new IsInRoleUseCase(repositoryFactory).ExecuteAsync(email, role).Result; 
        if (!isInRole) throw new UnauthorizedAccessException();
    }

    
}