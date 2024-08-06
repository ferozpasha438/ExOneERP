using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SNdDtos
{

    public class SndUtilDto
    {

        public long? Id { get; set; }
    }


    public class SndIntUtilDto
    {

        public int Id { get; set; }
    }


    public class SndResultDto
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMsg { get; set; }
    }







}