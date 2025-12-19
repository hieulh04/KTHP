using Cau1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cau1.Controllers
{
    public class NhanVienController : ApiController
    {
        QLNVEntities db = new QLNVEntities();

        public List<Nhanvien> GetNhanviens ()
        {
            return db.Nhanviens.ToList();
        }
        [HttpPost]
        public IHttpActionResult Post(Nhanvien nv)
        {
            if(GetNhanviens().Any(item => item.MaNV == nv.MaNV))
            {
                return BadRequest();
            }
            db.Nhanviens.Add(nv);
            db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult Put(Nhanvien nv)
        {
            Nhanvien findNv = db.Nhanviens.FirstOrDefault(item => item.MaNV == nv.MaNV);
            if (findNv == null)
            {
                return NotFound();
            }
            findNv.HoTen = nv.HoTen;
            findNv.TrinhDo = nv.TrinhDo;
            findNv.Luong = nv.Luong;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(string ma)
        {
            int maNV = int.Parse(ma);   // parse trước

            Nhanvien findNv = db.Nhanviens.FirstOrDefault(item => item.MaNV == maNV);
            if (findNv == null)
            {
                return NotFound();
            }
            db.Nhanviens.Remove(findNv);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public Nhanvien Search(int ma)
        {
            return db.Nhanviens.FirstOrDefault(item => item.MaNV == ma);
            
        }
    }
}
