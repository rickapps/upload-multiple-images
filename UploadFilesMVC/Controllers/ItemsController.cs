﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;

namespace RickApps.UploadFilesMVC.Controllers
{
    public class ItemsController : BaseController
    {
        public ItemsController(IUnitOfWork uow) : base(uow) { }

        // GET: Items
        public IActionResult Index()
        {
            IEnumerable<Item> list = _repository.Items.GetAll();
            return View(list);
        }
        //public async Task<IActionResult> Index()
        //{
        //    IEnumerable<Item> list = await _repository.Items.GetAllAsync();
        //    return View(list);
        //}

        // GET: Items/Details/5
        public IActionResult Details(int? id)
        {
            Item item = null;
            if (id.HasValue)
            {
                item = _repository.Items.Get(id.Value);
            }
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,Name,Description,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(item);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = new Item();
            //var item = await _context.Item.FindAsync(id);
            //if (item == null)
            //{
            //    return NotFound();
            //}
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,Name,Description,Price")] Item item)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(item);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ItemExists(item.ItemID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(item);
        }

        // GET: Items/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var item = await _context.Item
    //            .FirstOrDefaultAsync(m => m.ItemID == id);
    //        if (item == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(item);
    //    }

    //    // POST: Items/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var item = await _context.Item.FindAsync(id);
    //        _context.Item.Remove(item);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool ItemExists(int id)
    //    {
    //        return _context.Item.Any(e => e.ItemID == id);
    //    }
    }
}
