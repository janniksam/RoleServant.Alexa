using System;
using Microsoft.AspNetCore.Mvc;
using RoleShuffle.Application.Abstractions.Model;
using RoleShuffle.Application.Abstractions.RoleManager;

namespace RoleShuffle.Web.Controllers
{
    [Route("api/[controller]")]
    public class OneNightUltimateWerewolfController : Controller
    {
        private readonly IOneNightUltimateWerewolfRoleManager m_roleManager;

        public OneNightUltimateWerewolfController(IOneNightUltimateWerewolfRoleManager roleManager)
        {
            m_roleManager = roleManager;
        }

        // GET api/portfolio
        [HttpGet]
        public IActionResult Get()
        {
            var roleSelection = new RoleSelection();
            return View("Index", roleSelection);
        }

        [HttpPost]
        public IActionResult Post(RoleSelection roleSelection)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(roleSelection.PreSelection))
                {
                    roleSelection = m_roleManager.LoadRoles(roleSelection.PreSelection);
                }
                else
                {
                    var deckId = m_roleManager.AddRoleSelection(roleSelection);
                    roleSelection.DeckId = deckId;
                }
            }

            return View("Index", roleSelection);
        }
    }
}