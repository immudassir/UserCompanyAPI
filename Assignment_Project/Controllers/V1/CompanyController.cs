using Asp.Versioning;
using Assignment_Project.Data;
using Assignment_Project.Dtos;
using Assignment_Project.Models;
using Assignment_Project.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Assignment_Project.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Company")]
    [ApiVersion("1.0")]

    public class CompanyController : ControllerBase
    {
        private readonly APIResponse _response;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
        {
            this._companyRepository = companyRepository;
            this._mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCompanies()
        {
            var response = new APIResponse();

            try
            {
                var companies = await _companyRepository.GetAllAsync();
                response.Result = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return response;
        }

        [HttpGet("{id}", Name = "GetCompany")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCompany(int id)
        {
            var response = new APIResponse();

            try
            {
                if (id <= 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }

                var company = await _companyRepository.GetAsync(c => c.CompanyId == id);

                if (company == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }

                response.Result = _mapper.Map<CompanyDTO>(company);
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCompany([FromBody] CompanyDTO newCompany)
        {
            var response = new APIResponse();

            try
            {
                if (newCompany == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Company companyEntity = _mapper.Map<Company>(newCompany);
                await _companyRepository.CreateAsync(companyEntity);

                response.Result = _mapper.Map<CompanyDTO>(companyEntity);
                response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCompany", new { id = companyEntity.CompanyId }, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return response;
        }

        [HttpDelete("{id:int}", Name = "DeleteCompany")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCompany(int id)
        {
            var response = new APIResponse();

            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var company = await _companyRepository.GetAsync(c => c.CompanyId == id);

                if (company == null)
                {
                    return NotFound();
                }

                await _companyRepository.RemoveAsync(company);
                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return response;
        }

        [HttpPut("{id:int}", Name = "UpdateCompany")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateCompany(int id, [FromBody] CompanyDTO companyDTO)
        {
            var response = new APIResponse();

            try
            {
                if (companyDTO == null || id != companyDTO.CompanyId)
                {
                    return BadRequest();
                }

                var companyEntity = await _companyRepository.GetAsync(c => c.CompanyId == id);

                if (companyEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(companyDTO, companyEntity);
                await _companyRepository.UpdateAsync(companyEntity);
                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialCompany")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdatePartialCompany(int id, [FromBody] JsonPatchDocument<CompanyDTO> patchDTO)
        {
            var response = new APIResponse();

            try
            {
                if (patchDTO == null || id <= 0)
                {
                    return BadRequest();
                }

                var companyEntity = await _companyRepository.GetAsync(c => c.CompanyId == id);

                if (companyEntity == null)
                {
                    return NotFound();
                }

                var companyDTO = _mapper.Map<CompanyDTO>(companyEntity);
                patchDTO.ApplyTo(companyDTO, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(companyDTO, companyEntity);
                await _companyRepository.UpdateAsync(companyEntity);

                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return response;
        }
    }
}
