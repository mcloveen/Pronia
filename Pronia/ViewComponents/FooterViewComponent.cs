﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;

namespace Pronia.ViewComponents
{
	public class FooterViewComponent:ViewComponent
	{
		readonly ProniaDBContext _context;

        public FooterViewComponent(ProniaDBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value));
		}
	}
}

