using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;
using Université_Domain.UseCases.ParcoursUseCases.Create;
using Université_Domain.UseCases.ParcoursUseCases.Delete;
using Université_Domain.UseCases.ParcoursUseCases.Get;
using Université_Domain.UseCases.ParcoursUseCases.Update;
using Université_Domain.UseCases.SecurityUseCases.Get;
using UniversiteDomain.Dtos;

namespace UniversiteRestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParcoursController(IRepositoryFactory repositoryFactory) : ControllerBase{


    [HttpGet("{id}")]
    public async Task<ActionResult<ParcoursDto>> getParcours(long id)
    {
        GetUnParcoursUseCase pr = new GetUnParcoursUseCase(repositoryFactory);
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
        
        if (!pr.IsAuthorized(role)) return Unauthorized();

        
        try
        {
            var res = await pr.ExecuteAsync(id);
            ParcoursDto dto = new ParcoursDto();
            var res2=dto.ToDto(res);
            return res2;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
         
    }
    [HttpGet("all")]
    public async Task<ActionResult<List<ParcoursDto>>> getParcours()
    {
        GetAllParcoursUseCase pr = new GetAllParcoursUseCase(repositoryFactory);
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
        
        if (!pr.IsAuthorized(role)) return Unauthorized();

        try
        {
            var res =await pr.ExecuteAsync();
            ParcoursDto dto = new ParcoursDto();
            
            
            return dto.ToDtos(res);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
         
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<Parcours>> deleteParcours(long id) // fix delete cascade error 
    {
        DeleteParcoursUseCase del = new DeleteParcoursUseCase(repositoryFactory);
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
        
        if (!del.IsAuthorized(role)) return Unauthorized();

        try
        {
            await del.ExecuteAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
        return NoContent() ;
    }

    [HttpPost("add")]
    public async Task<ParcoursDto> addParcours(Parcours parcours)
    {
        CreateParcoursUseCase pr = new CreateParcoursUseCase(repositoryFactory);
        ParcoursDto dto = new ParcoursDto();
        var res= dto.ToDto(await pr.ExecuteAsync(parcours));
        return res;
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult<ParcoursDto>> updateParcours(long id, Parcours parcours)
    {
        
        UpdateParcoursUseCase pr = new UpdateParcoursUseCase(repositoryFactory);
        ParcoursDto dto = new ParcoursDto();
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
        
        if (!pr.IsAuthorized(role)) return Unauthorized();
        ParcoursDto res = new ParcoursDto(); 
        try
        {
             res= dto.ToDto(await pr.ExecuteAsync(id, parcours));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        return res;
    }
    
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