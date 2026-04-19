using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportDesk.Application.Dtos;
using SupportDesk.Application.Interfaces.Services;
using SupportDesk.Domain.Enums;

namespace SupportDesk.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupportTicketsController(ISupportTicketService service) : ControllerBase
    {
        private readonly ISupportTicketService _service = service;

        [HttpPost]
        public async Task<ActionResult<SupportTicketResponse>> Create(
        [FromBody] CreateSupportTicketRequest request,
        CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.CreateAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SupportTicketResponse>>> GetAll(
        [FromQuery] string? clientName,
        [FromQuery] int? priority,
        [FromQuery] int? status,
        CancellationToken cancellationToken)
        {
            if (priority.HasValue && !Enum.IsDefined(typeof(PriorityLevel), priority.Value))
                return BadRequest(new { message = "Priority inválida." });

            if (status.HasValue && !Enum.IsDefined(typeof(TicketStatus), status.Value))
                return BadRequest(new { message = "Status inválido." });

            var filter = new SupportTicketsFilter
            {
                ClientName = clientName,
                Priority = priority.HasValue ? (PriorityLevel)priority.Value : null,
                Status = status.HasValue ? (TicketStatus)status.Value : null
            };

            var result = await _service.GetAllAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<SupportTicketResponse>> UpdateStatus(
        Guid id,
        [FromBody] UpdateTicketStatusRequest request,
        CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.UpdateStatusAsync(id, request, cancellationToken);

                if (result is null)
                    return NotFound();

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}