using System;
using System.Threading.Tasks;
using CleanArchitecture.Api.Models;
using CleanArchitecture.Api.Swagger;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.SortProviders;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Sorting;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Authorize]
[Route("/api/v1/[controller]")]
public sealed class TenantController : ApiController
{
    private readonly ITenantService _tenantService;

    public TenantController(
        INotificationHandler<DomainNotification> notifications,
        ITenantService tenantService) : base(notifications)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    [SwaggerOperation("Get a list of all tenants")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<TenantViewModel>>))]
    public async Task<IActionResult> GetAllTenantsAsync(
        [FromQuery] PageQuery query,
        [FromQuery] string searchTerm = "",
        [FromQuery] bool includeDeleted = false,
        [FromQuery] [SortableFieldsAttribute<TenantViewModelSortProvider, TenantViewModel, Tenant>]
        SortQuery? sortQuery = null)
    {
        var tenants = await _tenantService.GetAllTenantsAsync(
            query,
            includeDeleted,
            searchTerm,
            sortQuery);
        return Response(tenants);
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Get a tenant by id")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<TenantViewModel>))]
    public async Task<IActionResult> GetTenantByIdAsync([FromRoute] Guid id)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(id);
        return Response(tenant);
    }

    [HttpPost]
    [SwaggerOperation("Create a new tenant")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
    public async Task<IActionResult> CreateTenantAsync([FromBody] CreateTenantViewModel tenant)
    {
        var tenantId = await _tenantService.CreateTenantAsync(tenant);
        return Response(tenantId);
    }

    [HttpPut]
    [SwaggerOperation("Update an existing tenant")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UpdateTenantViewModel>))]
    public async Task<IActionResult> UpdateTenantAsync([FromBody] UpdateTenantViewModel tenant)
    {
        await _tenantService.UpdateTenantAsync(tenant);
        return Response(tenant);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete an existing tenant")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
    public async Task<IActionResult> DeleteTenantAsync([FromRoute] Guid id)
    {
        await _tenantService.DeleteTenantAsync(id);
        return Response(id);
    }
}