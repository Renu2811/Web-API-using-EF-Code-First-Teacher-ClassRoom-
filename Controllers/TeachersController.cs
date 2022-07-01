using APIDBLayer;
using APIDBLayer.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_2.ApiModel;

namespace WebAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public TeachersController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<IActionResult> GetTeacher()
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }

            var obj = _context.Teachers.Include(t => t.ClassRoomList).ToList();
            var ob = _mapper.Map<List<Teacher>>(obj);

            return Ok(ob);

            // var ob=await _context.Teachers.ToListAsync();


            //return Ok(ob);
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
         public async Task<ActionResult<Teacher>> GetTeachers(int id)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }
            var teacher = _context.Teachers.Where(t => t.ID == id).Include(cls => cls.ClassRoomList).FirstOrDefault();

            if (teacher == null)
            {
                return NotFound();
            }
            else
            {
                return _mapper.Map<Teacher>(teacher);
            }

        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, TeacherAndClassRoomApiModel teacherAndClassRoomApiModel)
        {

            var obj = _mapper.Map<Teacher>(teacherAndClassRoomApiModel.Teacher);
            var list = _mapper.Map<List<ClassRoom>>(teacherAndClassRoomApiModel.ClassRoomList);
            var ob = _context.Teachers.Where(t => t.ID == id).Include(c => c.ClassRoomList).FirstOrDefault();


            if (id != ob.ID)
            {
                return BadRequest();
            }
            ob.Address = obj.Address;
            ob.Name = obj.Name;
            ob.EmailId = obj.EmailId;

            ob.ClassRoomList = list;

            _context.Update(ob);
            // _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(TeacherAndClassRoomApiModel teacherAndClassRoomApiModel)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'ApiDbContext.Teachers'  is null.");
            }



            //Teacher obj = new Teacher();
            //obj.Name = teacherAndClassRoomApiModel.Teacher.Name;
            //obj.Address = teacherAndClassRoomApiModel.Teacher.Address;
            //obj.EmailId = teacherAndClassRoomApiModel.Teacher.EmailId;

            var obj = _mapper.Map<Teacher>(teacherAndClassRoomApiModel.Teacher);

            var classRoomList = _mapper.Map<List<ClassRoom>>(teacherAndClassRoomApiModel.ClassRoomList);
            //List<ClassRoom> classRoomList = new List<ClassRoom>();


            //foreach(ClassRoomApiModel objApiModel in teacherAndClassRoomApiModel.ClassRoomList)
            //{
            //    classRoomList.Add(new ClassRoom { ClassName = objApiModel.ClassName });


            //}

            obj.ClassRoomList = classRoomList;
            _context.Teachers.Add(obj);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTeachers(int id)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }
            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teachers);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.ID == id)).GetValueOrDefault();
        }

         [HttpPatch("{id}")]
       
        [HttpPatch("{teacherId}")]
        public async Task<IActionResult> PatchTeacher(int teacherId, TeacherApiModel teacherApiModel)
        {
            if (_context.Teachers == null)
            {
                return NoContent();
            }
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher == null)
            {
                return NotFound();
            }
            teacher.Name = teacherApiModel.Name;
            teacher.Address = teacherApiModel.Address;
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
