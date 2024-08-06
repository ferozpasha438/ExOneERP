using AutoMapper;
using CIN.Domain.SchoolMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace CIN.Application.SchoolMgtDtos
{
    [AutoMap(typeof(TblParentMyGallery))]
    public class TblParentMyGalleryDto
    {
        public int Id { get; set; }
        public string RegisterMobile { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public string Path { get; set; }
        public bool IsVedio { get; set; }
    }
}
