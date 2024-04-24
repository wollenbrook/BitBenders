using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitBracket.Models;
using BitBracket.ViewModels;
using Microsoft.AspNetCore.Identity;
using BitBracket.DAL.Abstract;
using BitBracket.DAL.Concrete;
using Newtonsoft.Json.Linq;

namespace BitBracket.Controllers;

//  /api/GuidAPI
[Route("api/[controller]")]
[ApiController]
public class GuidAPIController : ControllerBase
{
    private readonly IGuidBracketRepository _guidBracketRepository;
    public GuidAPIController( IGuidBracketRepository guidBracketRepository)
    {
        _guidBracketRepository = guidBracketRepository;
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetGuidBracket(Guid guid)
    {
        var guidBracket = await _guidBracketRepository.GetGuidBracket(guid);
        if (guidBracket == null)
        {
            return NotFound("GuidBracket not found");
        }
        return Ok(guidBracket);
    }

    [HttpPost]
    public async Task<IActionResult> AddGuidBracket([FromBody] GuidBracketViewModel model)
    {
        var guidBracketLink = new GuidBracket
        {
            Guid = Guid.NewGuid(),
            BracketData = model.bracketData
        };
        if (await _guidBracketRepository.GuidBracketExists(guidBracketLink.Guid))
        {
            return BadRequest("Link already exists or Error has occurred. Please try again.");
        }
        var newGuidBracket = await _guidBracketRepository.AddGuidBracket(guidBracketLink);
        return Ok(newGuidBracket);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGuidBracket([FromBody] GuidBracketViewModel model)
    {
        var guidBracket = new GuidBracket
        {
            Guid = model.guid,
            BracketData = model.bracketData
        };
        var updatedGuidBracket = await _guidBracketRepository.UpdateGuidBracket(guidBracket);
        return Ok(updatedGuidBracket);
    }
}