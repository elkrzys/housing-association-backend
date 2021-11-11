﻿using System;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HousingAssociation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly BuildingsRepository _buildingsRepository;

        public TestController(BuildingsRepository buildingsRepository)
        {
            _buildingsRepository = buildingsRepository;
        }

        [HttpGet]
        public ActionResult<DateTime> GetCurrentDate()
        {
            return DateTime.Now;
        }

        [HttpPost]
        public ActionResult<int> PostBuilding(Building building)
        {
            var id = _buildingsRepository.Add(building);
            return Ok(id);
        }
    }
}