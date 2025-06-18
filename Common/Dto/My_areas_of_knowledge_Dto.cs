using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;

namespace Common.Dto
{
    public class My_areas_of_knowledge_Dto
    {
        
        public int ID_knowledge { get; set; }
        public string describtion { get; set; }
    }
}
