using Microsoft.AspNetCore.Mvc;
using SpaceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpacesController : ControllerBase
    {
        private static List<Space> spaces;

        static SpacesController()
        {
            spaces = new List<Space>
            {
                new Space
                {
                    Id = 1,
                    Title = "Space 1",
                    DefaultFocusMode = "Focus Mode 1",
                    Projects = "Project 1",
                    Description = "Description for Space 1",
                    Private = true,
                    CreationDate = DateTime.Now.AddDays(-10),
                    LastAccessedDate = DateTime.Now.AddDays(-5),
                    UpdatedDate = DateTime.Now.AddDays(-1),
                    UploadedFiles = "file1.txt",
                    SystemMessage = "System message for Space 1",
                    GroupId = "Group1"
                },
                new Space
                {
                    Id = 2,
                    Title = "Space 2",
                    DefaultFocusMode = "Focus Mode 2",
                    Projects = "Project 2",
                    Description = "Description for Space 2",
                    Private = false,
                    CreationDate = DateTime.Now.AddDays(-20),
                    LastAccessedDate = DateTime.Now.AddDays(-10),
                    UpdatedDate = DateTime.Now.AddDays(-2),
                    UploadedFiles = "file2.txt",
                    SystemMessage = "System message for Space 2",
                    GroupId = "Group2"
                }
            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<Space>> GetSpaces()
        {
            return Ok(spaces);
        }

        [HttpGet("{id}")]
        public ActionResult<Space> GetSpaceById(int id)
        {
            var space = spaces.FirstOrDefault(s => s.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            return Ok(space);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Space>> SearchSpaces([FromQuery] string query)
        {
            var result = spaces.Where(s => s.Title.Contains(query) || s.Description.Contains(query)).ToList();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Space> CreateSpace([FromBody] Space space)
        {
            space.CreationDate = DateTime.Now;
            space.LastAccessedDate = DateTime.Now;
            space.UpdatedDate = DateTime.Now;
            spaces.Add(space);
            return CreatedAtAction(nameof(GetSpaceById), new { id = space.Id }, space);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateSpace(int id, [FromBody] Space updatedSpace)
        {
            var space = spaces.FirstOrDefault(s => s.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            space.Title = updatedSpace.Title;
            space.DefaultFocusMode = updatedSpace.DefaultFocusMode;
            space.Projects = updatedSpace.Projects;
            space.Description = updatedSpace.Description;
            space.Private = updatedSpace.Private;
            space.UpdatedDate = DateTime.Now;
            space.UploadedFiles = updatedSpace.UploadedFiles;
            space.SystemMessage = updatedSpace.SystemMessage;
            space.GroupId = updatedSpace.GroupId;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteSpace(int id)
        {
            var space = spaces.FirstOrDefault(s => s.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            spaces.Remove(space);
            return NoContent();
        }

        [HttpPost("{id}/files")]
        public ActionResult AddFiles(int id, [FromBody] string file)
        {
            var space = spaces.FirstOrDefault(s => s.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            space.UploadedFiles += file;
            return Ok(space);
        }

        [HttpDelete("{id}/files")]
        public ActionResult RemoveFiles(int id, [FromBody] string file)
        {
            var space = spaces.FirstOrDefault(s => s.Id == id);
            if (space == null)
            {
                return NotFound();
            }
            space.UploadedFiles = space.UploadedFiles.Replace(file, string.Empty);
            return Ok(space);
        }
    }
}