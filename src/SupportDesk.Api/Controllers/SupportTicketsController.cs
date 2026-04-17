using Microsoft.AspNetCore.Mvc;
using SupportDesk.Application.Dtos;
using SupportDesk.Application.Interfaces.Services;

namespace SupportDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportTicketsController : ControllerBase
    {
        private readonly ISupportTicketService _service;

        public SupportTicketsController(ISupportTicketService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<SupportTicketResponse>> Create(
            [FromBody] CreateSupportTicketRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<SupportTicketResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<SupportTicketResponse>> UpdateStatus(
            Guid id,
            [FromBody] UpdateTicketStatusRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateStatusAsync(id, request, cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}