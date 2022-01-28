using System;
using System.Collections.Generic;
using AutoMapper;
using eArtRegister.API.Application.Common.Mappings;


namespace eArtRegister.API.Application.JobSpecification.Queries.GetJobSpecification
{
 
    public class JobSpecificationDto : IMapFrom<eArtRegister.API.Domain.Entities.JobSpecification>
    {
        public JobSpecificationDto()
        {
         
        }

        public int Id { get; set; }

        public string Description { get; set; }


    }
}
