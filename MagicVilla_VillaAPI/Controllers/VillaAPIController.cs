using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/VillaAPI")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger) 
        {
            _logger = logger;
        }
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all the Villas");
            return Ok(VillaStore.VillaList());

        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Error with id"+id);
                return BadRequest();
            }
            var villa = VillaStore.VillaList().FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> Create([FromBody] VillaDTO villaDTO)
        {
            if (VillaStore.VillaList().FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomeError", "Name cannot be Duplicate");
                return BadRequest(ModelState);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            villaDTO.Id = VillaStore.VillaList().OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList().Add(villaDTO);
            List<VillaDTO> mylist = VillaStore.VillaList();
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);

        }


        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList().FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.VillaList().Remove(villa);
            return NoContent();


        }

        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Update(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null|| id!=villaDTO.Id) {
            
            return BadRequest();
            }
            var villa = VillaStore.VillaList().FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            villa.Name = villaDTO.Name;
            return NoContent();




        }


        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult UpdatePartial(int id, JsonPatchDocument patchDTO)
        {
            if (id==0 || patchDTO==null)
            {

                return BadRequest();
            }
            var villa = VillaStore.VillaList().FirstOrDefault(u => u.Id == id);
            patchDTO.ApplyTo(villa);
            return NoContent();




        }
    }
}